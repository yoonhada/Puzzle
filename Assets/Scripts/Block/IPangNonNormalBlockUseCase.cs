using Scripts.Block;

namespace SJLGPP.Block
{
    public interface IPangNonNormalBlockUseCase
    {
        BlockType BlockType { get; }
        void Pang ( IBlockContract.IBlockPresenter block );
    }
}