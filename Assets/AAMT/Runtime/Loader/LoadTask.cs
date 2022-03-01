using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace AAMT
{
    public class LoadingABHandle
    {
        public Type t;
    }

    public class LoadTask
    {
        private readonly BundleManager _bundleManager;
        private LoaderHandler _loaderHandler;

        private static readonly FlexibleDictionary<string, bool> CommonLoadingAbNames =
            new FlexibleDictionary<string, bool>();

        private readonly List<string> _loadingAbNames;
        private readonly List<string> _alreadyLoadingAbNames;

        private LoadTask(string[] resPaths)
        {
            _loadingAbNames = new List<string>();
            _alreadyLoadingAbNames = new List<string>();
            _bundleManager = AssetsManager.Instance.bundleManager;
            Init(resPaths);
        }

        public static LoadTask GetTask(string[] resPath)
        {
            return new LoadTask(resPath);
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
            resPath = Tools.FilterSpriteUri(resPath);
            if (string.IsNullOrEmpty(resPath))
            {
                Debug.LogError("加载的资源为空.");
                OnLoadComplete();
                return;
            }

            if (!_bundleManager.PathToBundle.ContainsKey(resPath))
            {
                Debug.LogErrorFormat("加载资源时，找不到对应资源的ab包。path={0}", resPath);
                OnLoadComplete();
                return;
            }

            var abName = _bundleManager.PathToBundle[resPath];

            if (AddToAbNameList(abName))
            {
                _loadingAbNames.Add(abName);
                DetDependenciesAbNames(abName);
            }
        }

        private void DetDependenciesAbNames(string sourceAbName)
        {
            var abPaths = _bundleManager.AssetBundleManifest.GetAllDependencies(sourceAbName);
            for (int i = 0; i < abPaths.Length; i++)
            {
                var abName = abPaths[i].ToLower();
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

        public LoaderHandler Run()
        {
            _loaderHandler = new LoaderHandler();
            _loaderHandler.totalCount = _loadingAbNames.Count;
            if (_loadingAbNames.Count > 0)
            {
                foreach (var loadingAbName in _loadingAbNames)
                {
                    AssetsManagerRuntime.Instance.StartCoroutine(Load(loadingAbName));
                }
            }
            else
            {
                AssetsManagerRuntime.Instance.StartCoroutine(CheckAlreadyLoadingAbs());
            }

            return _loaderHandler;
        }

        IEnumerator Load(string abName)
        {
            var abPath = $"{BuildSetting.AssetSetting.GetLoadPath}/{abName}";
            var request = AssetBundle.LoadFromFileAsync(abPath);
            yield return request;
            if (request.assetBundle == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName={0}", abName);
            }
            else
            {
                _bundleManager.AddBundle(request.assetBundle);
                OnLoadOnAbComplete(abName);
            }
        }

        private void OnLoadOnAbComplete(string abName)
        {
            _loaderHandler.currentCount++;
            if (_loaderHandler.currentCount > _loaderHandler.totalCount)
            {
                _loaderHandler.currentCount = _loaderHandler.totalCount;
                Debug.LogErrorFormat("这里不可能出现这种问题,请检查逻辑.");
            }

            _loaderHandler.OnProgress();
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
                AssetsManagerRuntime.Instance.StartCoroutine(CheckAlreadyLoadingAbs());
            }
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
            _loaderHandler.OnComplete();
        }

        IEnumerator CheckAlreadyLoadingAbs()
        {
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