using Scripts.Block;

namespace SJLGPP.Block
{
    public interface IPangIndestructibleBlockUseCase
    {
        BlockType BlockType { get; }
        void OnPang ( IBlockContract.IBlockPresenter block );
    }
}