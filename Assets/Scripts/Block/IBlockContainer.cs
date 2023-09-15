using System.Collections.Generic;
using UnityEngine;

namespace SJLGPP.Block
{
    public interface IBlockContainer
    {
        void MakeBlockToBoard();
        IBlockContract.IBlockPresenter AddBlock(int x, int y);
        IBlockContract.IBlockPresenter FindBlock(GameObject obj);
        IBlockContract.IBlockPresenter FindBlock(Vector2Int index);
        bool HasWaitingPangBlock();
        void GetBlockDictionay(Dictionary<Vector2Int, IBlockContract.IBlockPresenter> dictionary);
        void UpdateDeltaPangBlock(float deltaTime);
    }
}