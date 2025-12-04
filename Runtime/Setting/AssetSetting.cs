using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAMT
{
    public class AssetSetting : ScriptableObject
    {
        [HideInInspector]
        public string Guid;

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

        [SerializeField]
        private BuildTarget buildPlatform;

        [SerializeField]
        private string buildPathSetting = "{UnityEngine.Application.dataPath}/../Build/";

        [SerializeField]
        private LoadType loadType = LoadType.Local;

        [SerializeField]
        private string remotePath = "http://localhost:80";

        [SerializeField]
        private string[] moveToStreamingAssetsPathList;

        private string _realBuildPath;
        [FormerlySerializedAs("_macro")]
        
        [SerializeField]
        private string macro;

        internal void Init()
        {
            if (remotePath[^1] == '/') remotePath = remotePath.Substring(0, remotePath.Length - 1);
#if UNITY_EDITOR
            InitForEditor();
#else
            InitForRuntime();
#endif
        }

        private void InitForEditor()
        {
            _realBuildPath = AAMTRuntimeProperties.EvaluateString(buildPathSetting);
            LoadPath       = "assets";
            if (buildPlatform != BuildTarget.editor)
            {
                if (loadType == LoadType.Local)
                    LoadPath = $"{_realBuildPath}/{BuildPlatform}";
                else
                    LoadPath = $"{Application.persistentDataPath}/{BuildPlatform}";
            }

            LoadPath = LoadPath.ToLower();

            // Debug.LogFormat("LoadType:{0}", getLoadType);
            // Debug.LogFormat("BuildTarget:{0}", buildPlatform);
            // Debug.LogFormat("Current load path:{0}", getLoadPath);
        }

        private void InitForRuntime()
        {
            InitBuildTarget();
            if (loadType == LoadType.Local)
                LoadPath = $"{Application.streamingAssetsPath}/{buildPlatform}";
            else
                LoadPath = $"{Application.persistentDataPath}/{buildPlatform}";
        }

        public string BuildPath => $"{_realBuildPath}/{buildPlatform}";

        public string[] MoveToStreamingAssetsPathList
        {
            get => moveToStreamingAssetsPathList;
            set => moveToStreamingAssetsPathList = value;
        }

        public string RemotePath
        {
            get => remotePath;
            set => remotePath = value;
        }
        public string Macro
        {
            get => macro;
            set => macro = value;
        }

        public BuildTarget BuildPlatform
        {
            get => buildPlatform;
            set => buildPlatform = value;
        }

        public string LoadPath { get; private set; }

        public LoadType CurrentLoadType
        {
            get => loadType;
            set => loadType = value;
        }

        public string BuildPathSetting
        {
            get => buildPathSetting;
            set => buildPathSetting = value;
        }

        public void Save()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(Guid)) return;
            var path     = AssetDatabase.GUIDToAssetPath(Guid);
            var instance = CreateInstance<AssetSetting>();
            instance.name             = name;
            instance.buildPlatform    = buildPlatform;
            instance.buildPathSetting = buildPathSetting;
            instance.loadType         = loadType;
            instance.remotePath       = remotePath;
            instance.Guid             = Guid;
            instance.macro            = macro;

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        private void InitBuildTarget()
        {
            buildPlatform = Tools.PlatformToBuildTarget();
        }

#if UNITY_EDITOR
        public void SetBuildTargetForBulidPlayer(BuildTarget target)
        {
            buildPlatform = target;
        }
#endif
    }
}