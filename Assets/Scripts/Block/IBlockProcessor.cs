namespace SJLGPP.Block
{
    public interface IBlockProcessor
    {
        void OnEndResourceLoading();
        void SelectBlock(IBlockContract.IBlockPresenter block);
        void ReleaseBlock();
    }
}