using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AAMT
{
    public class LoadTask
    {
        private readonly BundleManager _bundleManager;
        private string _resPath;
        private Action _callBack;

        public LoadTask(string resPath, Action callBack)
        {
            _bundleManager = AssetsManager.Instance.bundleManager;
            _resPath = resPath.ToLower();
            _callBack = callBack;
        }

        public void Start()
        {
            if (string.IsNullOrEmpty(_resPath))
            {
                Debug.LogError("加载的资源为空.");
                OnLoadComplete();
                return;
            }

            if (!_bundleManager.pathToBundle.ContainsKey(_resPath))
            {
                Debug.LogErrorFormat("加载资源时，找不到对应资源的ab包。path={0}", _resPath);
                OnLoadComplete();
                return;
            }

            StartLoad();
        }

        private async void StartLoad()
        {
            var abName = _bundleManager.pathToBundle[_resPath];
            //加载依赖项

            var results = await LoadDependencies(abName);
            foreach (var assetBundleCreateRequest in results)
            {
                AssetsManager.Instance.bundleManager.AddBundle(assetBundleCreateRequest.assetBundle);
            }

            if (!_bundleManager.HasBundleByBundleName(abName))
            {
                var abRequest = await Load(abName);
                AssetsManager.Instance.bundleManager.AddBundle(abRequest.assetBundle);
            }

            OnLoadComplete();
        }

        private Task<AssetBundleCreateRequest> Load(string abName)
        {
            var abPath = $"{BuildSetting.AssetSetting.GetLoadPath}/{abName}";
            var result = AssetBundle.LoadFromFileAsync(abPath);
            return Task.FromResult(result);
        }

        /// <summary>
        /// 加载依赖
        /// </summary>
        /// <param name="loadPath"></param>
        /// <returns></returns>
        private Task<AssetBundleCreateRequest[]> LoadDependencies(string targAbName)
        {
            var tasks = new List<Task<AssetBundleCreateRequest>>();
            var abPaths = _bundleManager.assetBundleManifest.GetAllDependencies(targAbName);
            for (int i = 0; i < abPaths.Length; i++)
            {
                var abName = abPaths[i].ToLower();
                if (_bundleManager.HasBundleByBundleName(abName))
                {
                    continue;
                }

                tasks.Add(Load(abName));
            }

            return Task.WhenAll(tasks);
        }

        private void OnLoadComplete()
        {
            _resPath = null;
            _callBack?.Invoke();
            _callBack = null;
        }
    }
}