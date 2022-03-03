using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAMT
{
    [CreateAssetMenu(fileName = "BuildSetting", menuName = "AAMT/BuildSetting", order = 1)]
    public class AssetSetting : ScriptableObject
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

        private static AssetSetting _assetSetting;
        [SerializeField] private LoadType loadType;

        internal void Init()
        {
            _realLoadPath = AAMTRuntimeProperties.EvaluateString(loadPath);
            _realBuildPath = AAMTRuntimeProperties.EvaluateString(buildPath);
#if !UNITY_EDITOR
            loadType = LoadType.Bundle;
#endif
            if (loadType == LoadType.LocalAssets)
            {
                _realLoadPath = _realLoadPath.Replace(Application.dataPath, "Assets");
            }
        }

        public string GetBuildPath => $"{_realBuildPath}/{buildTarget}";
        public string GetBuildTargetToString => buildTarget.ToString();
        public BuildTarget GetBuildTarget => buildTarget;

        public string GetLoadPath
        {
            get
            {
                if (loadType == LoadType.LocalAssets) return _realLoadPath;
                
                return $"{_realLoadPath}/{buildTarget}";
            }
        }

        public LoadType GetLoadType => loadType;
    }
}