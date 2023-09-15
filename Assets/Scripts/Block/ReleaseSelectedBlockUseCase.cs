using System.Collections.Generic;
using Scripts.Block;
using SJLGPP.Combo;
using Zenject;

namespace SJLGPP.Block
{
    public class ReleaseSelectedBlockUseCase
    {
        /// <summary>
        ///     블럭 드래그를 종료했을때 호출됩니다.
        ///     pang 조건에 부합하면 waiting pang 상태로 만들고 부합하지 않으면 idle 상태로 만듭니다.
        /// </summary>
        /// <param name="selectedBlocks"></param>
        /// <param name="selectedPangBlocks"></param>
        public void Excute ( List<IBlockContract.IBlockPresenter> selectedBlocks,
            List<IBlockContract.IBlockPresenter> selectedPangBlocks )
        {
            if ( IsValidPang( selectedBlocks ) )
            {
                SetPangState( selectedBlocks, selectedPangBlocks );
                _comboController.AddComboCount();
            }
            else
            {
                SetIdleState( selectedBlocks );
            }

            selectedBlocks.Clear();
        }

        /// <summary>
        ///     블럭이 pang 조건에 맞는지 확인 합니다.
        /// </summary>
        /// <param name="selectedBlocks"></param>
        /// <returns></returns>
        private bool IsValidPang ( List<IBlockContract.IBlockPresenter> selectedBlocks )
        {
            if ( selectedBlocks.Count == 0 )
            {
                return false;
            }

            if ( !_boardStateContoller.AbleToBlockControll )
            {
                return false;
            }

            return !selectedBlocks[0].IsConnectable() || ( selectedBlocks[0].IsConnectable() &&
                                                           selectedBlocks.Count >=
                                                           _blockConfigureData.BLOCK_MIN_CONNECTION );
        }

        /// <summary>
        ///     선택된 블럭을 pang 가능한 상태로 만듭니다.
        ///     드래그 갯수 체크해서 폭탄 블럭을 만듭니다.
        /// </summary>
        /// <param name="selectedBlocks"></param>
        /// <param name="selectedPangBlocks"></param>
        private void SetPangState ( List<IBlockContract.IBlockPresenter> selectedBlocks,
            List<IBlockContract.IBlockPresenter> selectedPangBlocks )
        {
            selectedPangBlocks.Clear();

            var order = 0;
            var totalCount = selectedBlocks.Count;
            var shouldChangeBlockByLongestConnecting = ShouldChangeBlockByLongestConnecting( totalCount );
            foreach ( var block in selectedBlocks )
            {
                order++;

                if ( shouldChangeBlockByLongestConnecting && order == totalCount )
                {
                    //길게 드래그 했을때 폭탄을 생성하는 조건이 된다면 마지막블럭은 폭탄생성
                    block.SetBlockState( EBlockState.IDLE );
                    block.SetBlockType( GetBlockTypeWhenLongestConnecting( order ) );
                }
                else
                {
                    block.SetBlockState( EBlockState.WAITING_PANG );
                    //드래그 한 블럭이 pang 할 수 있는 상태라면 pang 리스트에 넣습니다.
                    //드래그한 순서가 필요하기 때문에 리스트 만듭니다.
                    selectedPangBlocks.Add( block );
                }
            }
        }

        /// <summary>
        ///     선택한 블럭을 idle 상태로 만듭니다.
        /// </summary>
        /// <param name="selectedBlocks"></param>
        private void SetIdleState ( List<IBlockContract.IBlockPresenter> selectedBlocks )
        {
            foreach ( var block in selectedBlocks )
            {
                block.SetBlockState( EBlockState.IDLE );
            }
        }

        /// <summary>
        ///     선택된 블럭의 조건(길게 드래그)으로 블럭이 변경되는지(폭탄 생성) 체크합니다.
        /// </summary>
        /// <param name="connectionCount"></param>
        /// <returns></returns>
        private bool ShouldChangeBlockByLongestConnecting ( int connectionCount )
        {
            return connectionCount >= _blockConfigureData.CONNECTION_COUNT_CONDITION_FOR_BOMB ||
                   connectionCount >= _blockConfigureData.CONNECTION_COUNT_CONDITION_FOR_SUPER_BOMB;
        }

        /// <summary>
        ///     선택된 블럭의 조건(길게 드래그)으로 인해 블럭이 변경될때 어떤 블럭으로 변경되는지 반환 합니다.
        /// </summary>
        /// <param name="connectionCount"></param>
        /// <returns></returns>
        private BlockType GetBlockTypeWhenLongestConnecting ( int connectionCount )
        {
            if ( connectionCount >= _blockConfigureData.CONNECTION_COUNT_CONDITION_FOR_SUPER_BOMB )
            {
                return BlockType.ItemSuperBomb;
            }

            if ( connectionCount >= _blockConfigureData.CONNECTION_COUNT_CONDITION_FOR_BOMB )
            {
                return BlockType.ItemBomb;
            }

            return BlockType.ItemBomb;
        }

        #region private

        [Inject]
        private BlockConfigureData _blockConfigureData;
        [Inject]
        private IBoardStateContoller _boardStateContoller;
        [Inject]
        private IComboController _comboController;

        #endregion
    }
}