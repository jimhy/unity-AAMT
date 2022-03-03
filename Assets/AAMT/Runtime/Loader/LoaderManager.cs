using System;

namespace AAMT
{
    internal class LoaderManager
    {
        private ILoader _loader;

        internal LoaderManager()
        {
            if (SettingManager.AssetSetting.GetLoadType == AssetSetting.LoadType.Bundle)
                _loader = new BundleLoader();
            else if (SettingManager.AssetSetting.GetLoadType == AssetSetting.LoadType.LocalAssets)
                _loader = new LocalLoader();
        }

        internal LoaderHandler Load(string[] path)
        {
            if (_loader != null) return _loader.Load(path);
            return default;
        }
    }

    public class LoaderHandler
    {
        internal int currentCount;
        internal int totalCount;
        public long currentBytes { get; internal set; }
        public long totalBytes { get; internal set; }
        public object customData { get; set; }
        public Action<LoaderHandler> onComplete;
        public Action<LoaderHandler> onProgress;

        public float progress => totalCount == 0 ? 0 : (float) currentCount / (float) totalCount;

        internal void OnProgress()
        {
            onProgress?.Invoke(this);
        }

        internal void OnComplete()
        {
            onComplete?.Invoke(this);
        }
    }
}