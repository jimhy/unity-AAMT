using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    [CustomEditor(typeof(UnityEditor.DefaultAsset))]
    public class AssetBundleSetting : Editor
    {
        private bool                needPackage;
        private WindowDefine.ABType _abType;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = true;
            needPackage = GUILayout.Toggle(needPackage, "打包");
            if (needPackage)
            {
                _abType = (WindowDefine.ABType) EditorGUILayout.EnumPopup("打包类型", _abType);
            }
        }
    }

    public partial class AutoSetAssetsBundelName : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
                                                  string[] movedFromAssetPaths)
        {
            foreach (var str in importedAssets)
            {
                Debug.Log($"importedAssets:{str}");
            }

            foreach (var str in movedAssets)
            {
                Debug.Log($"movedAssets:{str}");
            }

            foreach (var str in deletedAssets)
            {
                Debug.Log($"deletedAssets:{str}");
            }

            foreach (var str in movedFromAssetPaths)
            {
                Debug.Log($"movedFromAssetPaths:{str}");
            }
        }
    }
}