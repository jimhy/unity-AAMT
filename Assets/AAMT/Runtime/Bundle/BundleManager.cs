using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class BundleManager : IResourceManager
    {
        internal AssetBundleManifest AssetBundleManifest { get; private set; }
        internal Dictionary<string, string> PathToBundle { get; private set; }
        internal Dictionary<string, BundleHandle> Bundles { get; }
        private readonly SpriteAtlasManager _atlasManager;

        internal BundleManager()
        {
            Bundles = new Dictionary<string, BundleHandle>();
            _atlasManager = new SpriteAtlasManager(this);
            InitManifest();
            InitBundleMap();
        }

        private void InitManifest()
        {
            var mainBundle =
                AssetBundle.LoadFromFile(
                    $"{SettingManager.AssetSetting.GetLoadPath}/{SettingManager.AssetSetting.GetBuildTargetToString}");
            if (mainBundle != null)
                AssetBundleManifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            else
                Debug.LogError("Manifest ab资源加载错误");

            mainBundle.Unload(false);
        }

        private void InitBundleMap()
        {
            PathToBundle = new Dictionary<string, string>();
            const string fileName = "assets-width-bundle";
            var path = $"{SettingManager.AssetSetting.GetLoadPath}/{fileName}.txt";
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

        private string ReadTextFileData(string path)
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
            Debug.LogFormat("AddBundle:{0}", ab.name);
            if (!Bundles.ContainsKey(ab.name)) Bundles.Add(ab.name, new BundleHandle(ab));
            else Debug.LogErrorFormat("重复添加ab包，应该是出现了重复加载，会出现双份内存的情况，请检查。path:{0}", ab.name);
        }

        internal bool HasBundleByBundleName(string abName)
        {
            return Bundles.ContainsKey(abName);
        }

        internal BundleHandle GetBundleByBundleName(string abName)
        {
            if (Bundles.ContainsKey(abName))
            {
                return Bundles[abName];
            }

            return null;
        }

        public bool HasAssetsByPath(string assetPath)
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
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(AAMTSpriteAtlas))
                _atlasManager.GetAssets(path, callBack);
            else AAMTRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
        }

        public void ChangeScene(string path, Action callBack)
        {
            if (string.IsNullOrEmpty(path) || !HasAssetsByPath(path.ToLower()))
            {
                Debug.LogErrorFormat("加载场景失败,path:{0}", path);
                callBack?.Invoke();
                return;
            }

            AAMTRuntime.Instance.StartCoroutine(LoadScene(path, callBack));
        }

        private IEnumerator LoadScene(string path, Action callBack)
        {
            var sceneName = Tools.FilterSceneName(path);
            yield return SceneManager.LoadSceneAsync(sceneName);
            callBack?.Invoke();
        }

        private IEnumerator<AssetBundleRequest> StartGetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            Tools.ParsingLoadUri(path, out var abName, out var itemName, out _);
            if (abName == null || itemName == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0},itemName:{1}", abName, itemName);
                callBack?.Invoke(default);
                yield break;
            }

            if (!Bundles.ContainsKey(abName))
            {
                callBack?.Invoke(default);
                yield break;
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

        public void Release(string path)
        {
            var abName = CheckAndGetAbName(path);
            if (!string.IsNullOrEmpty(abName)) Bundles[abName].Release();
        }

        public void Destroy(string path)
        {
            var abName = CheckAndGetAbName(path);
            if (!string.IsNullOrEmpty(abName)) Bundles[abName].Destroy();
        }

        private string CheckAndGetAbName(string path)
        {
            Tools.ParsingLoadUri(path, out var abName, out _, out _);
            if (abName == null)
            {
                Debug.LogErrorFormat("释放资源失败，找不到abName,path={0}", path);
                return string.Empty;
            }

            if (!Bundles.ContainsKey(abName))
            {
                Debug.LogErrorFormat("释放资源失败，找不到abName 对应的ab包", path);
                return string.Empty;
            }

            return abName;
        }
    }
}