using System;
using System.Collections;
using UnityEngine;

namespace JAssetsManager
{
    /// <summary>
    /// ab加载任务代理
    /// </summary>
    public class BundleLoader : ILoader
    {
        private AssetBundleManifest _assetBundleManifest;

        public BundleLoader()
        {
            InitMainFest();
        }

        public void InitMainFest()
        {
            var mainBundle =
                AssetBundle.LoadFromFile(
                    $"{BuildSetting.AssetSetting.GetLoadPath}/{BuildSetting.AssetSetting.GetBuildTag}");
            if (mainBundle != null)
            {
                _assetBundleManifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            else
                Debug.LogError("mian ab资源加载错误");
            
            mainBundle.Unload(false);
        }

        public void Load(string path, Action callBack)
        {
            path = path.ToLower();
            if (AssetsManager.Instance.HasAssets(path))
            {
                callBack?.Invoke();
            }
            else
            {
                JAssetsManagerRuntime.Instance.Coroutine(loadAssetBundle(path, callBack));
            }
        }

        IEnumerator loadAssetBundle(string resPath, Action callBack)
        {
            loadDependencies(resPath);
            //加载依赖项
            var trueLoadPath =
                $"{BuildSetting.AssetSetting.GetLoadPath}/{resPath}.ab";
            var abLoader = AssetBundle.LoadFromFileAsync(trueLoadPath);
            yield return abLoader;
            if (abLoader.assetBundle == null)
            {
                Debug.LogError("Load  ab File Error=============" + trueLoadPath);
            }
            else
            {
                AssetsManager.Instance.AddBundle(resPath,abLoader.assetBundle);
                callBack?.Invoke();
            }
        }

        /// <summary>
        /// 收集依赖
        /// </summary>
        /// <param name="loadPath"></param>
        /// <returns></returns>
        void loadDependencies(string loadPath)
        {
            var abPaths = _assetBundleManifest.GetAllDependencies($"{loadPath}.ab");
            for (int i = 0; i < abPaths.Length; i++)
            {
                var abp = abPaths[i].ToLower().Replace(".ab", "");
                var realPath = $"{BuildSetting.AssetSetting.GetLoadPath}/{abp}.ab";
                if (AssetsManager.Instance.HasAssets(abp))
                {
                    continue;
                }

                var bundle = AssetBundle.LoadFromFile(realPath);
                AssetsManager.Instance.AddBundle(abp, bundle);
            }
        }
    }
}