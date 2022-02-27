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

        internal void Load(string[] path, Action<object> callBack)
        {
            _loader.Load(path, callBack, null);
        }

        internal void Load(string[] path, Action<object> callBack, object data)
        {
            if (_loader != null) _loader.Load(path, callBack, data);
        }

        internal void LoadBatch(string[] path, Action<object> callBack)
        {
            LoadBatch(path, callBack, null);
        }

        internal void LoadBatch(string[] path, Action<object> callBack, object data)
        {
            if (_loaderBatch != null) _loaderBatch.Load(path, callBack, data);
        }
    }
}