using Scripts.Block;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockDataQueueUseCase : IBlockDataQueueUseCase
    {
        #region properties

        [Inject]
        private IBlockDataQueueRepository _repository;

        #endregion

        /// <summary>
        ///     게임에 등장할 블럭의 타입을 미리 만듭니다.
        /// </summary>
        public void LoadBlockTypeData ()
        {
            _repository.LoadBlockTypeData();
        }

        /// <summary>
        ///     다음 등장해야할 블럭의 타입을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public BlockType GetNextBlockType ()
        {
            return _repository.GetNextBlockType();
        }
    }
}