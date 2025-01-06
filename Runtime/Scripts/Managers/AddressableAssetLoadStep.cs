using LCHFramework.Addressables.Components;
using LCHFramework.Managers;
using UnityEngine;

namespace LCHFramework.Addressables.Managers
{
    [RequireComponent(typeof(AddressableAssetLoader<Object, object>))]
    public class AddressableAssetLoadStep : Step
    {
        private AddressableAssetLoader<Object, object> AddressableAssetLoader => _addressableAssetLoader == null ? _addressableAssetLoader = GetComponent<AddressableAssetLoader<Object, object>>() : _addressableAssetLoader;
        private AddressableAssetLoader<Object, object> _addressableAssetLoader;
        
        
        
        public override void Show()
        {
            base.Show();

            AddressableAssetLoader.LoadAsync().Completed += _ =>
            {
                PassCurrentStep.PassCurrentStep();
            }; 
        }
    }
}
