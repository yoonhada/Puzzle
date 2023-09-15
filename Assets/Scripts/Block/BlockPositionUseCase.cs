using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockPositionUseCase : IBlockPositionUseCase
    {
        #region properties
        [Inject] private BlockConfigureData _blockConfigureData;
        #endregion
        
        /// <summary>
        /// 블럭의 보드인덱스로 position값을 구합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 ConvertPosition(Vector2Int index)
        {
            return ConvertPosition(index.x, index.y);
        }

        /// <summary>
        /// 블럭의 보드인덱스로 position값을 구합니다.
        /// </summary>
        /// <param name="index_x"></param>
        /// <param name="index_y"></param>
        /// <returns></returns>
        public Vector3 ConvertPosition(int index_x, int index_y)
        {
            Vector3 pos = Vector3.zero;
            int inclineOffset = index_x % 2 == 1 ? _blockConfigureData.INCLINE_OFFSET : 0;
            
            pos.x = index_x * _blockConfigureData.BLOCK_HORIZONTAL_DISTANCE + _blockConfigureData.BOARD_LEFT_POSITION;
            pos.y = index_y * _blockConfigureData.BLOCK_VERTICAL_DISTANCE + _blockConfigureData.BOARD_BOTTOM_POSITION + inclineOffset;

            return pos;
        }
    }
}