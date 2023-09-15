using Scripts.Block;

namespace SJLGPP.Block
{
    public interface IBlockDataQueueUseCase
    {
        BlockType GetNextBlockType ();
        void LoadBlockTypeData ();
    }
}