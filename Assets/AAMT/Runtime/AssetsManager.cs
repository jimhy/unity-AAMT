using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT
{
    /// <summary>
    /// Automatic Assets Manager Tools
    /// </summary>
    public class AssetsManager
    {
        private static AssetsManager _instance;

        private LoaderManager _loaderManager;
        internal IResourceManager ResourceManager { get; private set; }

        internal static AssetsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AssetsManager();
                    _instance.Init();
                }

                return _instance;
            }
        }

        private AssetsManager()
        {
        }

        private void Init()
        {
            if (SettingManager.AssetSetting.GetLoadType == AssetSetting.LoadType.LocalAssets)
                ResourceManager = new LocalAssetManager();
            else
                ResourceManager = new BundleManager();

            _loaderManager = new LoaderManager();
            var runtimeGameObject = new GameObject
            {
                name = "JAssetsManagerRuntime"
            };
            runtimeGameObject.AddComponent<AssetsManagerRuntime>();
        }

        public static LoaderHandler LoadAssets(string[] assetsPath)
        {
            return Instance._loaderManager.Load(assetsPath);
        }

        public static void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            path = path.ToLower();
            if (Instance.ResourceManager.HasAssetsByPath(path))
            {
                Instance.ResourceManager.GetAssets(path, callBack);
            }
            else
            {
                var handler = Instance._loaderManager.Load(new[] {path});
                handler.customData = new List<object>() {path, callBack};
                handler.onComplete = loaderHandler =>
                {
                    if (loaderHandler.customData is not List<object> list) return;
                    var currentPath = list[0] as string;
                    var cb = list[1] as Action<T>;
                    Instance.ResourceManager.GetAssets(currentPath, cb);
                };
            }
        }

        public static void GetAssets<T>(IEnumerable<string> paths, Action<T> callBack) where T : Object
        {
            foreach (var path in paths)
            {
                GetAssets(path, callBack);
            }
        }

        public static void Release(string path)
        {
            path = path.ToLower();
            Instance.ResourceManager.Release(path);
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
            Instance.ResourceManager.Destroy(path);
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