using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SJLGPP.Block
{
    public class BlockInstaller : MonoInstaller
    {
        public AssetReference blockAtlasAssetReference;
        public GameObject blockPrefab;
        public GameObject blockConnectPrefab;
        public Transform blockContainer;

        public override void InstallBindings ()
        {
            BindBlockObject();
            BindBlockProcess();
            BindObjectPool();
        }

        /// <summary>
        ///     블럭 오브젝트 관련 바인드 처리 합니다.
        /// </summary>
        private void BindBlockObject ()
        {
            Container.Bind<IBlockModel>().To<BlockModel>().AsTransient();
            Container.Bind<IBlockUseCase>().To<BlockUseCase>().AsTransient();
            Container.Bind<IBlockContract.IBlockView>().To<BlockView>().FromComponentSibling();
        }

        /// <summary>
        ///     블럭 프로세스 관련 바인드 처리 합니다.
        /// </summary>
        private void BindBlockProcess ()
        {
            Container.Bind( typeof( IDisposable ), typeof( IBlockAssetRepository ) )
                .To<BlockAssetRepository>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<BlockProcessor>().AsSingle();
            Container.Bind<IBlockDataQueueRepository>().To<BlockDataQueueRepository>().AsTransient();
            Container.Bind<IBlockDataQueueUseCase>().To<BlockDataQueueUseCase>().AsTransient();
            Container.Bind<IBlockPositionUseCase>().To<BlockPositionUseCase>().AsTransient();
            Container.Bind<IBlockContainer>().To<BlockContainer>().AsSingle();
            Container.Bind<IBlockTypeRepository>().To<BlockTypeRepository>().AsSingle();
            Container.Bind<IBoardStateContoller>().To<BoardStateContoller>().AsSingle();
            Container.Bind<IBlockAssetUseCase>().To<BlockAssetUseCase>().AsSingle();

            Container.Bind<SelectBlockUseCase>().To<SelectBlockUseCase>().AsSingle();
            Container.Bind<ReleaseSelectedBlockUseCase>().To<ReleaseSelectedBlockUseCase>().AsSingle();
            Container.Bind<SetDeltaPangSelectedBlockUseCase>().To<SetDeltaPangSelectedBlockUseCase>().AsSingle();
            Container.Bind<PangWaitingBlockUseCase>().To<PangWaitingBlockUseCase>().AsSingle();
            Container.Bind<PangSelectedBlockAroundUseCase>().To<PangSelectedBlockAroundUseCase>().AsSingle();
            Container.Bind<DropBlockUseCase>().To<DropBlockUseCase>().AsSingle();
            Container.Bind<BlockConnectUseCase>().To<BlockConnectUseCase>().AsSingle();
            Container.Bind<PangNonNormalBlockUseCaseFacade>().To<PangNonNormalBlockUseCaseFacade>().AsSingle();
            Container.Bind<IPangBombBlockUseCase>().To<PangBombBlockUseCase>().AsSingle();
            Container.Bind<IPangSuperBombBlockUseCase>().To<PangSuperBombBlockUseCase>().AsSingle();
            Container.Bind<PangIndestructibleBlockUseCaseFacade>()
                .To<PangIndestructibleBlockUseCaseFacade>()
                .AsSingle();
            Container.Bind<IPangCherryBlockUseCase>().To<PangCherryBlockUseCase>().AsSingle();
            Container.Bind<AssetReference>()
                .FromInstance( blockAtlasAssetReference )
                .WhenInjectedInto<BlockAssetRepository>();
        }

        /// <summary>
        ///     오브젝트 풀 관련 바인드 처리 합니다.
        /// </summary>
        private void BindObjectPool ()
        {
            Container.Bind<DIObjectPool<BlockPresenter>>().AsTransient();
            Container.BindFactory<BlockPresenter, PlaceholderFactory<BlockPresenter>>()
                .FromComponentInNewPrefab( blockPrefab )
                .UnderTransform( blockContainer );

            Container.Bind<DIObjectPool<BlockConnect>>().AsTransient();
            Container.BindFactory<BlockConnect, PlaceholderFactory<BlockConnect>>()
                .FromComponentInNewPrefab( blockConnectPrefab )
                .UnderTransform( blockContainer );
        }
    }
}