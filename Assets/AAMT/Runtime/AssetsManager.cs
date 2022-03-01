using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
        internal BundleManager bundleManager { get; private set; }

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
            bundleManager = new BundleManager();
            _loaderManager = new LoaderManager();
            var runtimeGameObject = new GameObject();
            runtimeGameObject.name = "JAssetsManagerRuntime";
            runtimeGameObject.AddComponent<AssetsManagerRuntime>();
        }

        public static LoaderHandler LoadAssets(string[] assetsPath)
        {
            return Instance._loaderManager.Load(assetsPath);
        }

        public static void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            path = path.ToLower();
            if (Instance.bundleManager.HasBundleByAssetsPath(path))
            {
                Instance.bundleManager.GetAssets(path, callBack);
            }
            else
            {
                var handler = Instance._loaderManager.Load(new[] {path});
                handler.customData = new List<object>() {path, callBack};
                handler.onComplete = loaderHandler =>
                {
                    var list = loaderHandler.customData as List<object>;
                    var currentPath = list[0] as string;
                    var cb = list[1] as Action<T>;
                    Instance.bundleManager.GetAssets(currentPath, cb);
                };
            }
        }

        public static void GetAssets<T>(string[] paths, Action<T> callBack) where T : Object
        {
            foreach (var path in paths)
            {
                GetAssets(path, callBack);
            }
        }

        public static void Release(string path)
        {
            Instance.bundleManager.Release(path);
        }

        public static void Release(string[] path)
        {
            foreach (var s in path)
            {
                Release(s);
            }
        }

        public static void Destroy(string path)
        {
            Instance.bundleManager.Destroy(path);
        }

        public static void Destroy(string[] path)
        {
            foreach (var s in path)
            {
                Destroy(s);
            }
        }
    }
}