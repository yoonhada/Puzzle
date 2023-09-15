using System;
using DG.Tweening;
using Scripts.Block;
using UniRx;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockPresenter : MonoBehaviour, IBlockContract.IBlockPresenter
    {
        private void Awake ()
        {
            _onCompleteToMove = OnCompleteToMove;
            _blockView.SetAtlas( _blockAssetUseCase.GetBlockAtlas() );
            _blockView.SetBlockTypeDatas( _blockUseCase.GetBlockTypeDictionary() );

            SetObservable();
        }

        /// <summary>
        ///     Index를 설정합니다.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetIndex ( int x, int y )
        {
            _blockUseCase.SetIndex( x, y );
        }

        /// <summary>
        ///     블럭 상태를 설정합니다.
        /// </summary>
        /// <param name="state"></param>
        public void SetBlockState ( EBlockState state )
        {
            _blockUseCase.SetBlockState( state );
        }

        /// <summary>
        ///     게임 오브젝트의 포지션을 설정합니다.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition ( Vector3 position )
        {
            CachedTransform.localPosition = position;
        }

        /// <summary>
        ///     선택가능한 블럭 타입인지 체크합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsSelectable ()
        {
            return _blockUseCase.IsSelectable();
        }

        /// <summary>
        ///     연결 가능한 블럭타입인지 체크합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsConnectable ()
        {
            return _blockUseCase.IsConnectable();
        }

        /// <summary>
        ///     이 블럭이 매개변수의 인덱스 주변에 있는 블럭인지 확인합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsArround ( Vector2Int index )
        {
            return _blockUseCase.IsArround( index );
        }

        /// <summary>
        ///     pang 가능한 블럭타입인지 확인 합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsPangable ()
        {
            return _blockUseCase.IsPangable();
        }

        /// <summary>
        ///     블럭이 파괴될때 실행될 함수를 설정합니다.
        /// </summary>
        /// <param name="func"></param>
        public void SetDestroyFunc ( Action<BlockPresenter> func )
        {
            _destroyFunc = func;
        }

        /// <summary>
        ///     블럭타입을 설정합니다.
        /// </summary>
        /// <param name="type"></param>
        public void SetBlockType ( BlockType type )
        {
            _blockUseCase.SetBlockType( type );
        }

        /// <summary>
        ///     생성자
        /// </summary>
        /// <param name="blockUseCase"></param>
        [Inject]
        public void Construct ( IBlockUseCase blockUseCase )
        {
            _blockUseCase = blockUseCase;
            _blockUseCase.SetObject( gameObject );
        }

        /// <summary>
        ///     rx observe를 설정합니다.
        /// </summary>
        private void SetObservable ()
        {
            _blockUseCase.BlockTypeObserveStream.Subscribe( blockType =>
                {
                    _blockView.OnChangeBlockType( blockType );
                } )
                .AddTo( gameObject );

            _blockUseCase.BlockStateObserveStream.Subscribe( blockState =>
                {
                    _blockView.OnChangeBlockState( blockState );
                    OnChangeBlockState( blockState );
                } )
                .AddTo( gameObject );
        }

        /// <summary>
        ///     블럭이 pang 될때 호출됩니다.
        /// </summary>
        private void Pang ()
        {
            _destroyFunc( this );
        }

        /// <summary>
        ///     블럭을 모델에 설정된 Index의 위치로 이동시킵니다.
        /// </summary>
        private void MoveBlock ()
        {
            var curPosition = CachedTransform.localPosition;
            var destPosition = _blockPositionUseCase.ConvertPosition( Index );
            var duration = Mathf.Abs( curPosition.y - destPosition.y ) / _blockConfigureData.BLOCK_VERTICAL_DISTANCE *
                           _blockConfigureData.MOVE_SECOND_PER_ROW;
            duration = Mathf.Clamp( duration, _blockConfigureData.MOVE_SECOND_MIN,
                _blockConfigureData.MOVE_SECOND_MAX );

            CachedTransform.DOLocalMoveY( destPosition.y, duration )
                .SetEase( Ease.OutExpo )
                .OnComplete( _onCompleteToMove );
        }

        /// <summary>
        ///     이동 완료후 Idle 상태로 변경합니다.
        /// </summary>
        private void OnCompleteToMove ()
        {
            _blockUseCase.SetBlockState( EBlockState.IDLE );
        }

        /// <summary>
        ///     블럭이 상태가 변경되면 실행됩니다.
        /// </summary>
        /// <param name="eBlockState"></param>
        private void OnChangeBlockState ( EBlockState eBlockState )
        {
            switch ( eBlockState )
            {
                case EBlockState.PANG:
                {
                    Pang();
                    break;
                }
                case EBlockState.MOVE:
                {
                    MoveBlock();
                    break;
                }
            }
        }

        #region properties

        public BlockType BlockType
        {
            get { return _blockUseCase.BlockType; }
        }
        public Vector2Int Index
        {
            get { return _blockUseCase.Index; }
        }
        public EBlockState BlockState
        {
            get { return _blockUseCase.BlockState; }
        }

        public bool IsNormalBlock
        {
            get { return _blockUseCase.BlockType <= BlockType.NormalPurple; }
        }

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

        public GameObject CachedGameObject
        {
            get
            {
                if ( ReferenceEquals( _cachedGameObject, null ) )
                {
                    _cachedGameObject = gameObject;
                }

                return _cachedGameObject;
            }
        }

        /// <summary>
        ///     delta pang 상태일때 남은 시간(초)
        /// </summary>
        public float DeltaPangSecond
        {
            get { return _blockUseCase.DeltaPangSecond; }
            set { _blockUseCase.DeltaPangSecond = value; }
        }

        #endregion

        #region private

        [Inject]
        private IBlockPositionUseCase _blockPositionUseCase;
        [Inject]
        private BlockConfigureData _blockConfigureData;
        [Inject]
        private IBlockAssetUseCase _blockAssetUseCase;
        [Inject]
        private IBlockContract.IBlockView _blockView;
        private IBlockUseCase _blockUseCase;
        private Transform _cachedTransform;
        private GameObject _cachedGameObject;
        private Action<BlockPresenter> _destroyFunc;
        private TweenCallback _onCompleteToMove;

        #endregion
    }
}