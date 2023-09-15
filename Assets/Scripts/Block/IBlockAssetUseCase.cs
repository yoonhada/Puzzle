using System;
using UnityEngine.U2D;

namespace SJLGPP.Block
{
    public interface IBlockAssetUseCase
    {
        void LoadAsset ( Action onComplete );
        SpriteAtlas GetBlockAtlas ();
    }
}