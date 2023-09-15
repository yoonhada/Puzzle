using System;
using UnityEngine.U2D;

namespace SJLGPP.Block
{
    public interface IBlockAssetRepository
    {
        void        LoadBlockAtlas(Action onComplete);
        SpriteAtlas GetBlockAtlas();
    }
}