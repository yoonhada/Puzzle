using System;
using System.Collections.Generic;
using Scripts.Block;
using UniRx;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockUseCase : IBlockUseCase
    {
        public BlockUseCase ( IBlockModel blockModel )
        {
            _blockModel = blockModel;
            BlockTypeObserveStream = _blockModel.BlockType.Share();
            BlockStateObserveStream = _blockModel.BlockState.Share();
        }

        /// <summary>
        ///     블럭의 보드 인덱스를 설정합니다.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetIndex ( int x, int y )
        {
            _blockModel.SetIndex( new Vector2Int( x, y ) );
        }

        /// <summary>
        ///     블럭 상태를 설정합니다.
        /// </summary>
        /// <param name="state"></param>
        public void SetBlockState ( EBlockState state )
        {
            //pang 상태로 만들때 pang 가능한 타입인지 체크
            if ( state is EBlockState.DELTA_PANG or EBlockState.WAITING_PANG && !IsPangable() )
            {
                return;
            }

            _blockModel.SetBlockState( state );
        }

        /// <summary>
        ///     드래그 가능한 블럭타입 인지 확인합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsSelectable ()
        {
            return _blockTypeRepository.GetBlockTypeData( _blockModel.BlockType.Value )?.IsSelectable ?? false;
        }

        /// <summary>
        ///     블럭간 연결 가능한 블럭타입 인지 확인 합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsConnectable ()
        {
            return _blockTypeRepository.GetBlockTypeData( _blockModel.BlockType.Value )?.IsConnectable ?? false;
        }

        /// <summary>
        ///     Pang 가능한 블럭인지 확인 합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsPangable ()
        {
            return _blockTypeRepository.GetBlockTypeData( _blockModel.BlockType.Value )?.IsPangable ?? false;
        }

        /// <summary>
        ///     이 블럭이 매개변수의 인덱스 주변에 있는 블럭인지 확인합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsArround ( Vector2Int index )
        {
            var offsets = _blockModel.Index.Value.x % 2 == 0
                ? _blockConfigureData.AROUND_OFFSET_FOR_EVEN
                : _blockConfigureData.AROUND_OFFSET_FOR_ODD;

            foreach ( var offset in offsets )
            {
                if ( _blockModel.Index.Value + offset == index )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     게임 오브젝트 설정합니다.
        /// </summary>
        /// <param name="gameObject"></param>
        public void SetObject ( GameObject gameObject )
        {
            CachedGameObject = gameObject;
            CachedTransform = gameObject.transform;
        }

        /// <summary>
        ///     블럭타입을 설정합니다.
        /// </summary>
        /// <param name="type"></param>
        public void SetBlockType ( BlockType type )
        {
            _blockModel.SetBlockType( type );
        }

        public Dictionary<BlockType, BlockTypeData> GetBlockTypeDictionary ()
        {
            return _blockTypeRepository.GetDictionary();
        }

        #region properties

        public BlockType BlockType
        {
            get { return _blockModel.BlockType.Value; }
        }
        public Vector2Int Index
        {
            get { return _blockModel.Index.Value; }
        }
        public EBlockState BlockState
        {
            get { return _blockModel.BlockState.Value; }
        }
        public float DeltaPangSecond
        {
            get { return _blockModel.DeltaPangSecond; }
            set { _blockModel.DeltaPangSecond = value; }
        }
        public Transform CachedTransform { get; private set; }
        public GameObject CachedGameObject { get; private set; }

        public IObservable<BlockType> BlockTypeObserveStream { get; }
        public IObservable<EBlockState> BlockStateObserveStream { get; }

        #endregion

        #region private

        private readonly IBlockModel _blockModel;
        [Inject]
        private IBlockTypeRepository _blockTypeRepository;
        [Inject]
        private BlockConfigureData _blockConfigureData;

        #endregion
    }
}