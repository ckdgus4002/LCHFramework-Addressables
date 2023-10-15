using LCHFramework.Addressable.Components;
using LCHFramework.Managers;
using UnityEngine;

namespace LCHFramework.Addressable.Managers
{
    [RequireComponent(typeof(AssetAddressableLoader<Object, object>))]
    public class AssetAddressableLoadSequence : Sequence
    {
        private AssetAddressableLoader<Object, object> AssetAddressableLoader => _assetAddressableLoader == null ? _assetAddressableLoader = GetComponent<AssetAddressableLoader<Object, object>>() : _assetAddressableLoader;
        private AssetAddressableLoader<Object, object> _assetAddressableLoader;



        protected override void _Show()
        {
            AssetAddressableLoader.LoadAsync();
            
            Hide();
        }
    }
}
