using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AAMT
{
    /// <summary>
    /// ab加载任务代理
    /// </summary>
    public class BundleLoader : ILoader
    {
        private struct LoadData
        {
            public string[] Path;
            public Action CallBack;
        }
        private readonly BundleManager _bundleManager;
        private bool _hasLoading;
        private readonly Queue<LoadData> _loadDataList;

        public BundleLoader()
        {
            _bundleManager = AssetsManager.Instance.bundleManager;
            _loadDataList = new Queue<LoadData>();
        }

        public void Load(string[] path, Action callBack)
        {
            _loadDataList.Enqueue(new LoadData(){Path = path,CallBack = callBack});
            if(!_hasLoading) Load();
        }

        private void Load()
        {
            if(_loadDataList.Count <= 0) return;
            var data = _loadDataList.Dequeue();
            
        }

        private void StartLoad(string path, Action callBack)
        {
            
            path = path.ToLower();
            if (!_bundleManager.pathToBundle.ContainsKey(path))
            {
                Debug.LogErrorFormat("加载资源时，找不到对应资源的ab包。path={0}", path);
                return;
            }

            if (_bundleManager.HasAssets(path))
            {
                callBack?.Invoke();
            }
            else
            {
                // AssetsManagerRuntime.Instance.Coroutine(LoadAssetBundle(path, callBack));
            }
        }


        private IEnumerator LoadAssetBundle(string resPath, Action callBack)
        {
            var abName = _bundleManager.pathToBundle[resPath];
            //加载依赖项
            var abPath =
                $"{BuildSetting.AssetSetting.GetLoadPath}/{abName}";
            LoadDependencies(abName);
            var abLoader = AssetBundle.LoadFromFileAsync(abPath);
            yield return abLoader;
            if (abLoader.assetBundle == null)
            {
                Debug.LogErrorFormat("Load  ab File Error!path:{0}", abPath);
            }
            else
            {
                _bundleManager.AddBundle(abLoader.assetBundle);
                callBack?.Invoke();
            }
        }

        /// <summary>
        /// 加载依赖
        /// </summary>
        /// <param name="loadPath"></param>
        /// <returns></returns>
        private void LoadDependencies(string abName)
        {
            var abPaths = _bundleManager.assetBundleManifest.GetAllDependencies(abName);
            for (int i = 0; i < abPaths.Length; i++)
            {
                var abp = abPaths[i].ToLower();
                if (_bundleManager.HasAssets(abp))
                {
                    continue;
                }

                var realPath = $"{BuildSetting.AssetSetting.GetLoadPath}/{abp}";
                var bundle = AssetBundle.LoadFromFile(realPath);
                _bundleManager.AddBundle(bundle);
            }
        }
    }
}