using System.Collections.Generic;
using UnityEngine;

namespace SJLGPP.Block
{
    public class BlockConnectUseCase
    {
        #region properties
        private DIObjectPool<BlockConnect> _pool;
        private List<BlockConnect>         _conectionList;
        #endregion
        
        public BlockConnectUseCase(DIObjectPool<BlockConnect> pool)
        {
            _pool          = pool;
            _conectionList = new List<BlockConnect>();
        }

        /// <summary>
        /// 초기화 합니다.
        /// </summary>
        public void Initialize()
        {
            _pool.Initialize(20);
        }
        
        /// <summary>
        /// 블럭 연결 오브젝트를 생성합니다.
        /// </summary>
        /// <param name="startLocalPosition"></param>
        /// <param name="endLocalPosition"></param>
        public void SetConnection(Vector3 startLocalPosition, Vector3 endLocalPosition)
        {
            BlockConnect blockConnect = GetConnectObject();
            
            Vector3 dir = ( endLocalPosition - startLocalPosition ).normalized;
            blockConnect.CachedTransform.localPosition = (endLocalPosition + startLocalPosition) * 0.5f;
            blockConnect.CachedTransform.localRotation = Quaternion.FromToRotation( Vector3.up, dir );
        }

        /// <summary>
        /// 생성된 연결 오브젝트를 삭제합니다.
        /// </summary>
        public void RemoveAll()
        {
            foreach (var obj in _conectionList)
            {
                _pool.Release(obj);
            }
            _conectionList.Clear();
        }

        /// <summary>
        /// 컨넥션 오브젝트를 풀에서 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private BlockConnect GetConnectObject()
        {
            BlockConnect blockConnect = _pool.Get();

            _conectionList.Add(blockConnect);
            return blockConnect;
        }
        
    }
}