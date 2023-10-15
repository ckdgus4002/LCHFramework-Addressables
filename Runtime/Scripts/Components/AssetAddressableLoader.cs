using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace LCHFramework.Addressable.Components
{
    public class AssetAddressableLoader<T1, T2> : MonoBehaviour where T1 : Object
    {
#if UNITY_EDITOR
        [PostProcessScene(-1)]
        public static void OnBuild()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (var rootGameObjects in SceneManager.GetSceneAt(i).GetRootGameObjects())
                foreach (var assetAddressableLoader in rootGameObjects.GetComponentsInChildren<AssetAddressableLoader<T1, T2>>())
                    if (assetAddressableLoader.asset != null)
                        assetAddressableLoader.AssetAddress = GetAddress(assetAddressableLoader.asset);
        }
        
        private static string GetAddress(AssetReference sceneAsset) => AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(sceneAsset.AssetGUID).address;
#endif
        
        
        
        [SerializeField] private bool autoLoad;
        [SerializeField] private bool autoRelease = true;
        
#if UNITY_EDITOR
        public AssetReferenceT<T1> asset;
#endif
        
        
        
        
        protected string AssetAddress { get; private set; }
        protected AsyncOperationHandle<T2> AsyncOperationHandle { get; private set; }
        
        
        private bool IsLoaded => AsyncOperationHandle is { IsDone: true, Status: AsyncOperationStatus.Succeeded };
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!EditorApplication.isCompiling) AssetAddress = asset != null ? GetAddress(asset) : string.Empty;
        }
#endif

        protected virtual void Start()
        {
            if (autoLoad) LoadAsync();
        }

        protected virtual void OnDestroy()
        {
            if (autoRelease) Release();
        }
        
        
        
        public AsyncOperationHandle<T2> LoadAsync()
        {
            if (!IsLoaded)
            {
                AsyncOperationHandle = _LoadAsync();
            }
            
            return AsyncOperationHandle;
        }

        protected virtual AsyncOperationHandle<T2> _LoadAsync() => Addressables.LoadAssetAsync<T2>(AssetAddress);

        public void Release()
        {
            if (IsLoaded) _Release();
        }

        protected virtual void _Release() => Addressables.Release(AsyncOperationHandle);
    }
}
