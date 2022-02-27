using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAMT
{
    [CreateAssetMenu(fileName = "BuildSetting", menuName = "AAMT/BuildSetting", order = 0)]
    public class BuildSetting : ScriptableObject
    {
        [Serializable]
        public enum LoadType
        {
            LocalAssets,
            Bundle,
        }

        [Serializable]
        public enum BuildTarget
        {
            windows,
            android,
            ios,
        }

        [SerializeField] private BuildTarget buildTarget;

        [SerializeField] private string buildPath = "{UnityEngine.Application.streamingAssetsPath}";

        [SerializeField] private string loadPath = "{UnityEngine.Application.streamingAssetsPath}";
        private string _realBuildPath;
        private string _realLoadPath;

        private static BuildSetting _buildSetting;
        [SerializeField] private LoadType loadType;

        private void Init()
        {
            _realLoadPath = JAssetManagerRuntimeProperties.EvaluateString(loadPath);
            _realBuildPath = JAssetManagerRuntimeProperties.EvaluateString(buildPath);
#if !UNITY_EDITOR
            loadType = LoadType.Bundle;
#endif
        }

        public string GetBuildPath => $"{_realBuildPath}/{buildTarget}";
        public string GetBuildTargetToString => buildTarget.ToString();
        public BuildTarget GetBuildTarget => buildTarget;
        public string GetLoadPath => $"{_realLoadPath}/{buildTarget}";
        public LoadType GetLoadType => loadType;

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
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/AAMT/Data/BuildSetting.asset");
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", "Assets/AAMT/Data/BuildSetting.asset");
            _buildSetting = ScriptableObject.Instantiate(sprite) as BuildSetting;
            if (_buildSetting != null) _buildSetting.Init();
#else
            var buildTag = GetBuildTagByCurrentPlatform();
            if (buildTag == string.Empty)
            {
                Debug.LogErrorFormat("当前平台不支持:{0}", Application.platform.ToString());
                return;
            }

            var path = $"{Application.streamingAssetsPath}/{buildTag}/BuildSetting.asset.ab".ToLower();
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", path);
            var bundle =
                AssetBundle.LoadFromFile(path);
            _buildSetting = bundle.LoadAsset<BuildSetting>("buildsetting.asset");
            _buildSetting.Init();
#endif
        }

        private static string GetBuildTagByCurrentPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return BuildTarget.android.ToString();
                case RuntimePlatform.IPhonePlayer:
                    return BuildTarget.ios.ToString();
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return BuildTarget.windows.ToString();
            }

            return string.Empty;
        }
    }
}