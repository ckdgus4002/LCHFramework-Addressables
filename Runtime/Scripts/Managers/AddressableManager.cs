using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.U2D;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace LCHFramework.Addressables.Managers
{
    public static class AddressableManager
    {
        public static AsyncOperationHandle<IResourceLocator> InitializeAsync() => UnityEngine.AddressableAssets.Addressables.InitializeAsync();
        
        
        
        public static async Awaitable<bool> DownloadAsync(string label,
            Action<AsyncOperationHandle<long>> onDownloadSize,
            Func<AsyncOperationHandle<long>, Awaitable<bool>> waitForCanDownload,
            Action<AsyncOperationHandle<long>, AsyncOperationHandle> onDownload,
            float minimumDuration = 1f)
        {
            var downloadIsSuccess = false;
            var downloadSize = UnityEngine.AddressableAssets.Addressables.GetDownloadSizeAsync(label);
            onDownloadSize?.Invoke(downloadSize);
            var downloadSizeStartTime = Time.time;
            while (!downloadSize.IsDone || Time.time - downloadSizeStartTime < minimumDuration) await Awaitable.NextFrameAsync();
            
            if (downloadSize.Status == AsyncOperationStatus.Succeeded)
            {
                if (downloadSize is { Result: > 0 })
                {
                    var canDownload = waitForCanDownload == null || await waitForCanDownload.Invoke(downloadSize);
                    if (!canDownload) return false;
                
                    var download = DownloadAsync(label, false);
                    onDownload?.Invoke(downloadSize, download);
                    var downloadStartTime = Time.time;
                    while (!download.IsDone || Time.time - downloadStartTime < minimumDuration) await Awaitable.NextFrameAsync();
                    
                    downloadIsSuccess = download.Status == AsyncOperationStatus.Succeeded;
                    UnityEngine.AddressableAssets.Addressables.Release(download);
                }
                else
                    downloadIsSuccess = true;
            }
            UnityEngine.AddressableAssets.Addressables.Release(downloadSize);   

            return downloadIsSuccess;
        }
        
        public static AsyncOperationHandle DownloadAsync(string label, bool autoReleaseHandle = true)
        {
            var operationHandle = UnityEngine.AddressableAssets.Addressables.DownloadDependenciesAsync(label, autoReleaseHandle);
            operationHandle.Completed += handle =>
            {
                var dlError = GetDownloadError(handle);
                if (!string.IsNullOrEmpty(dlError))
                {
                    // handle what error
                    Debug.LogError(dlError);
                }
            };
            return operationHandle;
        }
        
        private static string GetDownloadError(AsyncOperationHandle fromHandle)
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
        
        
        
        public static List<string> GetAddresses<T>(string label)
        {
            var result = new List<string>();
            foreach (var locator in UnityEngine.AddressableAssets.Addressables.ResourceLocators)
                if (locator.Locate(label, typeof(T), out var locations))
                    result.AddRange(locations.Select(t => t.PrimaryKey));

            return result;
        }
        
#if UNITY_EDITOR
        /// <remarks>
        /// Used at AddressableAssetSettings.CustomContentStateBuildPath.
        /// </remarks>
        public static string AddressableContentStateBuildPath
        {
            get
            {
                var profileId = AddressableAssetSettingsDefaultObject.Settings.activeProfileId;
                var profileName = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetProfileName(profileId);
                var profileIsDefault = profileName == "Default";
                var defaultContentStateBuildPath = $"{AddressableAssetSettingsDefaultObject.kDefaultConfigFolder}/{PlatformMappingService.GetPlatformPathSubFolder()}";
                var customContentStateBuildPath = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetValueByName(profileId, AddressableAssetSettings.kRemoteBuildPath); 
                return profileIsDefault ? defaultContentStateBuildPath : customContentStateBuildPath;
            }
        }
#endif
    }
}
