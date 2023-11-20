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
    public class SceneAssetAddressableLoader : AssetAddressableLoader<SceneAsset, SceneInstance>
#else
    public class SceneAssetAddressableLoader : AssetAddressableLoader<Object, SceneInstance>
#endif
    {
        // UnityEvent event.
        public void OnClick() => LoadAsync();
        
        
        
        protected override AsyncOperationHandle<SceneInstance> GetLoadAsyncOperationHandle() => Addressables.LoadSceneAsync(AssetAddress);

        protected override void _Release() => Addressables.UnloadSceneAsync(AsyncOperationHandle);
    }
}