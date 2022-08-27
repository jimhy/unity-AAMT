using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAMT
{
    [CreateAssetMenu(fileName = "BuildSetting", menuName = "AAMT/BuildSetting", order = 1)]
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
        
        public string name;

        [SerializeField]
        private BuildTarget buildTarget;

        [SerializeField]
        private string buildPath = "{UnityEngine.Application.dataPath}/../Build/";

        [SerializeField]
        private string remotePath = "http://localhost:80";

        [SerializeField]
        private string[] moveToStreamingAssetsPathResList;

        [SerializeField]
        private LoadType loadType = LoadType.Local;


        private        string       _realBuildPath;

        private static AssetSetting assetSetting;

        internal void Init()
        {
#if UNITY_EDITOR
            _realBuildPath = AAMTRuntimeProperties.EvaluateString(buildPath);
            getLoadPath    = "assets";
            if (buildTarget != BuildTarget.editor)
            {
                if (loadType == LoadType.Local)
                    getLoadPath = $"{_realBuildPath}/{getBuildTarget}";
                else
                    getLoadPath = $"{Application.persistentDataPath}{getBuildTarget}";
            }

            getLoadPath = getLoadPath.ToLower();
#else
            InitBuildTarget();
            if (loadType == LoadType.Local)
                getLoadPath = $"{Application.streamingAssetsPath}/{buildTarget}";
            else
                getLoadPath = $"{Application.persistentDataPath}/{buildTarget}";
#endif
            Debug.LogFormat("LoadType:{0}", getLoadType);
            Debug.LogFormat("BuildTarget:{0}", buildTarget);
            Debug.LogFormat("Current load path:{0}", getLoadPath);
        }

        public string      getBuildPath                        => $"{_realBuildPath}/{buildTarget}";
        public string[]    getMoveToStreamingAssetsPathResList => moveToStreamingAssetsPathResList;
        public string      getRemotePath                       => remotePath;
        public BuildTarget getBuildTarget                      => buildTarget;
        public string      getLoadPath                         { get; private set; }
        public LoadType    getLoadType                         => loadType;

        private void InitBuildTarget()
        {
            buildTarget = Tools.PlatformToBuildTarget();
        }

#if UNITY_EDITOR
        public void SetBuildTargetForBulidPlayer(BuildTarget target)
        {
            buildTarget = target;
        }
#endif
    }
}