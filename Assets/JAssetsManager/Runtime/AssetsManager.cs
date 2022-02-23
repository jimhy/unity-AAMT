using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JAssetsManager
{
    public class AssetsManager
    {
        public static AssetsManager Instance { get; } = new AssetsManager();
        private Dictionary<string, AssetBundle> _bundles;
        private LoaderManager _loaderManager;

        private AssetsManager()
        {
            _loaderManager = new LoaderManager();
            _bundles = new Dictionary<string, AssetBundle>();
            var runtimeGameObject = new GameObject();
            runtimeGameObject.name = "JAssetsManagerRuntime";
            runtimeGameObject.AddComponent<JAssetsManagerRuntime>();
        }

        internal bool HasAssets(string path)
        {
            return false;
        }

        internal void AddBundle(string path, AssetBundle ab)
        {
            if (!_bundles.ContainsKey(path)) _bundles.Add(path, ab);
            else Debug.LogErrorFormat("重复添加ab包，应该是出现了重复加载，会出现双份内存的情况，请检查。path:{0}", path);
        }

        public T GetAssets<T>(string path) where T : Object
        {
            path = path.ToLower();
            if (!_bundles.ContainsKey(path)) return default;
            var name = path;
            var n = path.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                name = path.Substring(n + 1);
            }

            return _bundles[path].LoadAsset<T>(name);
        }
        public T GetAssets<T>(string abName,string itemName) where T : Object
        {
            abName = abName.ToLower();
            itemName = itemName.ToLower();
            if (!_bundles.ContainsKey(abName)) return default;

            return _bundles[abName].LoadAsset<T>(itemName);
        }

        public void LoadAssets(string assetsPath, Action callBack)
        {
            _loaderManager.Load(assetsPath, callBack);
        }
    }
}