using System.Collections.Generic;
using Scripts.Block;
using UnityEngine;
using UnityEngine.U2D;

namespace SJLGPP.Block
{
    public class BlockView : MonoBehaviour, IBlockContract.IBlockView
    {
        #region serialized

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        #endregion

        /// <summary>
        ///     블럭 상태가 변경되면 호출됩니다.
        /// </summary>
        /// <param name="eBlockState"></param>
        public void OnChangeBlockState ( EBlockState eBlockState )
        {
            switch ( eBlockState )
            {
                case EBlockState.IDLE:
                {
                    _spriteRenderer.sprite = GetSprite( _blockType, false );
                    break;
                }
                case EBlockState.SELECTED:
                {
                    _spriteRenderer.sprite = GetSprite( _blockType, true );
                    break;
                }
            }
        }

        /// <summary>
        ///     아틀라스를 셋팅합니다.
        /// </summary>
        /// <param name="atlas"></param>
        public void SetAtlas ( SpriteAtlas atlas )
        {
            _atlas = atlas;
        }

        /// <summary>
        ///     블럭타입이 변경되면 호출됩니다.
        /// </summary>
        /// <param name="blockType"></param>
        public void OnChangeBlockType ( BlockType blockType )
        {
            _blockType = blockType;
            _spriteRenderer.sprite = GetSprite( _blockType, false );
            ;
        }

        /// <summary>
        ///     블럭타입 데이터를 셋팅합니다.
        /// </summary>
        /// <param name="datas"></param>
        public void SetBlockTypeDatas ( Dictionary<BlockType, BlockTypeData> datas )
        {
            _blockTypeDatas = datas;
        }

        /// <summary>
        ///     타입에 맞는 스프라이트를 가져옵니다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        private Sprite GetSprite ( BlockType type, bool selected )
        {
            if ( _atlas == null )
            {
                return null;
            }

            if ( !_blockTypeDatas.ContainsKey( type ) )
            {
                return null;
            }

            return _atlas.GetSprite( selected
                ? _blockTypeDatas[type].SelectedSpriteName
                : _blockTypeDatas[type].IdleSpriteName );
        }

        #region private

        private BlockType _blockType;
        private SpriteAtlas _atlas;
        public Dictionary<BlockType, BlockTypeData> _blockTypeDatas;
        private IBlockContract.IBlockView _blockViewImplementation;

        #endregion
    }
}