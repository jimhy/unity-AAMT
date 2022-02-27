using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AAMT
{
    public class LoadTask
    {
        private readonly BundleManager _bundleManager;
        private Action<object> _callBack;
        private readonly object _data;

        private static readonly FlexibleDictionary<string, bool> CommonLoadingAbNames =
            new FlexibleDictionary<string, bool>();

        private readonly List<string> _loadingAbNames;

        private LoadTask(string[] resPaths, Action<object> callBack, object data)
        {
            _loadingAbNames = new List<string>();
            _bundleManager = AssetsManager.Instance.bundleManager;
            _data = data;
            _callBack = callBack;
            Init(resPaths);
        }

        public static LoadTask GetTask(string[] resPath, Action<object> callBack, object data)
        {
            return new LoadTask(resPath, callBack, data);
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
            if (string.IsNullOrEmpty(resPath))
            {
                Debug.LogError("加载的资源为空.");
                OnLoadComplete();
                return;
            }

            if (!_bundleManager.pathToBundle.ContainsKey(resPath))
            {
                Debug.LogErrorFormat("加载资源时，找不到对应资源的ab包。path={0}", resPath);
                OnLoadComplete();
                return;
            }

            var abName = _bundleManager.pathToBundle[resPath];

            if (AddToAbNameList(abName))
            {
                _loadingAbNames.Add(abName);
                DetDependenciesAbNames(abName);
            }
        }

        private void DetDependenciesAbNames(string sourceAbName)
        {
            var abPaths = _bundleManager.assetBundleManifest.GetAllDependencies(sourceAbName);
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

        public void Run()
        {
            foreach (var loadingAbName in _loadingAbNames)
            {
                AssetsManagerRuntime.Instance.StartCoroutine(Load(loadingAbName));
            }
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
                OnLoadComplete();
            }
        }

        private bool AddToAbNameList(string abName)
        {
            if (CommonLoadingAbNames.ContainsKey(abName)) return false;
            CommonLoadingAbNames.Add(abName, true);
            return true;
        }

        private void OnLoadComplete()
        {
            _callBack?.Invoke(_data);
            _callBack = null;
        }
    }
}