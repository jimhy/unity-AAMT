using System;

namespace AAMT
{
    internal class LoaderManager
    {
        private ILoader _loader;
        private ILoader _loaderBatch;

        internal LoaderManager()
        {
            if (BuildSetting.AssetSetting.GetLoadType == BuildSetting.LoadType.Bundle)
            {
                _loader = new BundleLoader();
                _loaderBatch = new BundleLoaderBatch();
            }
        }

        internal void Load(string[] path, Action callBack)
        {
            if (_loader != null) _loader.Load(path, callBack);
        }

        internal void LoadBatch(string[] path, Action callBack)
        {
            if (_loaderBatch != null) _loaderBatch.Load(path, callBack);
        }
    }
}