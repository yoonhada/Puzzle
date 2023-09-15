using System.Collections.Generic;
using Scripts.Block;
using UnityEngine;

namespace SJLGPP.Block
{
    public class PangIndestructibleBlockUseCaseFacade
    {
        public PangIndestructibleBlockUseCaseFacade ( IPangCherryBlockUseCase pangCherryBlockUseCase )
        {
            _useCases = new Dictionary<BlockType, IPangIndestructibleBlockUseCase>();
            _useCases.Add( pangCherryBlockUseCase.BlockType, pangCherryBlockUseCase );

            _deleteIndexList = new List<Vector2Int>();
        }

        /// <summary>
        ///     pang이 불가능한 블럭이 바닥에 있으면 또는 아래에 비어있거나 pang 불가능한 블럭만 있을경우 pang 합니다.
        /// </summary>
        /// <param name="dictionary"></param>
        public void Excute ( Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary )
        {
            IBlockContract.IBlockPresenter block;
            foreach ( var keyValue in dictionary )
            {
                block = keyValue.Value;
                if ( IsIndestructibleBlockOnFloor( block, dictionary ) )
                {
                    if ( _useCases.ContainsKey( block.BlockType ) )
                    {
                        _useCases[block.BlockType].OnPang( block );

                        block.SetBlockState( EBlockState.PANG );
                        _deleteIndexList.Add( keyValue.Key );
                    }
                }
            }

            //pang된 블럭은 dictionary에서 제거합니다.
            //빈공간을 채우기 위해 dictionary에서 판단하기 위함입니다.
            foreach ( var index in _deleteIndexList )
            {
                dictionary.Remove( index );
            }

            _deleteIndexList.Clear();
        }

        /// <summary>
        ///     pang이 불가능한 블럭이 바닥에 있는지 또는 아래에 비어있거나 pang 불가능한 블럭만 있는지 확인 합니다.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsIndestructibleBlockOnFloor ( IBlockContract.IBlockPresenter block,
            Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary )
        {
            if ( block == null )
            {
                return false;
            }

            var indexBelowBlock = block.Index;
            for ( var i = block.Index.y; i >= 0; i-- )
            {
                indexBelowBlock.y -= 1;

                if ( dictionary.ContainsKey( indexBelowBlock ) )
                {
                    if ( dictionary[indexBelowBlock].IsPangable() )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #region private

        private readonly Dictionary<BlockType, IPangIndestructibleBlockUseCase> _useCases;
        private readonly List<Vector2Int> _deleteIndexList;

        #endregion
    }
}