using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Addressable.Components
{
    public class SceneAssetAddressableLoader : AssetAddressableLoader<SceneAsset, SceneInstance>
    {
        // UnityEvent event.
        public void OnClick() => LoadAsync();
        
        
        
        protected override AsyncOperationHandle<SceneInstance> _LoadAsync() => Addressables.LoadSceneAsync(AssetAddress);

        protected override void _Release() => Addressables.UnloadSceneAsync(AsyncOperationHandle);
    }
}