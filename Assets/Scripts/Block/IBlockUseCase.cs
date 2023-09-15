using System;
using System.Collections.Generic;
using Scripts.Block;
using UnityEngine;

namespace SJLGPP.Block
{
    public interface IBlockUseCase
    {
        BlockType BlockType { get; }
        EBlockState BlockState { get; }
        Vector2Int Index { get; }

        Transform CachedTransform { get; }
        GameObject CachedGameObject { get; }
        float DeltaPangSecond { get; set; }

        public IObservable<BlockType> BlockTypeObserveStream { get; }
        public IObservable<EBlockState> BlockStateObserveStream { get; }

        void SetIndex ( int x, int y );
        void SetBlockType ( BlockType type );
        void SetBlockState ( EBlockState state );
        bool IsSelectable ();
        bool IsConnectable ();
        bool IsArround ( Vector2Int index );
        bool IsPangable ();
        void SetObject ( GameObject gameObject );
        Dictionary<BlockType, BlockTypeData> GetBlockTypeDictionary ();
    }
}