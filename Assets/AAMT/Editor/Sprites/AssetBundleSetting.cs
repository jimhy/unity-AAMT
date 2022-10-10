using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    [CustomEditor(typeof(UnityEditor.DefaultAsset))]
    public class AssetBundleSetting : Editor
    {
        private const string _singleText = "当前文件夹下的每个资源会被打包成单独的ab文件。";
        private const string _packageText = "当前文件夹下的所有资源会被统一打包成一个ab文件。(注意:如果当前目录存在子目录，如果子目录设置成Parent，则会一并打包成一个ab文件.)";
        private const string _parentText = "当前文件夹下的所有资源会打包到父级的ab文件中。";
        private bool _needPackage;
        private WindowDefine.ABType _abType;
        private WindowDefine.ABType _childAbType;
        private Object _selected;
        private static AssetBundlePackageData _packageData;

        protected override void OnHeaderGUI()
        {
            if (_packageData == null)
            {
                var sprite = AssetDatabase.LoadAssetAtPath<Object>(AAMTDefine.AAMT_BUNDLE_PACKAGE_DATA);
                _packageData = ScriptableObject.CreateInstance<AssetBundlePackageData>();
                AssetDatabase.CreateAsset(_packageData, AAMTDefine.AAMT_BUNDLE_PACKAGE_DATA);
                if (sprite != null)
                {
                    var source = ScriptableObject.Instantiate(sprite) as AssetBundlePackageData;
                    _packageData.CloneData(source);
                }
            }

            base.OnHeaderGUI();
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (_selected != Selection.activeObject)
            {
                _selected    = Selection.activeObject;
                _needPackage = false;
                _abType      = WindowDefine.ABType.PACKAGE;

                var data = _packageData.GetData(path);
                if (data != null)
                {
                    _needPackage = true;
                    _abType      = data.AbType;
                }

                _childAbType = WindowDefine.ABType.PARENT;
            }

            GUI.enabled = true;
            var np = _needPackage;
            var tp = _abType;
            _needPackage = GUILayout.Toggle(_needPackage, "AssetsBundle");
            if (_needPackage)
            {
                _abType = (WindowDefine.ABType)EditorGUILayout.EnumPopup("打包类型", _abType);
                var str = "";
                switch (_abType)
                {
                    case WindowDefine.ABType.SINGLE:
                        str = _singleText;
                        break;
                    case WindowDefine.ABType.PACKAGE:
                        str = _packageText;
                        break;
                    case WindowDefine.ABType.PARENT:
                        str = _parentText;
                        break;
                }

                EditorGUILayout.HelpBox(str, MessageType.Info);
            }

            if (np != _needPackage || tp != _abType)
            {
                if (_needPackage) _packageData.Set(path, _abType);
                else _packageData.Remove(path);
            }

            GUILayout.BeginHorizontal();
            _childAbType = (WindowDefine.ABType)EditorGUILayout.EnumPopup("子文件夹类型", _childAbType);
            if (GUILayout.Button("应用"))
            {
                UsedChildDirectories(path, _childAbType, false);
            }

            if (GUILayout.Button("移除"))
            {
                UsedChildDirectories(path, _childAbType, true);
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 应用于子文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="abType"></param>
        private void UsedChildDirectories(string path, WindowDefine.ABType abType, bool isRemove)
        {
            var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            if (isRemove)
            {
                foreach (var directory in directories)
                {
                    _packageData.Remove(directory);
                }
            }
            else
            {
                foreach (var directory in directories)
                {
                    _packageData.Set(directory, abType);
                }
            }
        }
    }

    // public partial class AutoSetAssetsBundelName : AssetPostprocessor
    // {
    //     public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    //     {
    //         foreach (var str in importedAssets)
    //         {
    //             Debug.Log($"importedAssets:{str}");
    //         }
    //
    //         foreach (var str in movedAssets)
    //         {
    //             Debug.Log($"movedAssets:{str}");
    //         }
    //
    //         foreach (var str in deletedAssets)
    //         {
    //             Debug.Log($"deletedAssets:{str}");
    //         }
    //
    //         foreach (var str in movedFromAssetPaths)
    //         {
    //             Debug.Log($"movedFromAssetPaths:{str}");
    //         }
    //     }
    // }
}