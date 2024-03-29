﻿using System;
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
        internal AssetBundleManifest assetBundleManifest { get; private set; }
        internal Dictionary<string, string> pathToBundle { get; private set; }
        internal Dictionary<string, BundleHandle> bundles { get; }
        private readonly SpriteAtlasManager _atlasManager;

        internal BundleManager()
        {
            bundles       = new Dictionary<string, BundleHandle>();
            _atlasManager = new SpriteAtlasManager(this);
            InitManifest();
            InitBundleMap();
        }

        private void InitManifest()
        {
            var path       = $"{SettingManager.assetSetting.LoadPath}/{SettingManager.assetSetting.BuildPlatform}";
            var mainBundle = Tools.LoadBundle(path);
            if (mainBundle != null)
                assetBundleManifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            else
                Debug.LogError("Manifest ab资源加载错误");

            mainBundle.Unload(false);
        }

        private void InitBundleMap()
        {
            pathToBundle = new Dictionary<string, string>();
            var path = $"{SettingManager.assetSetting.LoadPath}/{AAMTDefine.AAMT_ASSETS_WITH_BUNDLE_NAME}";
            Debug.LogFormat("Load assets-with-bundle file.path={0}", path);
            var content = Tools.ReadTextFileData(path);
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError("assets-with-bundle 资源加载错误");
                return;
            }

            var strAry = content.Replace("\r", "").Split("\n");
            foreach (var s in strAry)
            {
                if (string.IsNullOrEmpty(s)) continue;
                var ary = s.Split(",");
                pathToBundle[ary[0]] = ary[1];
            }
        }

        internal void AddBundle(AssetBundle ab)
        {
            if (ab == null) return;
            Debug.LogFormat("AddBundle:{0}", ab.name);
            if (!bundles.ContainsKey(ab.name)) bundles.Add(ab.name, new BundleHandle(ab));
            else Debug.LogErrorFormat("重复添加ab包，应该是出现了重复加载，会出现双份内存的情况，请检查。path:{0}", ab.name);
        }

        internal bool HasBundleByBundleName(string abName)
        {
            return bundles.ContainsKey(abName);
        }

        internal BundleHandle GetBundleByBundleName(string abName)
        {
            if (bundles.ContainsKey(abName))
            {
                return bundles[abName];
            }

            return null;
        }

        internal BundleHandle RemoveBundleByBundleName(string abName)
        {
            if (bundles.ContainsKey(abName))
            {
                var b = bundles[abName];
                bundles.Remove(abName);
                return b;
            }

            return null;
        }

        public bool HasAssetsByPath(string assetPath)
        {
            var abName = string.Empty;
            if (assetPath.LastIndexOf(".ab", StringComparison.Ordinal) != -1)
            {
                abName = assetPath;
            }
            else
            {
                assetPath = Tools.FilterSpriteUri(assetPath);
                if (!pathToBundle.ContainsKey(assetPath))
                {
                    Debug.LogFormat("找不到对应的ab包。assetPath:{0}", assetPath);
                    return false;
                }

                abName = pathToBundle[assetPath];
            }

            return bundles.ContainsKey(abName);
        }

        public void GetAssetsAsync<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(AAMTSpriteAtlas))
                _atlasManager.GetAssetsAsync(path, callBack);
            else AAMTRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
        }

        public void GetAllAssetsAsync(string path, Action<Object[]> callBack)
        {
            AAMTRuntime.Instance.StartCoroutine(StartGetAllAssets(path, callBack));
        }

        public Object[] GetAllAssets(string path)
        {
            Tools.ParsingLoadUri(path, out var abName, out var itemName, out _);
            if (abName == null || itemName == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0},itemName:{1}", abName, itemName);
                return null;
            }

            if (!bundles.ContainsKey(abName))
            {
                return null;
            }

            var objects = bundles[abName].LoadAllAsset();

            return objects;
        }

        public void ChangeScene(string path, LoadSceneMode sceneMode, Action callBack)
        {
            if (string.IsNullOrEmpty(path) || !HasAssetsByPath(path.ToLower()))
            {
                Debug.LogErrorFormat("加载场景失败,path:{0}", path);
                callBack?.Invoke();
                return;
            }

            AAMTRuntime.Instance.StartCoroutine(LoadScene(path, sceneMode, callBack));
        }

        private IEnumerator LoadScene(string path, LoadSceneMode sceneMode, Action callBack)
        {
            var sceneName = Tools.FilterSceneName(path);
            yield return SceneManager.LoadSceneAsync(sceneName, sceneMode);
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

            if (!bundles.ContainsKey(abName))
            {
                callBack?.Invoke(default);
                yield break;
            }

            var request = bundles[abName].LoadAssetAsync<T>(itemName);
            yield return request;
            if (request.asset == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0},itemName:{1}", abName, itemName);
                callBack?.Invoke(default);
                yield break;
            }

            callBack?.Invoke(request.asset as T);
        }

        private IEnumerator<AssetBundleRequest> StartGetAllAssets(string path, Action<Object[]> callBack)
        {
            Tools.ParsingLoadUri(path, out var abName, out _, out _);
            if (abName == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0}", abName);
                callBack?.Invoke(default);
                yield break;
            }

            if (!bundles.ContainsKey(abName))
            {
                callBack?.Invoke(default);
                yield break;
            }

            var request = bundles[abName].LoadAllAssetAsync();
            yield return request;
            if (request.allAssets == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0}", abName);
                callBack?.Invoke(default);
                yield break;
            }

            callBack?.Invoke(request.allAssets);
        }

        public void Release(string path)
        {
            var abName = CheckAndGetAbName(path);
            if (!string.IsNullOrEmpty(abName))
            {
                var ab = bundles[abName];
                ab.Release();
                if (ab.ReferenceCount <= 0) RemoveBundleByBundleName(abName);
            }
        }

        public void Destroy(string path)
        {
            var abName = CheckAndGetAbName(path);
            if (!string.IsNullOrEmpty(abName))
            {
                bundles[abName].Destroy();
                RemoveBundleByBundleName(abName);
            }
        }

        private string CheckAndGetAbName(string path)
        {
            Tools.ParsingLoadUri(path, out var abName, out _, out _);
            if (abName == null)
            {
                Debug.LogErrorFormat("释放资源失败，找不到abName,path={0}", path);
                return string.Empty;
            }

            if (bundles.ContainsKey(abName)) return abName;
            Debug.LogErrorFormat("释放资源失败，找不到abName 对应的ab包", path);
            return string.Empty;
        }
    }
}