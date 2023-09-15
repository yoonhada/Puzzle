using System.Collections.Generic;
using UnityEngine;

namespace SJLGPP.Block
{
    public class PangWaitingBlockUseCase
    {
        #region private
        private List<Vector2Int> _deleteIndexList;
        #endregion
        
        public PangWaitingBlockUseCase()
        {
            _deleteIndexList = new List<Vector2Int>();
        }

        /// <summary>
        /// wainting pang 상태인 블럭을 pang 상태로 만듭니다.
        /// </summary>
        /// <param name="dictionary"></param>
        public void Excute(Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary)
        {
            foreach (var keyValue in dictionary)
            {
                if (keyValue.Value.BlockState == EBlockState.WAITING_PANG)
                {
                    keyValue.Value.SetBlockState(EBlockState.PANG);
                    _deleteIndexList.Add(keyValue.Key);
                }
            }

            //pang된 블럭은 dictionary에서 제거합니다.
            //빈공간을 채우기 위해 dictionary에서 판단하기 위함입니다.
            foreach (var index in _deleteIndexList)
            {
                dictionary.Remove(index);
            }    
            
            _deleteIndexList.Clear();
        }
        
    }
}