using Scripts.Block;
using UniRx;
using UnityEngine;

namespace SJLGPP.Block
{
    public interface IBlockModel
    {
        IReadOnlyReactiveProperty<BlockType> BlockType { get; }
        IReadOnlyReactiveProperty<Vector2Int> Index { get; }
        IReadOnlyReactiveProperty<EBlockState> BlockState { get; }

        float DeltaPangSecond { get; set; }

        void SetBlockType ( BlockType blockType );
        void SetIndex ( Vector2Int index );
        void SetBlockState ( EBlockState eBlockState );
    }
}