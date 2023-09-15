using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SJLGPP
{
    public class DIObjectPool<T> where T : MonoBehaviour
    {
        private PlaceholderFactory<T> factory;
        private Stack<T>              instances;

        /// <summary>
        /// 생성자
        /// </summary>
        public DIObjectPool(PlaceholderFactory<T> factory)
        {
            this.factory = factory;
            instances    = new Stack<T>();
        }
        
        /// <summary>
        /// 초기 생성 갯수만큼 오브젝트 생성
        /// </summary>
        public void Initialize(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = CreateInstance();
                obj.gameObject.SetActive(false);
                instances.Push(obj);
            }
        }

        /// <summary>
        /// 오브젝트 생성
        /// </summary>
        /// <returns></returns>
        private T CreateInstance()
        {
            T obj = factory.Create();
            return obj;
        }

        /// <summary>
        /// 풀에서 오브젝트 가져오기
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T obj = instances.Count > 0 ? instances.Pop() : CreateInstance();
            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// 풀에 오브젝트 반환하기
        /// </summary>
        /// <param name="obj"></param>
        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
            
            if (!instances.Contains(obj))
            {
                instances.Push(obj);
            }
        }
    }
}