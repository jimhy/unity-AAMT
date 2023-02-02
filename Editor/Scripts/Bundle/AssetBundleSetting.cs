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
        private static AssetBundlePackageData _packageData;

        protected override void OnHeaderGUI()
        {
            if (_packageData == null)
            {
                _packageData = new AssetBundlePackageData();
            }

            base.OnHeaderGUI();
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.IndexOf("Assets/", StringComparison.Ordinal) == -1 || !Directory.Exists(path)) return;

            if (_selected != Selection.activeObject)
            {
                _selected    = Selection.activeObject;
                _needPackage = false;
                _abType      = WindowDefine.ABType.PACKAGE;

                var data = _packageData.GetData(path);
                if (data != null)
                {
                    _needPackage = true;
                    _abType      = (WindowDefine.ABType)data.AbType;
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
        /// <param name="isRemove"></param>
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

        [InitializeOnLoadMethod]
        static void OnDrawFolderIcon()
        {
            EditorApplication.projectWindowItemOnGUI = delegate(string guid, Rect rect)
            {
                if (!EditorCommon.ShowFolderIcon) return;
                if (_packageData == null) _packageData = new AssetBundlePackageData();

                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!AssetDatabase.IsValidFolder(path)) return;
                var packageData = _packageData.getData(guid);
                if (packageData == null) return;

                var isSmall = IsIconSmall(rect);
                rect        =  GetIconRect(rect, isSmall);
                rect.width  *= .6f;
                rect.height *= .6f;
                rect.y      += rect.height * .8f;
                rect.x      += rect.width;
                Texture2D img = null;
                if (packageData.AbType      == WindowDefine.ABType.PACKAGE) img = Icons.PACKAGE.Texture2D;
                else if (packageData.AbType == WindowDefine.ABType.PARENT) img  = Icons.PARENT.Texture2D;
                else if (packageData.AbType == WindowDefine.ABType.SINGLE) img  = Icons.SINGLE.Texture2D;

                GUI.DrawTexture(rect, img);
                EditorApplication.RepaintProjectWindow();
            };
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