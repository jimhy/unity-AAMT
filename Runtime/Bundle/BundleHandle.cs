using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AAMT
{
    public class BundleHandle
    {
        internal int ReferenceCount { get; private set; }
        private AssetBundle _assetBundle;
        private BundleManager _bundleManager;
        private List<string> _waitingDependency;

        internal BundleHandle(AssetBundle assetBundle)
        {
            _waitingDependency = new List<string>();
            _assetBundle       = assetBundle;
            _bundleManager     = AAMTManager.Instance.resourceManager as BundleManager;
            CalculationReferenceDependency(true);
        }

        internal AssetBundleRequest LoadAssetAsync<T>(string assetName) where T : Object
        {
            AddDependencyReference(_assetBundle.name);
            return _assetBundle.LoadAssetAsync<T>(assetName);
        }

        internal AssetBundleRequest LoadAllAssetAsync()
        {
            AddDependencyReference(_assetBundle.name);
            return _assetBundle.LoadAllAssetsAsync();
        }

        internal Object[] LoadAllAsset()
        {
            AddDependencyReference(_assetBundle.name);
            return _assetBundle.LoadAllAssets();
        }

        internal AssetBundleRequest LoadAssetWithSubAssetsAsync<T>(string assetName) where T : Object
        {
            AddDependencyReference(_assetBundle.name);
            return _assetBundle.LoadAssetWithSubAssetsAsync<T>(assetName);
        }

        /// <summary>
        /// 计算引用的依赖性
        /// </summary>
        /// <param name="isAdd"></param>
        private void CalculationReferenceDependency(bool isAdd)
        {
            var deps = _bundleManager.assetBundleManifest.GetDirectDependencies(_assetBundle.name);
            foreach (var dep in deps)
            {
                var bundle = _bundleManager.GetBundleByBundleName(dep);
                if (bundle != null)
                {
                    if (isAdd) bundle.AddDependencyReference(_assetBundle.name);
                    else if (bundle.RemoveDependencyReference(_assetBundle.name))
                    {
                        _bundleManager.RemoveBundleByBundleName(dep);
                    }
                }
                else
                {
                    _waitingDependency.Add(dep);
                }
            }

            if (_waitingDependency.Count > 0)
            {
                AAMTRuntime.Instance.StartCoroutine(CheckWaitingDependency());
            }
        }

        private int checkWaitingDependencyTimes;

        private IEnumerator CheckWaitingDependency()
        {
            while (_waitingDependency.Count > 0)
            {
                for (var i = _waitingDependency.Count - 1; i >= 0; i--)
                {
                    var abName = _waitingDependency[i];
                    var bundle = _bundleManager.GetBundleByBundleName(abName);
                    if (bundle != null)
                    {
                        bundle.AddDependencyReference(_assetBundle.name);
                        _waitingDependency.RemoveAt(i);
                    }
                }

                if (++checkWaitingDependencyTimes >= 60 && _waitingDependency.Count > 0)
                {
                    foreach (var s in _waitingDependency)
                    {
                        Debug.LogErrorFormat("找不到被依赖的Bundle:{0},当前bundle:{1}", s, _assetBundle.name);
                    }

                    yield break;
                }

                yield return 0;
            }
        }

        private void AddDependencyReference(string rootAbName)
        {
            ReferenceCount++;
            Debug.LogFormat("增加依赖引用计数,依赖者ab名称:{0},当前被依赖者:名称{1},引用计数:{2}", rootAbName, _assetBundle.name, ReferenceCount);
        }

        /// <summary>
        /// 减少引用计数
        /// </summary>
        /// <param name="rootAbName"></param>
        /// <returns>返回是否已经销毁,true:已经销毁,false:没有销毁</returns>
        private bool RemoveDependencyReference(string rootAbName)
        {
            Debug.LogFormat("减少依赖引用计数,依赖者ab名称:{0},当前被依赖者:名称{1},引用计数:{2}", rootAbName, _assetBundle.name, ReferenceCount - 1);
            Release();
            return ReferenceCount <= 0;
        }

        internal void Release()
        {
            --ReferenceCount;
            Debug.LogFormat("减少引用计数,当前引用计数:ab{0},count:{1}", _assetBundle.name, ReferenceCount);
            if (ReferenceCount <= 0) Destroy();
        }

        internal void Destroy()
        {
            if (_assetBundle == null) return;
            CalculationReferenceDependency(false);
            Debug.LogFormat("释放Bundle资源,abName:{0}", _assetBundle.name);
            _assetBundle.Unload(true);
            _assetBundle       = null;
            _bundleManager     = null;
            _waitingDependency = null;
        }
    }
}