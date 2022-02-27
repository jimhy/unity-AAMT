using System;
using UnityEngine;

namespace AAMT
{
    public class BundleLoaderBatch : ILoader
    {
        private readonly BundleManager _bundleManager;

        public BundleLoaderBatch()
        {
            _bundleManager = AssetsManager.Instance.bundleManager;
        }

        public void Load(string[] path, Action<object> callBack)
        {
            Load(path, callBack, null);
        }

        public void Load(string[] path, Action<object> callBack, object data)
        {
            LoadTask.GetTask(path, callBack, data).Run();
        }
    }
}