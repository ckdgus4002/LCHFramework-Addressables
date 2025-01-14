using System;
using LCHFramework.Components;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif

namespace LCHFramework.Addressables.Components
{
    public class AddressableAssetLoader<T1, T2> : LCHMonoBehaviour where T1 : Object
    {
        [SerializeField] private bool autoLoad;
        [SerializeField] private bool autoRelease = true;
#if UNITY_EDITOR
        public AssetReferenceT<T1> asset;
#endif
        [SerializeField] protected string assetAddress;
        
        
        protected AsyncOperationHandle<T2> AsyncOperationHandle { get; private set; }


        private bool IsLoaded => AsyncOperationHandle.IsValid();
        
        
        
        private void OnValidate()
        {
#if UNITY_EDITOR
            assetAddress = asset != null ? AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(asset.AssetGUID).address : string.Empty;
#endif
        }
        
        protected override void Start()
        {
            base.Start();
            
            if (autoLoad) LoadAsync();
        }

        private void OnDestroy()
        {
            if (autoRelease) ReleaseIfCan();
        }
        
        
        
        // https://docs.unity3d.com/Packages/com.unity.addressables@1.20/manual/LoadingAssetBundles.html
        public AsyncOperationHandle<T2> LoadAsync()
        {
            if (!IsLoaded)
            {
                AsyncOperationHandle = GetLoadAsyncOperationHandle();
                AsyncOperationHandle.Completed += handle =>
                {
                    var dlError = GetDownloadError(AsyncOperationHandle);
                    if (!string.IsNullOrEmpty(dlError))
                    {
                        // handle what error
                        Debug.LogError(dlError);
                    }
                };
            }
            
            return AsyncOperationHandle;
        }

        protected virtual AsyncOperationHandle<T2> GetLoadAsyncOperationHandle()
        {
            return UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T2>(assetAddress);
        }

        private string GetDownloadError(AsyncOperationHandle fromHandle)
        {
            if (fromHandle.Status != AsyncOperationStatus.Failed)
                return null;

            RemoteProviderException remoteException;
            var e = fromHandle.OperationException;
            while (e != null)
            {
                remoteException = e as RemoteProviderException;
                if (remoteException != null)
                    return remoteException.WebRequestResult.Error;
                e = e.InnerException;
            }

            return null;
        }

        public void ReleaseIfCan() => ReleaseIfCan(result => { if (result) Release(); });
        
        public void ReleaseIfCan(Action<bool> release)
        {
            var result = IsLoaded;
            
            release?.Invoke(result);
        }

        protected virtual void Release() => UnityEngine.AddressableAssets.Addressables.Release(AsyncOperationHandle);
    }
}
