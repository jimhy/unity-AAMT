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
            _assetBundle = assetBundle;
            _bundleManager = AAMTManager.Instance.ResourceManager as BundleManager;
            CalculationReferenceDependency(true);
        }

        internal AssetBundleRequest LoadAssetAsync<T>(string assetName) where T : Object
        {
            ReferenceCount++;
            Debug.LogFormat("增加引用计数,abName{0},当前引用计数:{1}", _assetBundle.name, ReferenceCount);
            return _assetBundle.LoadAssetAsync<T>(assetName);
        }

        internal AssetBundleRequest LoadAssetWithSubAssetsAsync<T>(string assetName) where T : Object
        {
            ReferenceCount++;
            Debug.LogFormat("增加引用计数SubAssets,abName:{0},当前引用计数:{1}", _assetBundle.name, ReferenceCount);
            return _assetBundle.LoadAssetWithSubAssetsAsync<T>(assetName);
        }

        private void CalculationReferenceDependency(bool isAdd)
        {
            var deps = _bundleManager.AssetBundleManifest.GetDirectDependencies(_assetBundle.name);
            foreach (var dep in deps)
            {
                var bundle = _bundleManager.GetBundleByBundleName(dep);
                if (bundle != null)
                {
                    if (isAdd) bundle.AddDependencyReference(_assetBundle.name);
                    else bundle.RemoveDependencyReference(_assetBundle.name);
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
            Debug.LogFormat("增加依赖引用计数,依赖者ab名称:{0},当前被依赖者:名称{1},引用计数:{2}", rootAbName, _assetBundle.name,
                ReferenceCount);
        }

        private void RemoveDependencyReference(string rootAbName)
        {
            Release();
            Debug.LogFormat("减少依赖引用计数,依赖者ab名称:{0},当前被依赖者:名称{1},引用计数:{2}", rootAbName, _assetBundle.name,
                ReferenceCount);
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
            Debug.LogFormat("释放Bundle资源,abName{0}", _assetBundle.name);
            CalculationReferenceDependency(false);
            _assetBundle.UnloadAsync(true);
            _assetBundle = null;
            _bundleManager = null;
            _waitingDependency = null;
        }
    }
}