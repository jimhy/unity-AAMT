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

        public void LoadAssets(string[] assetsPath, Action callBack)
        {
            _loaderManager.Load(assetsPath, callBack);
        }

        public void LoadAssetsBatch(string[] assetsPath, Action callBack)
        {
            _loaderManager.LoadBatch(assetsPath, callBack);
        }

        public T GetAssets<T>(string path) where T : Object
        {
            return bundleManager.GetAssets<T>(path);
        }

        public T GetAssets<T>(string abName, string itemName) where T : Object
        {
            return bundleManager.GetAssets<T>(abName, itemName);
        }
    }
}