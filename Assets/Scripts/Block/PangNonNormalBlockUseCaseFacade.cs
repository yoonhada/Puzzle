using System.Collections.Generic;
using Scripts.Block;
using UnityEngine;

namespace SJLGPP.Block
{
    public class PangNonNormalBlockUseCaseFacade
    {
        #region private

        private readonly Dictionary<BlockType, IPangNonNormalBlockUseCase> _useCases;

        #endregion

        public PangNonNormalBlockUseCaseFacade ( IPangBombBlockUseCase pangBombBlockUseCase,
            IPangSuperBombBlockUseCase pangSuperBombBlockUseCase )
        {
            _useCases = new Dictionary<BlockType, IPangNonNormalBlockUseCase>();
            _useCases.Add( pangBombBlockUseCase.BlockType, pangBombBlockUseCase );
            _useCases.Add( pangSuperBombBlockUseCase.BlockType, pangSuperBombBlockUseCase );
        }

        /// <summary>
        ///     특수 블럭을 그 블럭의 로직으로 pang 처리 합니다.
        ///     특수블럭이 없거나 그 블럭의 pang 로직이 없으면 종료합니다.
        /// </summary>
        /// <param name="dictionary"></param>
        public void Excute ( Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary )
        {
            var block = FindNonNormalBlock( dictionary );

            if ( block == null )
            {
                return;
            }

            if ( !_useCases.ContainsKey( block.BlockType ) )
            {
                return;
            }

            _useCases[block.BlockType].Pang( block );
        }

        /// <summary>
        ///     특수블럭을 찾아 반환합니다.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private IBlockContract.IBlockPresenter FindNonNormalBlock (
            Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary )
        {
            foreach ( var keyValue in dictionary )
            {
                if ( keyValue.Value.BlockType > BlockType.NormalPurple &&
                     keyValue.Value.BlockState == EBlockState.WAITING_PANG )
                {
                    return keyValue.Value;
                }
            }

            return null;
        }
    }
}