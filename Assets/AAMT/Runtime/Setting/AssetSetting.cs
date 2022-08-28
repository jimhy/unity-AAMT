using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAMT
{
    public partial class AssetSetting : ScriptableObject
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
            [LabelText("本地加载")]
            Local,

            [LabelText("远程加载")]
            Remote
        }

        [LabelText("名字")]
        public string name;

        [FormerlySerializedAs("buildTarget")]
        [LabelText("目标平台")]
        [SerializeField]
        private BuildTarget buildPlatform;

        [LabelText("打包路径")]
        [SerializeField]
        [HideIf("buildPlatform", BuildTarget.editor)]
        private string buildPath = "{UnityEngine.Application.dataPath}/../Build/";

        [LabelText("远程资源地址")]
        [SerializeField]
        [HideIf("buildPlatform", BuildTarget.editor)]
        private string remotePath = "http://localhost:80";

        [LabelText("资源加载类型")]
        [SerializeField]
        [HideIf("buildPlatform", BuildTarget.editor)]
        private LoadType loadType = LoadType.Local;

        [LabelText("AB包文件夹")]
        [InfoBox("在打包时,把需要打进apk包的资源拷贝到StreamingAssets目录")]
        [SerializeField]
        [HideIf("@this.buildPlatform==BuildTarget.editor || this.loadType==LoadType.Local")]
        [FolderPath]
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
                    getLoadPath = $"{Application.persistentDataPath}{GetBuildPlatform}";
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