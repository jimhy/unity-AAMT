using System;

namespace AAMT
{
    public class BundleLoaderBatch : ILoader
    {
        private readonly BundleManager _bundleManager;
        private string[] _resPathList;
        private Action _callBack;
        private int _loadCompleteCount;

        public BundleLoaderBatch()
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
            foreach (var path in _resPathList)
            {
                new LoadTask(path, OnTaskComplete).Start();
            }
        }

        private void OnTaskComplete()
        {
            if (++_loadCompleteCount >= _resPathList.Length)
            {
                OnLoadComplete();
            }
        }

        private void OnLoadComplete()
        {
            _callBack?.Invoke();
        }
    }
}