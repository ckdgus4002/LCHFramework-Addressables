using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LCHFramework.Addressable.Components
{
    public class AssetAddressableLoader<T1, T2> : EditorObjectAllocator where T1 : Object
    {
        [SerializeField] private bool autoLoad;
        [SerializeField] private bool autoRelease = true;
        
#if UNITY_EDITOR
        public AssetReferenceT<T1> asset;
#endif

        
        protected string AssetAddress { get; private set; }
        protected AsyncOperationHandle<T2> AsyncOperationHandle { get; private set; }
        
        
        private bool IsLoaded => AsyncOperationHandle is { IsDone: true, Status: AsyncOperationStatus.Succeeded };
        
        
        
        protected virtual void Start()
        {
            if (autoLoad) LoadAsync();
        }

        protected virtual void OnDestroy()
        {
            if (autoRelease) Release();
        }
        
        
        
        public override void OnAllocate() => AssetAddress = asset != null ? AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(asset.AssetGUID).address : string.Empty;
        
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
