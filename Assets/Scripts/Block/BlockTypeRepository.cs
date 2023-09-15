using System.Collections.Generic;
using Scripts.Block;

namespace SJLGPP.Block
{
    public class BlockTypeRepository : IBlockTypeRepository
    {
        public BlockTypeRepository ( BlockTypeDatas blockTypeDatas )
        {
            _blockTypeDatas = blockTypeDatas;
            _blockTypeDictionary = new Dictionary<BlockType, BlockTypeData>();

            MappingBlockTypeData();
        }

        /// <summary>
        ///     블럭타입의 데이터를 가져옵니다.
        /// </summary>
        /// <param name="blockType"></param>
        /// <returns></returns>
        public BlockTypeData GetBlockTypeData ( BlockType blockType )
        {
            return _blockTypeDictionary[blockType];
        }

        public Dictionary<BlockType, BlockTypeData> GetDictionary ()
        {
            return _blockTypeDictionary;
        }

        /// <summary>
        ///     블럭타입 데이터들을 dictionary로 저장합니다.
        /// </summary>
        public void MappingBlockTypeData ()
        {
            _blockTypeDictionary.Clear();

            foreach ( var data in _blockTypeDatas.datas )
            {
                _blockTypeDictionary.Add( data.BlockType, data );
            }
        }

        #region properties

        private readonly Dictionary<BlockType, BlockTypeData> _blockTypeDictionary;
        private readonly BlockTypeDatas _blockTypeDatas;

        #endregion
    }
}