using System;
using System.Collections.Generic;
using Scripts.Block;
using UnityEngine;
using UnityEngine.U2D;

namespace SJLGPP.Block
{
    public interface IBlockContract
    {
        public interface IBlockView : IBlockContract
        {
            void SetAtlas ( SpriteAtlas atlas );
            public void OnChangeBlockType ( BlockType blockType );
            public void OnChangeBlockState ( EBlockState blockState );
            public void SetBlockTypeDatas ( Dictionary<BlockType, BlockTypeData> datas );
        }

        public interface IBlockPresenter : IBlockContract
        {
            BlockType BlockType { get; }
            Vector2Int Index { get; }
            EBlockState BlockState { get; }
            GameObject CachedGameObject { get; }
            Transform CachedTransform { get; }
            bool IsNormalBlock { get; }
            float DeltaPangSecond { get; set; }

            void SetIndex ( int x, int y );
            void SetBlockType ( BlockType type );
            void SetBlockState ( EBlockState state );
            void SetPosition ( Vector3 position );
            bool IsSelectable ();
            bool IsConnectable ();
            bool IsArround ( Vector2Int index );
            bool IsPangable ();
            void SetDestroyFunc ( Action<BlockPresenter> func );
        }
    }
}