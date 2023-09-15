using System.Collections.Generic;
using Scripts.Block;

namespace SJLGPP.Block
{
    public interface IBlockTypeRepository
    {
        BlockTypeData GetBlockTypeData ( BlockType blockType );

        Dictionary<BlockType, BlockTypeData> GetDictionary ();
    }
}