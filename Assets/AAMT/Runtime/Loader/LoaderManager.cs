using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AAMT
{
    internal class LoaderManager
    {
        private ILoader _loader;

        internal LoaderManager()
        {
            if (SettingManager.assetSetting.getBuildTarget == AssetSetting.BuildTarget.editor)
                _loader = new LocalLoader();
            else
                _loader = new BundleLoader();
        }

        internal AsyncHandler LoadAsync(string[] path)
        {
#if UNITY_EDITOR
            foreach (var s in path)
            {
                Debug.LogFormat("Start to load-->{0}", s);
            }
#endif
            if (_loader != null) return _loader.LoadAsync(path);
            return default;
        }
    }
}