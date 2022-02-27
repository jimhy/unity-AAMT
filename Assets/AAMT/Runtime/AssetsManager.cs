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
        internal BundleManager bundleManager { get; private set; }

        public static AssetsManager Instance
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

        public void LoadAssets(string[] assetsPath, Action<object> callBack)
        {
            _loaderManager.Load(assetsPath, callBack);
        }

        public void LoadAssets(string[] assetsPath, Action<object> callBack, object data)
        {
            _loaderManager.Load(assetsPath, callBack, data);
        }

        public void LoadAssetsBatch(string[] assetsPath, Action<object> callBack)
        {
            _loaderManager.LoadBatch(assetsPath, callBack, null);
        }

        public void LoadAssetsBatch(string[] assetsPath, Action<object> callBack, object data)
        {
            _loaderManager.LoadBatch(assetsPath, callBack, data);
        }

        public void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            if (bundleManager.HasBundleByAssetsPath(path))
            {
                bundleManager.GetAssets(path, callBack);
            }
            else
            {
                _loaderManager.Load(new[] {path}, (cb) =>
                {
                    var mcb = cb as Action<T>;
                    bundleManager.GetAssets(path, mcb);
                }, callBack);
            }
        }
    }
}