using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif

namespace LCHFramework.Addressable.Components
{
#if UNITY_EDITOR
    public class AddressableSceneAssetLoader : AddressableAssetLoader<SceneAsset, SceneInstance>
#else
    public class AddressableSceneAssetLoader : AddressableAssetLoader<Object, SceneInstance>
#endif
    {
        // UnityEvent event.
        public void OnClick() => LoadAsync();



        protected override AsyncOperationHandle<SceneInstance> GetLoadAsyncOperationHandle() => Addressables.LoadSceneAsync(assetAddress);

        protected override void Release() => Addressables.UnloadSceneAsync(AsyncOperationHandle);
    }
}