using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.Block;
using UnityEngine;
using Zenject;

namespace SJLGPP.Block
{
    public class PangSuperBombBlockUseCase : IPangSuperBombBlockUseCase
    {
        public PangSuperBombBlockUseCase ( BlockConfigureData blockConfigureData )
        {
            _blockConfigureData = blockConfigureData;
            _blockDictionary = new Dictionary<Vector2Int, IBlockContract.IBlockPresenter>();
            _listArray = new List<IBlockContract.IBlockPresenter>[_blockConfigureData.SUPER_BOMB_SCOPE];
            _taskList = new List<UniTask>();

            for ( var i = 0; i < _blockConfigureData.SUPER_BOMB_SCOPE; i++ )
            {
                _listArray[i] = new List<IBlockContract.IBlockPresenter>();
            }
        }

        public BlockType BlockType
        {
            get { return BlockType.ItemSuperBomb; }
        }

        /// <summary>
        ///     폭탄이 터지는 처리 입니다.
        /// </summary>
        /// <param name="block"></param>
        public void Pang ( IBlockContract.IBlockPresenter block )
        {
            //보드에 연출 상태중이라고 설정합니다.
            _boardStateContoller.SetBombEffectState();

            //폭탄 범위의 블럭을 거리 별로 수집합니다. 링 형태로 수집됩니다.
            for ( var order = 0; order < _blockConfigureData.SUPER_BOMB_SCOPE; order++ )
            {
                SetRingNeighborBlocks( block, _listArray[order], order + 1 );
            }

            //수집된 블럭을 터뜨리는 연출을 합니다.
            PangBlocks( block ).Forget();
        }

        /// <summary>
        ///     폭탄 블럭 기준 특정 거리만큼에 위치한 블럭부터 링의 블럭들의 리스트에 저장합니다.
        /// </summary>
        /// <param name="pivotBlock"></param>
        /// <param name="list"></param>
        /// <param name="distance"></param>
        private void SetRingNeighborBlocks ( IBlockContract.IBlockPresenter pivotBlock,
            List<IBlockContract.IBlockPresenter> list, int distance )
        {
            //서치를 빠르게 하기위해 블럭상태를 dictionary에 저장합니다.
            _blockContainer.GetBlockDictionay( _blockDictionary );

            //리스트를 초기화 합니다.
            list.Clear();

            //링 형태로 순차적으로 수집하기 위해 수집한 최근의 블럭 인덱스를 기록합니다.
            var latestIndex = new Vector2Int();

            //주변방향으로 링을 그리며 위치한 블럭을 저장합니다.
            //위쪽을 기준으로 해서 시계방향으로 돕니다.
            for ( var direction = _blockConfigureData.BLOCK_DIRECTION_INDEX_UP;
                 direction <= _blockConfigureData.BLOCK_DIRECTION_INDEX_RIGHT_UP;
                 direction++ )
            {
                //링형태로 돌면서 직선방향으로 전진하고 꺽어야 하는 위치에 가지전에 종료합니다.
                for ( var straightCount = 0; straightCount < distance; straightCount++ )
                {
                    if ( straightCount == 0 )
                    {
                        //처음에는 한 방향으로 거리만큼 떨어진 블럭부터 수집합니다.
                        latestIndex = GetJumpBlockIndex( pivotBlock.Index, direction, distance );
                        AddNormalBlockToPangList( latestIndex, list );
                    }
                    else
                    {
                        //이후 다음 방향의 블럭을 만나기 전까지 그 방향으로 직선으로 이동하며 수집합니다.
                        latestIndex = GetNeighborBlockIndex( latestIndex, ( direction + 2 ) % 6 );
                        AddNormalBlockToPangList( latestIndex, list );
                    }
                }
            }
        }

