using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockProcessor : IBlockProcessor, IInitializable, ITickable, IDisposable
    {
        #region private
        private          List<IBlockContract.IBlockPresenter>                   _selectedBlocks;
        private          List<IBlockContract.IBlockPresenter>                   _selectedPangBlocks;
        private          Dictionary<Vector2Int, IBlockContract.IBlockPresenter> _tempBlockDictionary;
        private          IBlockContainer                                        _blockContainer;
        private          Func<int, int, IBlockContract.IBlockPresenter>         _createBlockFunc;
        [Inject] private IBoardStateContoller                                   _boardStateContoller;
        [Inject] private SelectBlockUseCase                                     _selectBlockUseCase;
        [Inject] private ReleaseSelectedBlockUseCase                            _releaseSelectedBlockUseCase;
        [Inject] private PangSelectedBlockAroundUseCase                         _pangSelectedBlockAroundUseCase;
        [Inject] private SetDeltaPangSelectedBlockUseCase                       _setDeltaPangSelectedBlockUseCase;
        [Inject] private PangWaitingBlockUseCase                                _pangWaitingBlockUseCase;
        [Inject] private DropBlockUseCase                                       _dropBlockUseCase;
        [Inject] private BlockConnectUseCase                                    _blockConnectUseCase;
        [Inject] private PangNonNormalBlockUseCaseFacade                        _pangNonNormalBlockUseCaseFacade;
        [Inject] private PangIndestructibleBlockUseCaseFacade                   _pangIndestructibleBlockUseCaseFacade;
        #endregion

        public BlockProcessor(IBlockContainer blockContainer)
        {
            _blockContainer  = blockContainer;
            _createBlockFunc = _blockContainer.AddBlock;

            _selectedBlocks      = new List<IBlockContract.IBlockPresenter>();
            _selectedPangBlocks  = new List<IBlockContract.IBlockPresenter>();
            _tempBlockDictionary = new Dictionary<Vector2Int, IBlockContract.IBlockPresenter>();
        }

        public void Initialize()
        {
            _dropBlockUseCase.SetCreatingBlockFunc(_createBlockFunc);
            //TO DO : 게임 상태가 정해진다면 SKIP으로 처리하는게 아닌 상태로 거를수 있도록 해야 합니다.
            _boardStateContoller.IgnoreDropBlockProcess.Skip(1).Subscribe(ignoreDropBlockProcess => { if(!ignoreDropBlockProcess)ForceDropBlock(); });
        }

        public void Tick()
        {
            //delta Pang 상태의 블럭을 업데이트 합니다. delta pang 시간이 완료된 블럭은 waiting Pang 상태가 됩니다.
            _blockContainer.UpdateDeltaPangBlock(Time.unscaledDeltaTime);
            
            //waiting pang 상태가 된 블럭이 없으면 종료합니다. 
            if (!_blockContainer.HasWaitingPangBlock())
            {
                return;
            }

            //현재 블럭상태를 dictionary로 업데이트 합니다.
            _blockContainer.GetBlockDictionay(_tempBlockDictionary);
            //피버모드라면 선택한 주변블럭도 같이 터지게 만듭니다.
            _pangSelectedBlockAroundUseCase.Excute(_selectedPangBlocks, _tempBlockDictionary);
            //드래그한 블럭중에서 delta pang 조건에 맞는 블럭은 delta pang 상태로 만듭니다.
            _setDeltaPangSelectedBlockUseCase.Excute(_selectedPangBlocks);
            //특수블럭(폭탄)을 pang상태로 만들고 특수 로직을 실행시킵니다.
            _pangNonNormalBlockUseCaseFacade.Excute(_tempBlockDictionary);
            //waiting pang 상태의 블럭을 pang 상태로 만듭니다. 
            _pangWaitingBlockUseCase.Excute(_tempBlockDictionary);
            //파괴 불가능 블럭이 바닥에 있으면 pang 상태로 만듭니다.
            _pangIndestructibleBlockUseCaseFacade.Excute(_tempBlockDictionary);
            //pang 상태가 되어 파괴된 블럭의 빈자리를 다른 블럭을 채웁니다.
            if (!_boardStateContoller.IgnoreDropBlockProcess.Value)
            {
                _dropBlockUseCase.Excute(_tempBlockDictionary);
            }
        }

        public void Dispose()
        {
            //TO DO :  게임 리소스 정리
        }

        /// <summary>
        /// 리소스 로딩이 완료되었을때 호출됩니다.
        /// </summary>
        public void OnEndResourceLoading()
        {
            _blockConnectUseCase.Initialize();
            _blockContainer.MakeBlockToBoard();
        }

        /// <summary>
        /// 강제로 블럭을 드랍해야 할때 사용
        /// </summary>
        private void ForceDropBlock()
        {
            _blockContainer.GetBlockDictionay(_tempBlockDictionary);
            _dropBlockUseCase.Excute(_tempBlockDictionary);
        }

        /// <summary>
        /// 블럭을 드래그 했을때 호출됩니다. select 상태로 만듭니다.
        /// </summary>
        /// <param name="block"></param>
        public void SelectBlock(IBlockContract.IBlockPresenter block)
        {
            bool added = _selectBlockUseCase.Excute(_selectedBlocks, block);

            if (added && _selectedBlocks.Count > 1)
            {
                ConnectionBlock(_selectedBlocks[^2], _selectedBlocks[^1]);
            }
        }

        /// <summary>
        /// 블럭 사이에 연결선을 만듭니다.
        /// </summary>
        /// <param name="startBlock"></param>
        /// <param name="endBlock"></param>
        private void ConnectionBlock(IBlockContract.IBlockPresenter startBlock, IBlockContract.IBlockPresenter endBlock)
        {
            _blockConnectUseCase.SetConnection(startBlock.CachedTransform.localPosition, endBlock.CachedTransform.localPosition);
        }

        /// <summary>
        /// 드래그 종료시 호출됩니다. 룰에 따라 waiting pang 이나 idle 상태로 만듭니다.
        /// </summary>
        public void ReleaseBlock()
        {
            _releaseSelectedBlockUseCase.Excute(_selectedBlocks, _selectedPangBlocks);
            _blockConnectUseCase.RemoveAll();
        }
    }
}