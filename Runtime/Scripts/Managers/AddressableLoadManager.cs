using System.Collections.Generic;
using LCHFramework.Managers;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace LCHFramework.Addressables.Managers
{
    public class AddressableLoadManager<T> where T : UnityEngine.Object
    {
        private static readonly Dictionary<string, AsyncOperationHandle<T>> LoadedAssets = new();
        public static AsyncOperationHandle<T> LoadAssetAsync(string address)
        {
            if (!LoadedAssets.ContainsKey(address))
            {
                var operationHandle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(address);
                operationHandle.Completed += handle =>
                {
                    if (handle.Result is SpriteAtlas spriteAtlas) SpriteAtlasBindingManager.AddSpriteAtlas(spriteAtlas);
                };
                LoadedAssets.Add(address, operationHandle);
            }

            return LoadedAssets[address];
        }
        
        
        
        public static void ReleaseSpriteAtlas(string address)
        {
            if (LoadedAssets.Remove(address, out var value))
            {
                if (value.Result is SpriteAtlas spriteAtlas) SpriteAtlasBindingManager.RemoveSpriteAtlas(spriteAtlas);
                
                UnityEngine.AddressableAssets.Addressables.Release(value);
            }
        }
    }
}