        /// <summary>
        ///     pivot 블럭의 특정방향으로 특정거리만큼 위치한 블럭을 가져옵니다.
        /// </summary>
        /// <param name="pivotBlock"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private Vector2Int GetJumpBlockIndex ( Vector2Int pivotBlockIndex, int direction, int distance )
        {
            //블럭위치마다 방향인덱스가 다르기 때문에 거리만큼의 횟수로 직접 더해줍니다.
            var directionIndex = pivotBlockIndex;
            for ( var i = 0; i < distance; i++ )
            {
                directionIndex = _blockConfigureData.GetNextDirectionIndex( directionIndex, direction );
            }

            return directionIndex;
        }

        /// <summary>
        ///     pivot 블럭의 특정방향 블럭을 가져옵니다.
        /// </summary>
        /// <param name="pivotBlock"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Vector2Int GetNeighborBlockIndex ( Vector2Int pivotBlockIndex, int direction )
        {
            return _blockConfigureData.GetNextDirectionIndex( pivotBlockIndex, direction );
        }

        /// <summary>
        ///     특정 위치의 블럭이 노말 블럭이라면 수집 리스트에 추가합니다.
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="list"></param>
        private void AddNormalBlockToPangList ( Vector2Int blockIndex, List<IBlockContract.IBlockPresenter> list )
        {
            if ( _blockDictionary.ContainsKey( blockIndex ) )
            {
                var findedBlock = _blockDictionary[blockIndex];
                if ( findedBlock.IsNormalBlock )
                {
                    list.Add( findedBlock );
                }
            }
        }

        /// <summary>
        ///     수집된 블럭을 순차적으로 터뜨립니다.
        /// </summary>
        /// <param name="pivotBlock"></param>
        private async UniTaskVoid PangBlocks ( IBlockContract.IBlockPresenter pivotBlock )
        {
            _taskList.Clear();
            for ( var order = 0; order < _blockConfigureData.SUPER_BOMB_SCOPE; order++ )
            {
                _taskList.Add( PlayPangDirection( pivotBlock, _listArray[order], 30 * order ) );
            }

            await UniTask.WhenAll( _taskList );

            _boardStateContoller.CompleteBombEffectState();
        }

        /// <summary>
        ///     폭탄 주위로 모였다 펼치는 연출을 하고 제자리에 오면 터지도록 합니다.
        /// </summary>
        /// <param name="pivotBlock"></param>
        /// <param name="list"></param>
        /// <param name="delay"></param>
        private async UniTask PlayPangDirection ( IBlockContract.IBlockPresenter pivotBlock,
            List<IBlockContract.IBlockPresenter> list, int delay )
        {
            //링 마다 딜레이를 줍니다.
            await UniTask.Delay( delay );

            var center = pivotBlock.CachedTransform.localPosition;
            float timer = 0;

            while ( true )
            {
                //모일때 스피드와 펼쳐질때 스피드를 다르게 합니다.
                timer += timer < Mathf.PI * 0.5f ? Time.unscaledDeltaTime * 7f : Time.unscaledDeltaTime * 4f;

                if ( timer > Mathf.PI )
                {
                    timer = Mathf.PI;
                }

                var delta = Mathf.Sin( timer );

                foreach ( var block in list )
                {
                    block.CachedTransform.localPosition =
                        Vector3.Lerp( _blockPositionUseCase.ConvertPosition( block.Index ), center, delta * 0.45f );

                    //제 위치로 돌아오면 터지게 합니다.
                    if ( timer >= Mathf.PI )
                    {
                        block.SetBlockState( EBlockState.DELTA_PANG );
                    }
                }

                // 연출 시간이 되면 종료합니다.
                if ( timer >= Mathf.PI )
                {
                    break;
                }

                await UniTask.NextFrame();
            }
        }

        #region private

        [Inject]
        private IBoardStateContoller _boardStateContoller;
        [Inject]
        private IBlockContainer _blockContainer;
        [Inject]
        private IBlockPositionUseCase _blockPositionUseCase;
        private readonly BlockConfigureData _blockConfigureData;
        private readonly List<IBlockContract.IBlockPresenter>[] _listArray;
        private readonly List<UniTask> _taskList;
        private readonly Dictionary<Vector2Int, IBlockContract.IBlockPresenter> _blockDictionary;

        #endregion
    }
}