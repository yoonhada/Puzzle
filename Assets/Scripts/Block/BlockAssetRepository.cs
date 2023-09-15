using System;
using Extensions;
using UniRx;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockAssetRepository : IBlockAssetRepository, IDisposable
    {
        public BlockAssetRepository ()
        {
            _disposable = new CompositeDisposable();
        }


        public void LoadBlockAtlas ( Action onComplete )
        {
            _blockAtlasAssetReference.LoadAssetAsObservable<SpriteAtlas>()
                .Subscribe( atlas =>
                {
                    _blockAtlas = atlas;
                    onComplete?.Invoke();
                } )
                .AddTo( _disposable );
        }

        public SpriteAtlas GetBlockAtlas ()
        {
            return _blockAtlas;
        }

        public void Dispose ()
        {
            _disposable?.Dispose();
        }

        #region private

        [Inject]
        private AssetReference _blockAtlasAssetReference;
        private SpriteAtlas _blockAtlas;
        private readonly CompositeDisposable _disposable;

        #endregion
    }
}