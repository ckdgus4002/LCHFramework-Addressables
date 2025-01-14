using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif

namespace LCHFramework.Addressables.Components
{
#if UNITY_EDITOR
    public class AddressableSceneAssetLoader : AddressableAssetLoader<SceneAsset, SceneInstance>
#else
    public class AddressableSceneAssetLoader : AddressableAssetLoader<Object, SceneInstance>
#endif
    {
        // UnityEvent event.
        public void OnClick() => LoadAsync();



        protected override AsyncOperationHandle<SceneInstance> GetLoadAsyncOperationHandle() => UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(assetAddress);

        protected override void Release() => UnityEngine.AddressableAssets.Addressables.UnloadSceneAsync(AsyncOperationHandle);
    }
}