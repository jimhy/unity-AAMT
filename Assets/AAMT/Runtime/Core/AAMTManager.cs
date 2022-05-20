using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AAMT
{
    /// <summary>
    /// Automatic Assets Manager Tools
    /// </summary>
    public class AAMTManager
    {
        private static AAMTManager _instance;

        internal static AAMTManager Instance
        {
            get
            {
                if (_instance == null) _instance = new AAMTManager();
                return _instance;
            }
        }

        private LoaderManager _loaderManager;
        internal IResourceManager resourceManager { get; private set; }

        private AAMTManager()
        {
            Init();
        }

        private void Init()
        {
            AddRuntime();
            if (SettingManager.assetSetting.getBuildTarget == AssetSetting.BuildTarget.editor)
                resourceManager = new LocalAssetManager();
            else
                resourceManager = new BundleManager();
            _loaderManager = new LoaderManager();
        }

        private static void AddRuntime()
        {
            if (GameObject.Find("AAMTRuntime") == null)
            {
                var runtimeGameObject = new GameObject
                {
                    name = "AAMTRuntime"
                };
                runtimeGameObject.AddComponent<AAMTRuntime>();
                Object.DontDestroyOnLoad(runtimeGameObject);
            }
        }

        public static AsyncHandler MoveBundles()
        {
            AddRuntime();
            if (SettingManager.assetSetting.getLoadType == AssetSetting.LoadType.Local)
            {
                Debug.LogErrorFormat("加载类型为Local,不能移动Bundles.");
                return null;
            }

            var moveBundleManager = new MoveBundleManager();
            moveBundleManager.MoveAssets();
            return moveBundleManager.handler;
        }

        public static AsyncHandler UpdateAssets()
        {
            AddRuntime();
            if (SettingManager.assetSetting.getLoadType == AssetSetting.LoadType.Local)
            {
                Debug.LogErrorFormat("加载类型为Local,不能更新资源.");
                return null;
            }

            var downloadManager = new BundleDowndloadManager();
            downloadManager.Start();
            return downloadManager.handler;
        }

        public static AsyncHandler LoadAssetsAsync(string[] assetsPath)
        {
            return Instance._loaderManager.LoadAsync(assetsPath);
        }

        public static AsyncHandler LoadAssetsAsync(string assetsPath)
        {
            return Instance._loaderManager.LoadAsync(new[] {assetsPath});
        }

        public static void GetAssetsAsync<T>(string path, Action<T> callBack) where T : Object
        {
            path = path.ToLower();
            if (Instance.resourceManager.HasAssetsByPath(path))
            {
                Instance.resourceManager.GetAssetsAsync(path, callBack);
            }
            else
            {
                var handler = Instance._loaderManager.LoadAsync(new[] {path});
                handler.customData = new List<object>() {path, callBack};
                handler.onComplete = loaderHandler =>
                {
                    if (loaderHandler.customData is not List<object> list) return;
                    var currentPath = list[0] as string;
                    var cb = list[1] as Action<T>;
                    Instance.resourceManager.GetAssetsAsync(currentPath, cb);
                };
            }
        }

        public static void GetAllAssetsAsync(string path, Action<Object[]> callBack)
        {
            path = path.ToLower();
            if (Instance.resourceManager.HasAssetsByPath(path))
            {
                Instance.resourceManager.GetAllAssetsAsync(path, callBack);
            }
            else
            {
                var handler = Instance._loaderManager.LoadAsync(new[] {path});
                handler.customData = new List<object>() {path, callBack};
                handler.onComplete = loaderHandler =>
                {
                    if (loaderHandler.customData is not List<object> list) return;
                    var currentPath = list[0] as string;
                    var cb = list[1] as Action<Object[]>;
                    Instance.resourceManager.GetAllAssetsAsync(currentPath, cb);
                };
            }
        }

        public static void GetAssetsAsync<T>(IEnumerable<string> paths, Action<T> callBack) where T : Object
        {
            foreach (var path in paths)
            {
                GetAssetsAsync(path, callBack);
            }
        }

        public static void LoadScene(string path, [CanBeNull] Action callBack)
        {
            LoadScene(path, LoadSceneMode.Additive, callBack);
        }

        public static void LoadScene(string path, LoadSceneMode mode, [CanBeNull] Action callBack)
        {
            var h = Instance._loaderManager.LoadAsync(new[] {path.ToLower()});
            h.onComplete = _ => { Instance.resourceManager.ChangeScene(path, callBack); };
        }


        public static void Release(string path)
        {
            path = path.ToLower();
            Instance.resourceManager.Release(path);
        }

        public static void Release(IEnumerable<string> path)
        {
            foreach (var s in path)
            {
                Release(s);
            }
        }

        public static void Destroy(string path)
        {
            path = path.ToLower();
            Instance.resourceManager.Destroy(path);
        }

        public static void Destroy(IEnumerable<string> path)
        {
            foreach (var s in path)
            {
                Destroy(s);
            }
        }
    }
}