using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AAMT
{
    public class LoadBundleTask
    {
        private readonly BundleManager _bundleManager;
        private AsyncHandler _asyncHandler;

        private static readonly Dictionary<string, bool> CommonLoadingAbNames = new();

        private readonly List<string> _loadingAbNames;
        private readonly List<string> _alreadyLoadingAbNames;

        private LoadBundleTask(string[] resPaths)
        {
            _loadingAbNames        = new List<string>();
            _alreadyLoadingAbNames = new List<string>();
            _bundleManager         = AAMTManager.Instance.resourceManager as BundleManager;
            Init(resPaths);
        }

        public static LoadBundleTask GetTask(string[] resPath)
        {
            return new LoadBundleTask(resPath);
        }

        private void Init(string[] resPaths)
        {
            foreach (var resPath in resPaths)
            {
                InitPath(resPath);
            }
        }

        private void InitPath(string resPath)
        {
            resPath = resPath.ToLower();
            resPath = Tools.FilterSpriteUri(resPath);
            if (string.IsNullOrEmpty(resPath))
            {
                Debug.LogError("加载的资源为空.");
                OnLoadComplete();
                return;
            }

            string abName = string.Empty;

            if (resPath.LastIndexOf(".ab") != -1) //这里是直接加载指定ab包
            {
                abName = resPath;
            }
            else
            {
                if (!_bundleManager.pathToBundle.ContainsKey(resPath))
                {
                    Debug.LogErrorFormat("加载资源时，找不到对应资源的ab包。path={0}/{1}", SettingManager.assetSetting.getLoadPath, resPath);
                    OnLoadComplete();
                    return;
                }

                abName = _bundleManager.pathToBundle[resPath];
            }


            if (!AddToAbNameList(abName)) return;
            _loadingAbNames.Add(abName);
            DetDependenciesAbNames(abName);
        }

        /// <summary>
        /// 依赖性
        /// </summary>
        /// <param name="sourceAbName"></param>
        private void DetDependenciesAbNames(string sourceAbName)
        {
            var abPaths = _bundleManager.assetBundleManifest.GetAllDependencies(sourceAbName);
            foreach (var t in abPaths)
            {
                var abName = t.ToLower();
                if (_bundleManager.HasBundleByBundleName(abName))
                {
                    continue;
                }

                if (AddToAbNameList(abName))
                {
                    _loadingAbNames.Add(abName);
                }
            }
        }

        public AsyncHandler RunAsync()
        {
            _asyncHandler = new AsyncHandler { totalCount = _loadingAbNames.Count };
            if (_loadingAbNames.Count > 0)
            {
                for (int i = _loadingAbNames.Count - 1; i >= 0; i--)
                {
                    Load(_loadingAbNames[i]);
                }
            }
            else
            {
                AAMTRuntime.Instance.StartCoroutine(CheckAlreadyLoadingAbs());
            }

            return _asyncHandler;
        }

        private void Load(string abName)
        {
            var abPath = $"{SettingManager.assetSetting.getLoadPath}/{abName}";
            Debug.LogFormat("loading bundle:{0}", abPath);
            if (_bundleManager.HasBundleByBundleName(abName))
            {
                Debug.Log($"已经存在 {abName}");
                OnLoadOneAbComplete(abName);
            }
            else
            {
                Tools.LoadBundleAsync(abPath, OnLoadBundleComplete);
            }
        }

        private void OnLoadBundleComplete(AssetBundle bundle)
        {
            if (bundle == null)
            {
                Debug.LogErrorFormat("Load bundle faile!!");
                return;
            }

            _bundleManager.AddBundle(bundle);
            OnLoadOneAbComplete(bundle.name);
        }

        private void OnLoadOneAbComplete(string abName)
        {
            _asyncHandler.currentCount++;
            if (_asyncHandler.currentCount > _asyncHandler.totalCount)
            {
                _asyncHandler.currentCount = _asyncHandler.totalCount;
                Debug.LogErrorFormat("这里不可能出现这种问题,请检查逻辑.");
            }

            var i = _loadingAbNames.IndexOf(abName);
            if (i != -1)
            {
                _loadingAbNames.RemoveAt(i);
            }

            if (CommonLoadingAbNames.ContainsKey(abName))
            {
                CommonLoadingAbNames.Remove(abName);
            }

            if (_loadingAbNames.Count == 0)
            {
                _asyncHandler.currentCount = 1;
                AAMTRuntime.Instance.StartCoroutine(CheckAlreadyLoadingAbs());
            }

            _asyncHandler.OnProgress();
        }

        private bool AddToAbNameList(string abName)
        {
            if (CommonLoadingAbNames.ContainsKey(abName))
            {
                _alreadyLoadingAbNames.Add(abName);
                return false;
            }

            CommonLoadingAbNames.Add(abName, true);
            return true;
        }

        private void OnLoadComplete()
        {
            _asyncHandler.OnComplete();
        }

        private IEnumerator CheckAlreadyLoadingAbs()
        {
            if (_alreadyLoadingAbNames.Count <= 0)
            {
                //为了防止已经加载过的资源没有回调，
                //这里需要下一帧再执行下面的逻辑，因为需要用户用返回的handler注册complete事件。
                yield return 0;
            }

            while (_alreadyLoadingAbNames.Count > 0)
            {
                for (int i = _alreadyLoadingAbNames.Count - 1; i >= 0; i--)
                {
                    var abName = _alreadyLoadingAbNames[i];
                    if (!CommonLoadingAbNames.ContainsKey(abName))
                    {
                        _alreadyLoadingAbNames.RemoveAt(i);
                    }
                }

                yield return 0;
            }

            OnLoadComplete();
        }
    }
}