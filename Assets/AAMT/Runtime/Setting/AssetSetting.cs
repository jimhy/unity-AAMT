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
            LocalBundle,
            Remote
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
        [SerializeField] private string remotePath = "http://localhot:88";
        [SerializeField] private LoadType loadType;
        [SerializeField] private bool moveAssetToPersistentDataPath = false;
        [SerializeField] private string[] streamingAssetsPaths;

        private string _realBuildPath;
        private string _realLoadPath;
        private static AssetSetting _assetSetting;

        internal void Init()
        {
            _realLoadPath = AAMTRuntimeProperties.EvaluateString(loadPath);
#if UNITY_EDITOR
            _realBuildPath = AAMTRuntimeProperties.EvaluateString(buildPath);
            if (loadType == LoadType.LocalAssets)
                _realLoadPath = _realLoadPath.Replace(Application.dataPath, "Assets");
#else
            loadType = (loadType == LoadType.LocalAssets) ? LoadType.Remote : loadType;
#endif
            if (loadType == LoadType.Remote)
            {
                _realLoadPath = Application.persistentDataPath;
            }

            Debug.LogFormat("Current load path:{0}", _realLoadPath);
        }

        public string GetBuildPath => $"{_realBuildPath}/{buildTarget}";
        public string GetBuildTargetToString => buildTarget.ToString();
        public string[] GetStreamingAssetsPaths => streamingAssetsPaths;
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