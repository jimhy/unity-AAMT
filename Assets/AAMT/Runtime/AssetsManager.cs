using System;
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
        internal BundleManager bundleManager { get; private set; }

        internal static AssetsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AssetsManager();
                    _instance.init();
                }

                return _instance;
            }
        }

        private AssetsManager()
        {
        }

        private void init()
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
                handler.customData = callBack;
                handler.onComplete = loaderHandler =>
                {
                    Instance.bundleManager.GetAssets(path, loaderHandler.customData as Action<T>);
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
    }
}