using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    public class LoadLocalTask
    {
        private LocalAssetManager _manager;
        private LoaderHandler _loaderHandler;
        private List<string> _resPaths;

        private LoadLocalTask(string[] resPaths)
        {
            _manager = AssetsManager.Instance.ResourceManager as LocalAssetManager;
            _resPaths = new List<string>();
            foreach (var p in resPaths)
            {
                var resPath = Tools.FilterSpriteUri(p).ToLower();
                _resPaths.Add(resPath);
            }
        }

        public static LoadLocalTask GetTask(string[] resPath)
        {
            return new LoadLocalTask(resPath);
        }

        private void OnLoadComplete()
        {
            _loaderHandler.OnComplete();
        }

        public LoaderHandler Run()
        {
            _loaderHandler = new LoaderHandler();
            _loaderHandler.totalCount = _resPaths.Count;
            if (_resPaths.Count > 0)
            {
                AssetsManagerRuntime.Instance.StartCoroutine(Load());
            }
            else
            {
                OnLoadComplete();
            }

            return _loaderHandler;
        }

        IEnumerator Load()
        {
            while (_resPaths.Count > 0)
            {
                var assetName = _resPaths[0];
                if (!_manager.HasAssetsByPath(assetName))
                {
                    var assetPath = $"{SettingManager.AssetSetting.GetLoadPath}/{assetName}";
                    var obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    if (obj == null)
                    {
                        Debug.LogErrorFormat("加载资源失败,abName={0}", assetName);
                    }
                    else
                    {
                        _manager.AddAsset(assetName, obj);
                    }
                }

                yield return 0;

                OnLoadOneAssetComplete(assetName);
            }
        }

        private void OnLoadOneAssetComplete(string assetName)
        {
            _loaderHandler.currentCount++;
            if (_loaderHandler.currentCount > _loaderHandler.totalCount)
            {
                _loaderHandler.currentCount = _loaderHandler.totalCount;
                Debug.LogErrorFormat("这里不可能出现这种问题,请检查逻辑.");
            }

            var i = _resPaths.IndexOf(assetName);
            if (i != -1)
            {
                _resPaths.RemoveAt(i);
            }

            if (_resPaths.Count == 0)
            {
                _loaderHandler.currentCount = 1;
                OnLoadComplete();
            }

            _loaderHandler.OnProgress();
        }
    }
}