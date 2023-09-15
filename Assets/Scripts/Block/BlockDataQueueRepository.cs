using System.Collections.Generic;
using Scripts.Block;
using UnityEngine;

namespace SJLGPP.Block
{
    public class BlockDataQueueRepository : IBlockDataQueueRepository
    {
        public BlockDataQueueRepository ()
        {
            _blockTypeQueue = new Queue<BlockType>();
        }

        /// <summary>
        ///     게임에 등장할 블럭의 타입을 미리 만듭니다.
        /// </summary>
        public void LoadBlockTypeData ()
        {
            _blockTypeQueue.Clear();

            for ( var i = 0; i < BLOCK_TYPE_LOAD_COUNT; i++ )
            {
                _blockTypeQueue.Enqueue( IntToBlockType( Random.Range( 0, 5 ) ) );
            }
        }

        /// <summary>
        ///     다음 등장해야할 블럭의 타입을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public BlockType GetNextBlockType ()
        {
            if ( _blockTypeQueue.Count == 0 )
            {
                LoadBlockTypeData();
            }

            //TO DO : 체리블럭은 보스 쓰러뜨려야 나오는거라 임시로 여기에서 설정하고 보스 구현시 수정필요
            if ( _blockTypeQueue.Count == 30 )
            {
                _blockTypeQueue.Dequeue();
                return BlockType.ItemCherry;
            }

            return _blockTypeQueue.Dequeue();
        }

        /// <summary>
        ///     블럭번호를 블럭타입으로 변환합니다. 캐스팅을 하지 않기 위해 사용합니다.
        /// </summary>
        /// <param name="typeNo"></param>
        /// <returns></returns>
        private BlockType IntToBlockType ( int typeNo )
        {
            switch ( typeNo )
            {
                case 0:
                    return BlockType.NormalRed;
                case 1:
                    return BlockType.NormalYellow;
                case 2:
                    return BlockType.NormalBlue;
                case 3:
                    return BlockType.NormalGreen;
                default:
                    return BlockType.NormalPurple;
            }
        }

        #region properties

        private static readonly int BLOCK_TYPE_LOAD_COUNT = 100;
        private readonly Queue<BlockType> _blockTypeQueue;

        #endregion
    }
}