using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    [CustomEditor(typeof(AssetSetting))]
    public class SettingPropertyDrawer : UnityEditor.Editor
    {
        private SerializedProperty _buildTarget;
        private SerializedProperty _buildPath;
        private SerializedProperty _loadType;
        private SerializedProperty _remotePath;
        private SerializedProperty _moveToStreamingAssetsPathResList;
        private Regex _httpRegex;
        private AssetSetting.LoadType _lastLoadType;

        private const string DialogText =
            "远程模式需要把一些指定的资源拷贝到StreamingAssets文件夹下，" +
            "如果之前没有拷贝过或者Bundle文件有更新，则需要重新拷贝资源。" +
            "是否需要帮您拷贝文件到StreamingAssets文件夹下?";

        private void OnEnable()
        {
            _buildTarget = serializedObject.FindProperty("buildTarget");
            _buildPath = serializedObject.FindProperty("buildPath");
            _loadType = serializedObject.FindProperty("loadType");
            _remotePath = serializedObject.FindProperty("remotePath");
            _moveToStreamingAssetsPathResList = serializedObject.FindProperty("moveToStreamingAssetsPathResList");
            _httpRegex = new Regex(@"http(s)?://.+");
            _lastLoadType = (AssetSetting.LoadType) _loadType.enumValueIndex;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_buildTarget);
            var target = (AssetSetting.BuildTarget) _buildTarget.enumValueIndex;
            var loadType = (AssetSetting.LoadType) _loadType.enumValueIndex;
            if (target != AssetSetting.BuildTarget.editor)
            {
                EditorGUILayout.PropertyField(_loadType);
                EditorGUILayout.PropertyField(_buildPath);
                if (loadType == AssetSetting.LoadType.Remote)
                {
                    if (!_httpRegex.IsMatch(_remotePath.stringValue))
                    {
                        EditorGUILayout.HelpBox("远程加载必须要填写远程服务器路径地址,需要带http(s)://", MessageType.Error);
                    }

                    if (loadType != _lastLoadType)
                    {
                        var b = EditorUtility.DisplayDialog("温馨提示", DialogText, "需要", "不需要");
                        if (b)
                        {
                            SettingManager.ReloadAssetSetting(target);
                            OnPreprocessHandler.MoveBundleToStreamingAssets();
                            OnPreprocessHandler.CreateBundleFilesDictionary();
                            OnPreprocessHandler.CreateStreamingAssetsVersionData();
                            AssetDatabase.Refresh();
                        }
                    }
                }

                _lastLoadType = loadType;

                EditorGUILayout.PropertyField(_remotePath);
                EditorGUILayout.PropertyField(_moveToStreamingAssetsPathResList);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}