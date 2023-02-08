using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    public class LoadLocalTask
    {
        private LocalAssetManager _manager;
        private AsyncHandler _asyncHandler;
        private List<string> _resPaths;

        private LoadLocalTask(string[] resPaths)
        {
            _manager = AAMTManager.Instance.resourceManager as LocalAssetManager;
            _resPaths = new List<string>();
            var loadPath = $"{SettingManager.assetSetting.LoadPath}/";
            foreach (var p in resPaths)
            {
                if (p.LastIndexOf(".") == -1)
                {
                    var dir = $"{loadPath}{p}";
                    if (Directory.Exists(dir))
                    {
                        var fs = Directory.GetFiles(dir);
                        foreach (var s in fs)
                        {
                            if (s.EndsWith(".meta")) continue;
                            var ss = s.ToLower().Replace(loadPath, "").Replace("\\", "/");
                            _resPaths.Add(ss);
                        }

                        continue;
                    }
                }

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
            _asyncHandler.OnComplete();
        }

        public AsyncHandler Run()
        {
            _asyncHandler = new AsyncHandler();
            _asyncHandler.totalCount = _resPaths.Count;
            if (_resPaths.Count > 0)
            {
                AAMTRuntime.Instance.StartCoroutine(Load());
            }
            else
            {
                OnLoadComplete();
            }

            return _asyncHandler;
        }

        IEnumerator Load()
        {
#if UNITY_EDITOR
            while (_resPaths.Count > 0)
            {
                var assetName = _resPaths[0];
                if (!_manager.HasAssetsByPath(assetName))
                {
                    var assetPath = $"{SettingManager.assetSetting.LoadPath}/{assetName}";
                    var obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    if (obj == null)
                    {
                        Debug.LogErrorFormat("加载资源失败,assetPath={0}", assetPath);
                    }
                    else
                    {
                        _manager.AddAsset(assetName, obj);
                    }
                }

                yield return 0;

                OnLoadOneAssetComplete(assetName);
            }
#else
            yield return 0;

#endif
        }

        private void OnLoadOneAssetComplete(string assetName)
        {
            _asyncHandler.currentCount++;
            if (_asyncHandler.currentCount > _asyncHandler.totalCount)
            {
                _asyncHandler.currentCount = _asyncHandler.totalCount;
                Debug.LogErrorFormat("这里不可能出现这种问题,请检查逻辑.");
            }

            var i = _resPaths.IndexOf(assetName);
            if (i != -1)
            {
                _resPaths.RemoveAt(i);
            }

            if (_resPaths.Count == 0)
            {
                _asyncHandler.currentCount = 1;
                OnLoadComplete();
            }

            _asyncHandler.OnProgress();
        }
    }
}