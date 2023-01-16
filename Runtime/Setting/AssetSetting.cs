using System;
using UnityEngine;

namespace AAMT
{
    public class AssetSetting : ScriptableObject
    {
        [Serializable]
        public enum BuildTarget
        {
            editor,
            windows,
            android,
            ios,
        }

        [Serializable]
        public enum LoadType
        {
            Local,

            Remote
        }

        
        public string fileName;

        [SerializeField]
        private BuildTarget buildPlatform;

        [SerializeField]
        private string buildPath = "{UnityEngine.Application.dataPath}/../Build/";

        [SerializeField]
        private LoadType loadType = LoadType.Local;

        [SerializeField]
        private string remotePath = "http://localhost:80";
        
        [SerializeField]
        private string[] moveToStreamingAssetsPathList;

        private string _realBuildPath;

        private static AssetSetting assetSetting;

        internal void Init()
        {
#if UNITY_EDITOR
            _realBuildPath = AAMTRuntimeProperties.EvaluateString(buildPath);
            getLoadPath    = "assets";
            if (buildPlatform != BuildTarget.editor)
            {
                if (loadType == LoadType.Local)
                    getLoadPath = $"{_realBuildPath}/{GetBuildPlatform}";
                else
                    getLoadPath = $"{Application.persistentDataPath}/{GetBuildPlatform}";
            }

            getLoadPath = getLoadPath.ToLower();
#else
            InitBuildTarget();
            if (loadType == LoadType.Local)
                getLoadPath = $"{Application.streamingAssetsPath}/{buildPlatform}";
            else
                getLoadPath = $"{Application.persistentDataPath}/{buildPlatform}";
#endif
            Debug.LogFormat("LoadType:{0}", getLoadType);
            Debug.LogFormat("BuildTarget:{0}", buildPlatform);
            Debug.LogFormat("Current load path:{0}", getLoadPath);
        }

        public string      getBuildPath                     => $"{_realBuildPath}/{buildPlatform}";
        public string[]    GetMoveToStreamingAssetsPathList => moveToStreamingAssetsPathList;
        public string      getRemotePath                    => remotePath;
        public BuildTarget GetBuildPlatform                 => buildPlatform;
        public string      getLoadPath                      { get; private set; }
        public LoadType    getLoadType                      => loadType;

        private void InitBuildTarget()
        {
            buildPlatform = Tools.PlatformToBuildTarget();
        }

#if UNITY_EDITOR
        public void SetBuildTargetForBulidPlayer(BuildTarget target)
        {
            buildPlatform = target;
        }

        public string WindowGetSourceBuildPath => buildPath;
#endif
    }
}