using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockConnect : MonoBehaviour
    {
        #region const

        private static readonly string SPRITE_NAME = "blockConnect";

        #endregion

        #region serialize

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        #endregion

        [Inject]
        public void Construct ( IBlockAssetUseCase blockAssetUseCase )
        {
            _spriteRenderer.sprite = blockAssetUseCase.GetBlockAtlas()?.GetSprite( SPRITE_NAME );
        }

        #region properties

        public Transform CachedTransform
        {
            get
            {
                if ( ReferenceEquals( _cachedTransform, null ) )
                {
                    _cachedTransform = transform;
                }

                return _cachedTransform;
            }
        }

        private Transform _cachedTransform;

        #endregion
    }
}