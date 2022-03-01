using UnityEngine;

namespace AAMT
{
    public class BundleHandle
    {
        internal int referenceCount { get; private set; }
        private readonly AssetBundle _assetBundle;

        internal BundleHandle(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;
        }

        internal AssetBundleRequest LoadAssetAsync<T>(string assetName) where T : Object
        {
            referenceCount++;
            return _assetBundle.LoadAssetAsync<T>(assetName);
        }

        internal AssetBundleRequest LoadAssetWithSubAssetsAsync<T>(string assetName) where T : Object
        {
            referenceCount++;
            return _assetBundle.LoadAssetWithSubAssetsAsync<T>(assetName);
        }
        private void AddDependencyReference()
        {
            referenceCount++;
        }

        private void RemoveDependencyReference()
        {
            Release();
        }
        
        internal void Release()
        {
            if (--referenceCount <= 0) Destroy();
        }

        internal void Destroy()
        {
            _assetBundle.UnloadAsync(true);
        }
    }
}