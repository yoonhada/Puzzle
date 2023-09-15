using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockContainer : IBlockContainer
    {
        #region private
        private          DIObjectPool<BlockPresenter> _blockPool;
        private          IBlockDataQueueUseCase       _blockDataQueueUseCase;
        private          BlockConfigureData           _blockConfigureData;
        [Inject] private IBlockPositionUseCase        _blockPositionUseCase;
        #endregion
        
        
        private List<IBlockContract.IBlockPresenter> _blocks;

        private Predicate<IBlockContract.IBlockPresenter> _findPangBlockMatch = x => x.BlockState == EBlockState.WAITING_PANG;
        private Action<BlockPresenter> _destroyBlockFunc;
        
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="blockPool"></param>
        public BlockContainer(DIObjectPool<BlockPresenter> blockPool, IBlockDataQueueUseCase blockDataQueueUseCase, BlockConfigureData blockConfigureData)
        {
            _blockPool             = blockPool;
            _blockDataQueueUseCase = blockDataQueueUseCase;
            _blockConfigureData    = blockConfigureData;
            _blocks                = new List<IBlockContract.IBlockPresenter>();
            _destroyBlockFunc      = DestroyBlock;
            
            _blockDataQueueUseCase.LoadBlockTypeData();
        }

        /// <summary>
        /// 초기화 합니다.
        /// </summary>
        private void Initialize()
        {
            _blockPool.Initialize(_blockConfigureData.BLOCK_INITIALIZE_COUNT);
        }

        /// <summary>
        /// 블럭을 보드에 생성합니다.
        /// </summary>
        public void MakeBlockToBoard()
        {
            Initialize();
            
            for (int row = 0; row < _blockConfigureData.BOARD_ROW; row++)
            {
                for (int column = 0; column < _blockConfigureData.BOARD_COLUMN; column++)
                {
                    AddBlock(column, row);
                }
            }
        }

        /// <summary>
        /// 블럭을 생성하고 리스트에 추가합니다.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="blockType"></param>
        public IBlockContract.IBlockPresenter AddBlock(int x, int y)
        {
            IBlockContract.IBlockPresenter block = _blockPool.Get();
            block.SetIndex(x, y);
            block.SetBlockType(_blockDataQueueUseCase.GetNextBlockType());
            block.SetBlockState(EBlockState.IDLE);
            block.SetPosition(_blockPositionUseCase.ConvertPosition(x, y));
            block.SetDestroyFunc(_destroyBlockFunc);
            _blocks.Add(block);

            return block;
        }

        /// <summary>
        /// GameObject를 블럭리스트에 검사해서 맞는 블럭을 반환
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IBlockContract.IBlockPresenter FindBlock(GameObject obj)
        {
            return _blocks.Find(x => x.CachedGameObject.Equals(obj));
        }

        public IBlockContract.IBlockPresenter FindBlock(Vector2Int index)
        {
            return _blocks.Find(x => x.Index == index);
        }

        /// <summary>
        /// 블럭리스트중에 waitingPang 상태를 가진 블럭이 있는지 조사합니다.
        /// </summary>
        /// <returns></returns>
        public bool HasWaitingPangBlock()
        {
            return _blocks.Exists(_findPangBlockMatch);
        }

        /// <summary>
        /// 블럭을 파괴합니다.
        /// 실제로는 블럭을 풀에 다시 돌려놓습니다.
        /// </summary>
        /// <param name="blockPresenter"></param>
        public void DestroyBlock(BlockPresenter blockPresenter)
        {
            _blocks.Remove(blockPresenter);
            _blockPool.Release(blockPresenter);
        }

        /// <summary>
        /// 현재 블럭을 딕셔너리 형태로 업데이트 합니다.
        /// 업데이트 한 상태의 index값을 키값으로 사용하고 있어 내부적으로 index가 바뀌어도 이미 key로 설정된 index를 사용할 수 있습니다.
        /// </summary>
        /// <param name="dictionary"></param>
        public void GetBlockDictionay(Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary)
        {
            dictionary.Clear();

            foreach (var block in _blocks)
            {
                dictionary.Add(block.Index, block);
            }
        }

        /// <summary>
        /// deltaPang 상태의 블럭을 업데이트 합니다.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateDeltaPangBlock(float deltaTime)
        {
            foreach (var block in _blocks)
            {
                if (block.BlockState == EBlockState.DELTA_PANG)
                {
                    block.DeltaPangSecond -= deltaTime;

                    if (block.DeltaPangSecond <= 0)
                    {
                        block.DeltaPangSecond = 0;
                        block.SetBlockState(EBlockState.WAITING_PANG);
                    }
                }
            }
        }
    }
    
}