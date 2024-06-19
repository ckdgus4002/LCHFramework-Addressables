using LCHFramework.Addressable.Components;
using LCHFramework.Managers;
using UnityEngine;

namespace LCHFramework.Addressable.Managers
{
    [RequireComponent(typeof(AddressableSceneAssetLoader))]
    public class AddressableSceneAssetLoadStep : Step
    {
        private AddressableSceneAssetLoader SceneAssetLoader => _sceneAssetLoader == null ? _sceneAssetLoader = GetComponent<AddressableSceneAssetLoader>() : _sceneAssetLoader;
        private AddressableSceneAssetLoader _sceneAssetLoader;
        
        
        
        public override async void Show()
        {
            base.Show();

            await SceneAssetLoader.LoadAsync().Task;
            
            // PassCurrentStep.PassCurrentStep();
        }
    }
}