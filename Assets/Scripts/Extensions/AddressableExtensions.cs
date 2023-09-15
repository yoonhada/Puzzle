using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UniRx;
using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using Object = UnityEngine.Object;

namespace Extensions
{
    public static class AddressableExtensions
    {
        /// <summary>
        /// AssetReference를 통해 객체를 비동기적으로 로드합니다.
        /// </summary>
        /// <typeparam name="TObject">로드할 객체 타입입니다.</typeparam>
        /// <param name="assetReference">로드할 AssetReference 객체입니다.</param>
        /// <param name="monoBehaviour">
        /// 핸들이 자동으로 릴리즈되는 MonoBehaviour 객체입니다. 
        /// 빈 값인 경우, 로드 즉시 릴리즈됩니다. 
        /// this를 전달한 경우, MonoBehaviour OnDestroy() 메서드가 호출될 때 자동으로 릴리즈됩니다.
        /// </param>
        /// <returns>로드된 객체를 발행하는 Observable</returns>
        public static IObservable<TObject> LoadAssetAsObservable<TObject>(this AssetReference assetReference, MonoBehaviour monoBehaviour = null) where TObject : Object
        {
            // Observable.Defer를 사용하여 Observable을 반환합니다.
            return Observable.Defer(() =>
            {
                // AssetReference를 사용하여 비동기적으로 객체를 로드합니다.
                var asyncOperationHandle = assetReference.LoadAssetAsync<TObject>();

                // MonoBehaviour가 설정된 경우
                if (monoBehaviour != null)
                {
                    // MonoBehaviour가 OnDestroy 이벤트를 발생시키면, 핸들을 자동으로 릴리즈합니다.
                    var subject = new Subject<Unit>();
                    monoBehaviour.OnDestroyAsObservable().Subscribe(_ => subject.OnNext(Unit.Default));
                    asyncOperationHandle.ToUniTask(cancellationToken: default)
                                        .ContinueWith(_ => subject.OnCompleted());
                }

                // UniTask를 Observable로 변환합니다.
                return asyncOperationHandle.ToUniTask(cancellationToken: default)
                                           .ContinueWith(_ => asyncOperationHandle.Status == AsyncOperationStatus.Succeeded
                                               ? asyncOperationHandle.Result
                                               : throw new Exception(
                                                   $"Failed to load asset with error {asyncOperationHandle.OperationException}"))
                                           .ToObservable();
            });
        }
    }
}