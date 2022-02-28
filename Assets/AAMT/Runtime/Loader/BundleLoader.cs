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
        public LoaderHandler Load(string[] path)
        {
            return LoadTask.GetTask(path).Run();
        }
    }
}