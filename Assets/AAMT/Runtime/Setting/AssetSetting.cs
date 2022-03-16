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

        [SerializeField] private BuildTarget buildTarget;
        [SerializeField] private string buildPath = "{UnityEngine.Application.dataPath}/../Build/";
        [SerializeField] private string remotePath = "http://localhost:80";
        [SerializeField] private string[] moveToStreamingAssetsPathResList;
        [SerializeField] private LoadType loadType = LoadType.Local;


        private string _realBuildPath;
        private static AssetSetting assetSetting;

        internal void Init()
        {
#if UNITY_EDITOR
            _realBuildPath = AAMTRuntimeProperties.EvaluateString(buildPath);
            getLoadPath = "Assets";
            if (buildTarget != BuildTarget.editor)
            {
                if (loadType == LoadType.Local)
                    getLoadPath = Path.Combine(_realBuildPath, getBuildTargetToString);
                else
                    getLoadPath = Path.Combine(Application.persistentDataPath, getBuildTargetToString);
            }
#else
            InitBuildTarget();
            if (loadType == LoadType.Local)
                getLoadPath = Path.Combine(_realBuildPath, getBuildTargetToString);
            else
                getLoadPath = Path.Combine(Application.persistentDataPath, getBuildTargetToString);
#endif
            Debug.LogFormat("Current load path:{0}", getLoadPath);
        }

        public string getBuildPath => Path.Combine(_realBuildPath, getBuildTargetToString);
        public string getBuildTargetToString => buildTarget.ToString().ToLower();
        public string[] getMoveToStreamingAssetsPathResList => moveToStreamingAssetsPathResList;
        public string getRemotePath => remotePath;
        public BuildTarget getBuildTarget => buildTarget;
        public string getLoadPath { get; private set; }

        private void InitBuildTarget()
        {
            buildTarget = PlatformToBuildTarget();
        }

        internal static BuildTarget PlatformToBuildTarget()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return BuildTarget.android;
                case RuntimePlatform.IPhonePlayer:
                    return BuildTarget.ios;
                case RuntimePlatform.WindowsPlayer:
                    return BuildTarget.windows;
            }

            return BuildTarget.editor;
        }

#if UNITY_EDITOR
        public string getEditorBuildPath
        {
            get { return $"{_realBuildPath}/{GetBuildTarget()}"; }
        }

        internal static string GetBuildTarget()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case UnityEditor.BuildTarget.Android:
                    return BuildTarget.android.ToString();
                case UnityEditor.BuildTarget.iOS:
                    return BuildTarget.ios.ToString();
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return BuildTarget.windows.ToString();
            }

            return "";
        }
#endif
    }
}