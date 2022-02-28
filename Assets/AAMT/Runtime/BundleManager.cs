using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class BundleManager
    {
        internal AssetBundleManifest AssetBundleManifest { get; private set; }
        internal Dictionary<string, string> PathToBundle { get; private set; }
        internal Dictionary<string, AssetBundle> Bundles { get; set; }
        private SpriteAtlasManager _atlasManager;

        public BundleManager()
        {
            Bundles = new Dictionary<string, AssetBundle>();
            _atlasManager = new SpriteAtlasManager(this);
            InitManifest();
            InitBundleMap();
        }

        private void InitManifest()
        {
            var mainBundle =
                AssetBundle.LoadFromFile(
                    $"{BuildSetting.AssetSetting.GetLoadPath}/{BuildSetting.AssetSetting.GetBuildTargetToString}");
            if (mainBundle != null)
                AssetBundleManifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            else
                Debug.LogError("mian ab资源加载错误");

            mainBundle.Unload(false);
        }

        private void InitBundleMap()
        {
            PathToBundle = new Dictionary<string, string>();
            var fileName = "assets-width-bundle";
            var path = $"{BuildSetting.AssetSetting.GetLoadPath}/{fileName}.txt";
            Debug.LogFormat("Load assets-width-bundle file.path={0}", path);
            var content = ReadTextFileData(path);
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError("assets-width-bundle 资源加载错误");
                return;
            }

            var strAry = content.Replace("\r", "").Split("\n");
            foreach (var s in strAry)
            {
                if (string.IsNullOrEmpty(s)) continue;
                var ary = s.Split(",");
                PathToBundle[ary[0]] = ary[1];
            }
        }

        internal string ReadTextFileData(string path)
        {
            var request = UnityWebRequest.Get(path);
            request.SendWebRequest();
            while (!request.isDone)
            {
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogErrorFormat("ReadTextFileData error,errorCode:{0},path:{1}", request.result, path);
                return string.Empty;
            }

            return request.downloadHandler.text;
        }

        internal void AddBundle(AssetBundle ab)
        {
            if (ab == null) return;
            // Debug.LogFormat("AddBundle:{0}", ab.name);
            if (!Bundles.ContainsKey(ab.name)) Bundles.Add(ab.name, ab);
            else Debug.LogErrorFormat("重复添加ab包，应该是出现了重复加载，会出现双份内存的情况，请检查。path:{0}", ab.name);
        }

        internal bool HasBundleByBundleName(string abName)
        {
            return Bundles.ContainsKey(abName);
        }

        internal bool HasBundleByAssetsPath(string assetPath)
        {
            assetPath = Tools.FilterSpriteUri(assetPath);
            if (!PathToBundle.ContainsKey(assetPath))
            {
                Debug.LogFormat("找不到对应的ab包。assetPath:{0}", assetPath);
                return false;
            }

            var abName = PathToBundle[assetPath];
            return Bundles.ContainsKey(abName);
        }

        public void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(ASpriteAtlas)) _atlasManager.GetAssets(path, callBack);
            else AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
        }

        private IEnumerator<AssetBundleRequest> StartGetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            path = path.ToLower();
            if (!PathToBundle.ContainsKey(path))
            {
                Debug.LogErrorFormat("获取资源时，找不到对应的ab包。path:{0}", path);
                callBack?.Invoke(default);
                yield break;
            }

            var abName = PathToBundle[path];
            if (!Bundles.ContainsKey(abName))
            {
                callBack?.Invoke(default);
                yield break;
            }

            var itemName = path;
            var n = path.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                itemName = path.Substring(n + 1);
            }

            var request = Bundles[abName].LoadAssetAsync<T>(itemName);
            yield return request;
            if (request.asset == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0},itemName:{1}", abName, itemName);
                callBack?.Invoke(default);
                yield break;
            }

            callBack?.Invoke(request.asset as T);
        }
    }
}