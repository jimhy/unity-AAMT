using System;

namespace AAMT
{
    internal class LoaderManager
    {
        private ILoader _loader;

        internal LoaderManager()
        {
            if (BuildSetting.AssetSetting.GetLoadType == BuildSetting.LoadType.Bundle)
            {
                _loader = new BundleLoader();
            }
        }

        internal void Load(string[] path, Action<object> callBack, object data)
        {
            if (_loader != null) _loader.Load(path, callBack, data);
        }
    }
}