using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace JAssetsManager
{
    [CreateAssetMenu(fileName = "BuildSetting", menuName = "JAssetsManager/BuildSetting", order = 0)]
    public class BuildSetting : ScriptableObject
    {
        [Serializable]
        public enum LoadType
        {
            LocalAssets,
            Bundle,
        }

        public enum BuildTag
        {
            windows,
            android,
            iOS,
        }

        [SerializeField] private BuildTag _buildTag;

        [SerializeField] private string _buildPath;

        [SerializeField] private string _loadPath;

        private static BuildSetting _buildSetting;
        [SerializeField] private LoadType _loadType;

        public void Init()
        {
            _loadPath = JAssetManagerRuntimeProperties.EvaluateString(_loadPath);
            _buildPath = JAssetManagerRuntimeProperties.EvaluateString(_buildPath);
#if !UNITY_EDITOR
            _loadType = LoadType.Bundle;
#endif
        }

        public string GetBuildPath => $"{_buildPath}/{_buildTag}";
        public string GetBuildTag => _buildTag.ToString();
        public string GetLoadPath => $"{_loadPath}/{_buildTag}";
        public LoadType GetLoadType => _loadType;

        public static BuildSetting AssetSetting
        {
            get
            {
                if (_buildSetting == null) LoadAssetSetting();
                return _buildSetting;
            }
        }

        private static void LoadAssetSetting()
        {
#if UNITY_EDITOR
            var sprite =
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/JAssetsManager/Data/BuildSetting.asset");
            _buildSetting = ScriptableObject.Instantiate(sprite) as BuildSetting;
            _buildSetting.Init();
#else
            var buildTag = getBuildTagByCurrentPlatform();
            if (buildTag == string.Empty)
            {
                Debug.LogErrorFormat("当前平台不支持:{0}", Application.platform.ToString());
                return;
            }

            var path = $"{Application.streamingAssetsPath}/{buildTag}/BuildSetting.asset.ab".ToLower();
            var bundle =
                AssetBundle.LoadFromFile(path);
            _buildSetting = bundle.LoadAsset<BuildSetting>("buildsetting.asset");
            _buildSetting.Init();
#endif
        }

        private static string getBuildTagByCurrentPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return BuildTag.android.ToString();
                case RuntimePlatform.IPhonePlayer:
                    return BuildTag.iOS.ToString();
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return BuildTag.windows.ToString();
            }

            return string.Empty;
        }
    }
}