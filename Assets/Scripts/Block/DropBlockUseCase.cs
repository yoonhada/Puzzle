using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class DropBlockUseCase
    {
        #region private
        [Inject] private BlockConfigureData                             _blockConfigureData;
        private          Func<int, int, IBlockContract.IBlockPresenter> _createBlockFunc;
        #endregion

        /// <summary>
        /// 블럭 생성 함수를 설정합니다.
        /// </summary>
        /// <param name="createBlockFunc"></param>
        public void SetCreatingBlockFunc(Func<int, int, IBlockContract.IBlockPresenter> createBlockFunc)
        {
            _createBlockFunc = createBlockFunc;
        }

        /// <summary>
        /// 블럭의 빈공간이 생기면 남은 블럭들을 빈공간으로 내리고 신규 블럭을 생성합니다.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="createBlockFunc"></param>
        public void Excute(Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary)
        {
            Vector2Int index = new ();
            IBlockContract.IBlockPresenter indicateBlock;
            int indicate;
            int empty;
            for (int i = 0; i < _blockConfigureData.BOARD_COLUMN; i++)
            {
                indicate = -1;
                empty = -1;
                index.x = i;
                index.y = 0;
                
                //아래부터 위로 한칸씩 아래에 빈공간이 있는 블럭들을 떨어뜨립니다.
                while (indicate < _blockConfigureData.BOARD_ROW)
                {
                    indicate++;
                    index.y = indicate;
                    bool hasIndicatedBlock = dictionary.ContainsKey(index);
                    //검사하는 행에 블럭이 있지만 빈공간이 없으면 넘어갑니다.
                    if (hasIndicatedBlock && empty == -1)
                    {
                        continue;
                    }

                    //검사하는 행에 블럭이 있는 상태에서 아래에 빈공간이 있으면 블럭을 떨어뜨립니다.
                    //빈공간이 채워지면 empty를 한칸 올립니다.
                    if (hasIndicatedBlock)
                    {
                        indicateBlock = dictionary[index];
                        indicateBlock.SetIndex(i, empty);
                        SetMoveState(indicateBlock);
                        empty++;
                    }
                    else
                    {
                        if (empty == -1)
                        {
                            empty = indicate;
                        }
                    }
                }
                
                //해당 열에 빙곤간이 없으면 해당열 검사는 종료합니다.
                if(empty == -1)
                    continue;

                //빈공간을 기존블럭을 내려서 채웠다면 남은 빈공간에는 신규 블럭으로 채웁니다.
                int offset = _blockConfigureData.BOARD_ROW - empty;
                while (empty < _blockConfigureData.BOARD_ROW)
                {
                    CreateBlock(i, empty, empty + offset);
                    empty++;
                }
            }
        }
        
        /// <summary>
        /// 블럭상태를 move 상태로 만듭니다.
        /// </summary>
        /// <param name="block"></param>
        private void SetMoveState(IBlockContract.IBlockPresenter block)
        {
            ///delta pang인 블럭은 상태를 변경하면 안되기 때문에 index만 수정하고 상태를 변경하지 않습니다.
            if (block.BlockState == EBlockState.DELTA_PANG)
            {
                return;
            }

            //이미 move상태인 블럭을 다시 move로 하면 변경 이벤트가 오지 않아 다른 상태로 변경후 다시 move 변경합니다.
            if (block.BlockState == EBlockState.MOVE)
            {
                block.SetBlockState(EBlockState.PAUSE_MOVE);
            }
        
            block.SetBlockState(EBlockState.MOVE);
        }

        /// <summary>
        /// 특정 위치에 블럭 생성합니다.
        /// </summary>
        /// <param name="index_x"></param>
        /// <param name="index_y"></param>
        /// <param name="from_y"></param>
        private void CreateBlock(int index_x, int index_y, int from_y)
        {
            IBlockContract.IBlockPresenter block = _createBlockFunc(index_x, from_y);
            block.SetIndex(index_x, index_y);
            block.SetBlockState(EBlockState.MOVE);
        }
    }
}