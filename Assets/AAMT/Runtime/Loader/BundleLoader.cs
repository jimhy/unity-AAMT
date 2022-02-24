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
        private readonly BundleManager _bundleManager;
        private string[] _resPathList;
        private Action _callBack;
        private int _loadIndex;

        public BundleLoader()
        {
            _bundleManager = AssetsManager.Instance.bundleManager;
        }

        public void Load(string[] path, Action callBack)
        {
            _resPathList = path;
            _callBack = callBack;
            StartLoad();
        }

        private void StartLoad()
        {
            if (_loadIndex >= _resPathList.Length)
            {
                OnLoadComplete();
                return;
            }

            var path = _resPathList[_loadIndex++];
            if (string.IsNullOrEmpty(path))
            {
                StartLoad();
                return;
            }

            Load(path);
        }

        private void Load(string path)
        {
            path = path.ToLower();
            if (!_bundleManager.pathToBundle.ContainsKey(path))
            {
                Debug.LogErrorFormat("加载资源时，找不到对应资源的ab包。path={0}", path);
                return;
            }

            if (_bundleManager.HasBundleByAssetsPath(path))
                StartLoad();
            else
                AssetsManagerRuntime.Instance.Coroutine(LoadAssetBundle(path));
        }


        private IEnumerator LoadAssetBundle(string resPath)
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
                StartLoad();
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
                if (_bundleManager.HasBundleByBundleName(abp))
                {
                    continue;
                }

                var realPath = $"{BuildSetting.AssetSetting.GetLoadPath}/{abp}";
                var bundle = AssetBundle.LoadFromFile(realPath);
                _bundleManager.AddBundle(bundle);
            }
        }

        private void OnLoadComplete()
        {
            _callBack?.Invoke();
        }
    }
}