using System;
using UnityEngine.U2D;

namespace LCHFramework.Addressable.Components
{
    public class AtlasAddressableLoader : AssetAddressableLoader<SpriteAtlas, SpriteAtlas>
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
        
        
        
        private async void OnAtlasRequested(string tag, Action<SpriteAtlas> action)
        {
            if (tag == AssetAddress) action(await LoadAsync().Task);
        }
    }
}
