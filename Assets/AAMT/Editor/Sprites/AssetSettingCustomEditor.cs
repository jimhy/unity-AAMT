using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    [CustomEditor(typeof(AssetSetting))]
    public class AssetSettingCustomEditor : UnityEditor.Editor
    {
        private SerializedProperty _nameLabel;
        private string _lastNameLabel;
        private SerializedProperty _buildTarget;
        private SerializedProperty _buildPath;
        private SerializedProperty _loadType;
        private SerializedProperty _remotePath;
        private SerializedProperty _moveToStreamingAssetsPathResList;
        private Regex _httpRegex;
        private AssetSetting.LoadType _lastLoadType;
        private GUIContent moveSpecifiedAbContent;
        private GUIContent moveAllAbContent;

        private const string DialogText1 =
            "远程模式需要把一些指定的资源拷贝到StreamingAssets文件夹下，" +
            "如果之前没有拷贝过或者Bundle文件有更新，则需要重新拷贝资源。" +
            "是否需要帮您拷贝文件到StreamingAssets文件夹下?";

        private const string DialogText2 =
            "本地模式需要把所有的Bundle资源拷贝到StreamingAssets文件夹下，" +
            "如果之前没有拷贝过或者Bundle文件有更新，则需要重新拷贝资源。" +
            "是否需要帮您拷贝文件到StreamingAssets文件夹下?";

        private void OnEnable()
        {
            _nameLabel = serializedObject.FindProperty("name");
            if (!string.IsNullOrEmpty(serializedObject.targetObject.name))
            {
                _nameLabel.stringValue = serializedObject.targetObject.name;
                serializedObject.ApplyModifiedProperties();
            }
           
            _lastNameLabel = _nameLabel.stringValue;
            _buildTarget = serializedObject.FindProperty("buildTarget");
            _buildPath = serializedObject.FindProperty("buildPath");
            _loadType = serializedObject.FindProperty("loadType");
            _remotePath = serializedObject.FindProperty("remotePath");
            _moveToStreamingAssetsPathResList = serializedObject.FindProperty("moveToStreamingAssetsPathResList");
            _httpRegex = new Regex(@"http(s)?://.+");
            _lastLoadType = (AssetSetting.LoadType) _loadType.enumValueIndex;
            moveSpecifiedAbContent = new GUIContent();
            moveSpecifiedAbContent.text = "Move AB To SA";
            moveSpecifiedAbContent.tooltip = "Move the specified bundles to the StreamingAssets folder.";
            moveAllAbContent = new GUIContent();
            moveAllAbContent.text = "Move ALL AB To SA";
            moveAllAbContent.tooltip = "Move all the bundles to the StreamingAssets folder.";
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_nameLabel);
            EditorGUILayout.PropertyField(_buildTarget);
            var target = (AssetSetting.BuildTarget) _buildTarget.enumValueIndex;
            var loadType = (AssetSetting.LoadType) _loadType.enumValueIndex;
            var buildPath = AAMTRuntimeProperties.EvaluateString(_buildPath.stringValue);

            if (target != AssetSetting.BuildTarget.editor)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_loadType);
                var buildPathIsSA = buildPath.IndexOf(Application.streamingAssetsPath, StringComparison.Ordinal) != -1;
                if (loadType == AssetSetting.LoadType.Remote)
                {
                    if (GUILayout.Button(moveSpecifiedAbContent))
                    {
                        MoveAb(target);
                    }
                }
                else if (loadType == AssetSetting.LoadType.Local && !buildPathIsSA)
                {
                    if (GUILayout.Button(moveAllAbContent))
                    {
                        MoveAllAb(target);
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(_buildPath);
                if (loadType == AssetSetting.LoadType.Remote)
                {
                    if (!_httpRegex.IsMatch(_remotePath.stringValue))
                    {
                        EditorGUILayout.HelpBox("远程加载必须要填写远程服务器路径地址,需要带http(s)://", MessageType.Error);
                    }
                }

                if (loadType != _lastLoadType)
                {
                    if (loadType == AssetSetting.LoadType.Local && !buildPathIsSA)
                    {
                        var b = EditorUtility.DisplayDialog("温馨提示", DialogText2, "需要", "不需要");
                        if (b) MoveAllAb(target);
                    }
                    else if (loadType == AssetSetting.LoadType.Remote)
                    {
                        var b = EditorUtility.DisplayDialog("温馨提示", DialogText1, "需要", "不需要");
                        if (b) MoveAb(target);
                    }
                }

                _lastLoadType = loadType;

                EditorGUILayout.PropertyField(_remotePath);
                if (loadType == AssetSetting.LoadType.Remote)
                    EditorGUILayout.PropertyField(_moveToStreamingAssetsPathResList);
            }
            
            if (_lastNameLabel != _nameLabel.stringValue)
            {
                var path = AssetDatabase.GetAssetPath(serializedObject.targetObject);
                var asset = AssetDatabase.RenameAsset(path, _nameLabel.stringValue);
                serializedObject.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                _lastNameLabel = _nameLabel.stringValue;
                Debug.Log(path);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void MoveAb(AssetSetting.BuildTarget target)
        {
            SettingManager.ReloadAssetSetting(target);
            OnPreprocessHandler.MoveBundleToStreamingAssets();
            OnPreprocessHandler.CreateBundleFilesDictionary();
            OnPreprocessHandler.CreateStreamingAssetsVersionData();
            AssetDatabase.Refresh();
        }

        private void MoveAllAb(AssetSetting.BuildTarget target)
        {
            SettingManager.ReloadAssetSetting(target);
            OnPreprocessHandler.MoveAllBundleToStreamingAssets();
            AssetDatabase.Refresh();
        }
    }
}