using System;
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

        internal AsyncHandler Load(string[] path)
        {
            if (_loader != null) return _loader.Load(path);
            return default;
        }
    }
}