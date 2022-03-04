using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AAMT
{
    public class BundleLoader : ILoader
    {
        public BundleLoader()
        {
        }

        public LoaderHandler Load(string[] path)
        {
            return LoadBundleTask.GetTask(path).Run();
        }
    }
}