using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.U2D;
using UnityEngine.U2D;

namespace LCHFramework.Editor
{
    public static class SpriteAtlasPostprocessor_GetIncludeInBuild
    {
        /// <remarks> Invoke by Reflection. </remarks>
        private static bool GetIncludeInBuild(SpriteAtlasImporter spriteAtlasImporter, SpriteAtlas spriteAtlas) => AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(spriteAtlasImporter.assetPath)) != null;
    }
}