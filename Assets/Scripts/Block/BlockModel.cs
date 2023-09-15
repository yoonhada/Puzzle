using Scripts.Block;
using UniRx;
using UnityEngine;

namespace SJLGPP.Block
{
    public class BlockModel : IBlockModel
    {
        public BlockModel ()
        {
            _blockType = new ReactiveProperty<BlockType>();
            _index = new ReactiveProperty<Vector2Int>();
            _blockState = new ReactiveProperty<EBlockState>();
        }

        /// <summary>
        ///     블럭의 보드 인덱스를 설정합니다.
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex ( Vector2Int index )
        {
            _index.Value = index;
        }

        /// <summary>
        ///     블럭상태를 설정합니다.
        /// </summary>
        /// <param name="eBlockState"></param>
        public void SetBlockState ( EBlockState eBlockState )
        {
            _blockState.Value = eBlockState;
        }

        /// <summary>
        ///     블럭타입을 설정합니다.
        /// </summary>
        /// <param name="blockType"></param>
        public void SetBlockType ( BlockType blockType )
        {
            _blockType.Value = blockType;
        }

        #region properties

        public IReadOnlyReactiveProperty<BlockType> BlockType
        {
            get { return _blockType; }
        }
        private readonly ReactiveProperty<BlockType> _blockType;
        public IReadOnlyReactiveProperty<Vector2Int> Index
        {
            get { return _index; }
        }
        private readonly ReactiveProperty<Vector2Int> _index;
        public IReadOnlyReactiveProperty<EBlockState> BlockState
        {
            get { return _blockState; }
        }
        private readonly ReactiveProperty<EBlockState> _blockState;
        public float DeltaPangSecond { get; set; }

        #endregion
    }
}