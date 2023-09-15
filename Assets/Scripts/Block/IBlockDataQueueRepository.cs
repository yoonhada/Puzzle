using Scripts.Block;

namespace SJLGPP.Block
{
    public interface IBlockDataQueueRepository
    {
        BlockType GetNextBlockType ();

        void LoadBlockTypeData ();
    }
}