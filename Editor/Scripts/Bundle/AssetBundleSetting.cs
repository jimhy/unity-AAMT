using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT.Editor
{
    [CustomEditor(typeof(DefaultAsset))]
    public class AssetBundleSetting : UnityEditor.Editor
    {
        private const string _singleText = "当前文件夹下的每个资源会被打包成单独的ab文件。";
        private const string _packageText = "当前文件夹下的所有资源会被统一打包成一个ab文件。(注意:如果当前目录存在子目录，如果子目录设置成Parent，则会一并打包成一个ab文件.)";
        private const string _parentText = "当前文件夹下的所有资源会打包到父级的ab文件中。";
        private bool _needPackage;
        private WindowDefine.ABType _abType;
        private WindowDefine.ABType _childAbType;
        private Object _selected;
        private AssetBundlePackageData PackageData => EditorCommon.PackageData;

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.IndexOf("Assets/", StringComparison.Ordinal) == -1 || !Directory.Exists(path)) return;

            if (_selected != Selection.activeObject)
            {
                _selected    = Selection.activeObject;
                _needPackage = false;
                _abType      = WindowDefine.ABType.PACKAGE;

                var data = PackageData.GetDataByPath(path);
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
                var str = _abType switch
                {
                    WindowDefine.ABType.SINGLE  => _singleText,
                    WindowDefine.ABType.PACKAGE => _packageText,
                    WindowDefine.ABType.PARENT  => _parentText,
                    _                           => ""
                };

                EditorGUILayout.HelpBox(str, MessageType.Info);
            }

            if (np != _needPackage || tp != _abType)
            {
                if (_needPackage) PackageData.Set(path, _abType);
                else PackageData.RemoveByPath(path);
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
        /// <param name="isRemove"></param>
        private void UsedChildDirectories(string path, WindowDefine.ABType abType, bool isRemove)
        {
            var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            if (isRemove)
            {
                foreach (var directory in directories)
                {
                    PackageData.RemoveByPath(directory);
                }
            }
            else
            {
                foreach (var directory in directories)
                {
                    PackageData.Set(directory, abType);
                }
            }
        }

        [InitializeOnLoadMethod]
        private static void OnDrawFolderIcon()
        {
            EditorApplication.projectWindowItemOnGUI = delegate(string guid, Rect rect)
            {
                if (!EditorCommon.ShowFolderIcon) return;

                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!AssetDatabase.IsValidFolder(path)) return;
                var packageData = EditorCommon.PackageData.GetDataByGuid(guid);
                if (packageData == null) return;

                var isSmall = IsIconSmall(rect);
                rect        =  GetIconRect(rect, isSmall);
                rect.width  *= .6f;
                rect.height *= .6f;
                rect.y      += rect.height * .8f;
                rect.x      += rect.width;
                var img = packageData.AbType switch
                {
                    WindowDefine.ABType.PACKAGE => Icons.PACKAGE.Texture2D,
                    WindowDefine.ABType.PARENT  => Icons.PARENT.Texture2D,
                    WindowDefine.ABType.SINGLE  => Icons.SINGLE.Texture2D,
                    _                           => null
                };

                GUI.DrawTexture(rect, img);
                EditorApplication.RepaintProjectWindow();
            };
        }

        [InitializeOnLoadMethod]
        private static void OnInitEvents()
        {
            EditorCommon.EventBus.removeEventListener(EventType.DELETED_ASSETS, OnDeleteAssets);
            EditorCommon.EventBus.addEventListener(EventType.DELETED_ASSETS, OnDeleteAssets);
        }

        private static void OnDeleteAssets(Event e)
        {
            if (e.data is not string[] paths) return;
            foreach (var path in paths)
            {
                EditorCommon.PackageData.RemoveByPath(path);
            }
        }

        private static Rect GetIconRect(Rect rect, bool isSmall)
        {
            if (isSmall)
                rect.width = rect.height;
            else
                rect.height = rect.width;

            return rect;
        }

        private static bool IsIconSmall(Rect rect)
        {
            return rect.width > rect.height;
        }
    }
}