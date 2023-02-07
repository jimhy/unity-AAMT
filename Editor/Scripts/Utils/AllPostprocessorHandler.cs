using System.Collections.Generic;
using UnityEditor;

namespace AAMT.Editor
{
    public class AllPostprocessorHandler : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (EditorCommon.EventBus == null) return;

            if (importedAssets.Length > 0) EditorCommon.EventBus.dispatchEventWith(EventType.IMPORTED_ASSETS, importedAssets);

            if (deletedAssets.Length > 0) EditorCommon.EventBus.dispatchEventWith(EventType.DELETED_ASSETS, deletedAssets);

            if (movedAssets.Length > 0) EditorCommon.EventBus.dispatchEventWith(EventType.MOVED_ASSETS, new List<string[]>() {movedAssets, movedFromAssetPaths});
        }
    }
}