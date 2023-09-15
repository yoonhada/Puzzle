using System.Collections.Generic;
using Zenject;

namespace SJLGPP.Block
{
    public class SetDeltaPangSelectedBlockUseCase
    {
        #region private
        [Inject]private BlockConfigureData _blockConfigureData;
        #endregion

        /// <summary>
        /// 드래그한 블럭중 delta pang 조건에 맞는 블럭을 delta pang상태로 만듭니다.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="selectedPangBlocks"></param>
        public void Excute(List<IBlockContract.IBlockPresenter> selectedPangBlocks)
        {
            if (selectedPangBlocks.Count == 0)
            {
                return;
            }

            int order = 0;
            foreach (var block in selectedPangBlocks)
            {
                //delta pang 조건에 맞는 블럭은 delta pang 상태로 만들고 시간을 설정합니다.
                if(order >= _blockConfigureData.DELTAPANG_START_ORDER) 
                {
                    block.SetBlockState(EBlockState.DELTA_PANG);
                    block.DeltaPangSecond = _blockConfigureData.DELTAPANG_FIRST_DELAY + _blockConfigureData.DELTAPANG_LEFT_DELAY * (order - _blockConfigureData.DELTAPANG_START_ORDER);
                }
                order++;
            }
            
            selectedPangBlocks.Clear();
        }
        
    }
}