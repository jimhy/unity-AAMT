using System;
using UnityEngine;

namespace AAMT
{
    public class BundleLoader : ILoader
    {
        private readonly BundleManager _bundleManager;

        public BundleLoader()
        {
            _bundleManager = AssetsManager.Instance.bundleManager;
        }
        public void Load(string[] path, Action<object> callBack, object data)
        {
            LoadTask.GetTask(path, callBack, data).Run();
        }
    }
}