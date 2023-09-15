using UnityEngine;

namespace SJLGPP.Block
{
    public interface IBlockPositionUseCase
    {
        Vector3 ConvertPosition ( Vector2Int index );
        Vector3 ConvertPosition ( int index_x, int index_y );
    }
}