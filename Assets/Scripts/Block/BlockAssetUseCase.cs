using System;
using UniRx;
using UnityEngine.U2D;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockAssetUseCase : IBlockAssetUseCase
    {
        #region private
        [Inject] private IBlockAssetRepository _assetRepository;
        #endregion
        
        public void LoadAsset(Action onComplete) => _assetRepository.LoadBlockAtlas(onComplete);

        public SpriteAtlas GetBlockAtlas() => _assetRepository.GetBlockAtlas();
    }
}