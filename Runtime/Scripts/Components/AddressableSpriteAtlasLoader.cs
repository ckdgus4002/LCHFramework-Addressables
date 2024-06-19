using System;
using UnityEngine.U2D;

namespace LCHFramework.Addressable.Components
{
    public class AddressableSpriteAtlasLoader : AddressableAssetLoader<SpriteAtlas, SpriteAtlas>
    {
        protected override void Start()
        {
            base.Start();
            
            SpriteAtlasManager.atlasRequested += OnAtlasRequested;        
        }

        private void OnDestroy()
        {
            SpriteAtlasManager.atlasRequested -= OnAtlasRequested;
        }
        
        
        
        private void OnAtlasRequested(string tag, Action<SpriteAtlas> action)
        {
            if (tag == AssetAddress)
                LoadAsync().Completed += handle =>
                {
                    action(handle.Result);
                };
        }
    }
}
