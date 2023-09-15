using System.Collections.Generic;
using SJLGPP.Combo;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class PangSelectedBlockAroundUseCase
    {
        #region private
        [Inject] private BlockConfigureData _blockConfigureData;
        [Inject] private IComboController    _comboController;
        #endregion
        
        public void Excute(List<IBlockContract.IBlockPresenter> selectedPangBlocks, Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary)
        {
            //피버모드가 아니라면 종료합니다.
            if (!_comboController.IsFeverMode)
            {
                return;
            }
            
            //선택된 블럭이 없으면 종료합니다.
            if (selectedPangBlocks.Count == 0)
            {
                return;
            }

            SetPangBlockAround(selectedPangBlocks, dictionary);
        }

        /// <summary>
        /// 선택된 블럭 주위를 pang가능 상태로 만듭니다.
        /// </summary>
        /// <param name="selectedPangBlocks"></param>
        /// <param name="???"></param>
        private void SetPangBlockAround(List<IBlockContract.IBlockPresenter> selectedPangBlocks, Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary)
        {
            IBlockContract.IBlockPresenter tmpBlock = null;
            foreach (var block in selectedPangBlocks)
            {
                for (int direction = _blockConfigureData.BLOCK_DIRECTION_INDEX_UP;
                     direction <= _blockConfigureData.BLOCK_DIRECTION_INDEX_RIGHT_UP;
                     direction++)
                {
                    Vector2Int neighborIndex = _blockConfigureData.GetNextDirectionIndex(block.Index, direction);
                    if (dictionary.ContainsKey(neighborIndex))
                    {
                        tmpBlock = dictionary[neighborIndex];
                        if (IsValidWaitingPang(tmpBlock))
                        {
                            tmpBlock.SetBlockState(EBlockState.WAITING_PANG);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 블럭이 pang 가능상태가 될 수 있는지 확인 합니다.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsValidWaitingPang(IBlockContract.IBlockPresenter block)
        {
            return block != null
                   && block.IsNormalBlock
                   && block.BlockState == EBlockState.IDLE;
        }
    }
}