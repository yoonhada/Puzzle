using System;
using Scripts.Block;
using UniRx;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class PangBombBlockUseCase : IPangBombBlockUseCase
    {
        public BlockType BlockType
        {
            get { return BlockType.ItemBomb; }
        }

        /// <summary>
        ///     폭탄이 터지는 처리 입니다.
        /// </summary>
        /// <param name="block"></param>
        public void Pang ( IBlockContract.IBlockPresenter block )
        {
            //보드에 연출 상태중이라고 설정합니다.
            _boardStateContoller.SetBombEffectState();

            //폭탄 주위 블럭을 순차적으로 터뜨립니다.
            var maxOrder = 0;
            for ( var direction = _blockConfigureData.BLOCK_DIRECTION_INDEX_UP;
                 direction <= _blockConfigureData.BLOCK_DIRECTION_INDEX_RIGHT_UP;
                 direction++ )
            {
                //폭탄 주위 특정 방향으로 하나씩 재귀를 돌아 터뜨립니다.
                var lastOrder = PangToNextDirection( block, direction, 0 );

                //마지막 터진 순서를 설정합니다.
                maxOrder = Mathf.Max( maxOrder, lastOrder );
            }

            //마지막 순서 터지는 시간 후에 보드에 연출 완료 상태로 설정합니다.
            var completeTime = TimeSpan.FromSeconds( maxOrder * _blockConfigureData.DELTAPANG_LEFT_DELAY );

            Observable.Timer( completeTime )
                .Subscribe( _ => _boardStateContoller.CompleteBombEffectState() )
                .AddTo( block.CachedGameObject );
        }

        /// <summary>
        ///     기준이 되는 블럭 주위의 특정 방향 블럭을 delta pang 상태로 만듭니다.
        ///     다음 블럭이 없을때까지 재귀로 돌아 실행됩니다.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="directionIndex"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private int PangToNextDirection ( IBlockContract.IBlockPresenter block, int directionIndex, int order )
        {
            //순서를 중가합니다.
            order++;

            //특정방향의 다음 블럭을 찾습니다.
            var nextBlockIndex = _blockConfigureData.GetNextDirectionIndex( block.Index, directionIndex );
            var nextBlock = _blockContainer.FindBlock( nextBlockIndex );

            //블럭이 없으면 재귀를 종료하고 순서를 반환합니다.
            if ( nextBlock == null )
            {
                return order;
            }

            //일반블럭이면 delta pang 상태로 만듭니다.
            if ( nextBlock.BlockType <= BlockType.NormalPurple )
            {
                nextBlock.SetBlockState( EBlockState.DELTA_PANG );
                nextBlock.DeltaPangSecond = _blockConfigureData.DELTAPANG_LEFT_DELAY * order;
            }

            //재귀를 호출합니다.
            return PangToNextDirection( nextBlock, directionIndex, order );
        }

        #region private

        [Inject]
        private IBoardStateContoller _boardStateContoller;
        [Inject]
        private IBlockContainer _blockContainer;
        [Inject]
        private BlockConfigureData _blockConfigureData;

        #endregion
    }
}