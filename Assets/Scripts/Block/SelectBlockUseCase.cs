using System.Collections.Generic;
using SJLGPP.Combo;
using Zenject;

namespace SJLGPP.Block
{
    public class SelectBlockUseCase
    {
        #region private
        [Inject] private IBoardStateContoller _boardStateContoller;
        [Inject] private IComboController     _comboController;
        #endregion
        
        /// <summary>
        /// 블럭을 드래그 했을때 호출합니다. 드래그 가능한 블럭인지 확인해서 가능하면 드래그 리스트에 추가합니다.
        /// </summary>
        /// <param name="selectedBlocks"></param>
        /// <param name="block"></param>
        public bool Excute(List<IBlockContract.IBlockPresenter> selectedBlocks, IBlockContract.IBlockPresenter block)
        {
            //조건에 맞지 않으면 종료합니다.
            if (!IsValid(selectedBlocks, block))
            {
                return false;
            }
            
            block.SetBlockState(EBlockState.SELECTED);
            selectedBlocks.Add(block);

            //블럭이 처음 선택이 아닌 드래그중일경우 콤보시스템에 알립니다.
            if (selectedBlocks.Count > 0)
            {
                _comboController.OnDragBlock();
            }

            return true;
        }

        /// <summary>
        /// 드래그 조건에 맞는지 확인 합니다.
        /// </summary>
        /// <param name="selectedBlocks"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsValid(List<IBlockContract.IBlockPresenter> selectedBlocks, IBlockContract.IBlockPresenter block)
        {
            if (!_boardStateContoller.AbleToBlockControll)
            {
                return false;
            }

            //선택가능한 블럭타입인지 확인 합니다.
            if (!block.IsSelectable())
            {
                return false;
            }
            
            //이미 드래그한 리스트에 있는지 확인 합니다.
            if (selectedBlocks.Contains(block))
            {
                return false;
            }

            //블럭 상태가 idle상태인지 확인 합니다.
            if (block.BlockState != EBlockState.IDLE)
            {
                return false;
            }
            
            //드래그 리스트에 없으면 나머지 조건체크하지 않고 true 반환합니다.
            if (selectedBlocks.Count == 0)
            {
                return true;
            }
            
            //드래그 리스트에 있는 블럭이 연결가능한 블럭인지 확인 합니다.
            if(!selectedBlocks[0].IsConnectable())
            {
                return false;
            }
            
            // 해당 블럭이 연결가능한 블럭타입인지 확인 합니다.
            if(!block.IsConnectable())
            {
                return false;
            }

            //드래그 리스트에 있는 블럭과 같은 타입인지 확인 합니다.
            if (selectedBlocks[0].BlockType != block.BlockType)
            {
                return false;
            }

            //마지막으로 드래그한 블럭 주위에 위치한 블럭인지 확인 합니다.
            if (!selectedBlocks[^1].IsArround(block.Index))
            {
                return false;
            }

            return true;
        }

  
    }
}