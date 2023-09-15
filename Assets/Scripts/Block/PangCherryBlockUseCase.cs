using Scripts.Block;

namespace SJLGPP.Block
{
    public class PangCherryBlockUseCase : IPangCherryBlockUseCase
    {
        public BlockType BlockType
        {
            get { return BlockType.ItemCherry; }
        }

        public void OnPang ( IBlockContract.IBlockPresenter block ) { }
    }
}