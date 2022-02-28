using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public static void LoadAssets(string[] assetsPath, Action<object> callBack)
        {
            Instance._loaderManager.Load(assetsPath, callBack, null);
        }

        public static void LoadAssets(string[] assetsPath, Action<object> callBack, object data)
        {
            Instance._loaderManager.Load(assetsPath, callBack, data);
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
                Instance._loaderManager.Load(new[] {path}, (cb) =>
                {
                    var mcb = cb as Action<T>;
                    Instance.bundleManager.GetAssets(path, mcb);
                }, callBack);
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