using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class SpriteAtlasManager
    {
        protected BundleManager manager;
        protected Dictionary<string, AAMTSpriteAtlas> atlasMap;

        internal SpriteAtlasManager(BundleManager manager)
        {
            this.manager = manager;
            atlasMap = new Dictionary<string, AAMTSpriteAtlas>();
        }

        internal virtual void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite))
                StartGetAssetsSprite(path, callBack);
            else if (typeof(T) == typeof(AAMTSpriteAtlas))
                AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssetsAtlas(path, callBack));
        }

        protected virtual IEnumerator StartGetAssetsAtlas<T>(string path, Action<T> callBack) where T : Object
        {
            Tools.ParsingLoadUri(path, out var abName, out var atlasName, out _);
            if (string.IsNullOrEmpty(abName) || string.IsNullOrEmpty(atlasName))
            {
                Debug.LogErrorFormat("加载资源失败,参数错误,abName:{0},atlasName:{1}", abName, atlasName);
                callBack?.Invoke(default);
                yield break;
            }

            AAMTSpriteAtlas atl;
            if (atlasMap.ContainsKey(atlasName))
            {
                atl = atlasMap[atlasName];
                callBack?.Invoke(atl as T);
                yield break;
            }

            var request = manager.Bundles[abName].LoadAssetWithSubAssetsAsync<Sprite>(atlasName);
            yield return request;
            if (request.allAssets.Length == 0)
            {
                callBack?.Invoke(default);
                yield break;
            }

            atl = new AAMTSpriteAtlas();
            atlasMap.Add(atlasName, atl);
            atl.Add(request);
            callBack?.Invoke(atl as T);
        }

        /// <summary>
        /// 加载Sprite
        /// </summary>
        /// <param name="path">加载路径(ui/imgs/atlas.png?spritename)</param>
        /// <param name="callBack">加载完成回调</param>
        /// <typeparam name="T">加载的类型，必须要Sprite类型</typeparam>
        /// <returns></returns>
        protected virtual void StartGetAssetsSprite<T>(string path, Action<T> callBack) where T : Object
        {
            Tools.ParsingLoadUri(path, out var abName, out var atlasName, out string spriteName);
            if (string.IsNullOrEmpty(abName) || string.IsNullOrEmpty(atlasName) || string.IsNullOrEmpty(spriteName))
            {
                Debug.LogErrorFormat("加载资源失败,参数错误,abName:{0},atlasName:{1},spriteName:{2}", abName, atlasName,
                    spriteName);
                callBack?.Invoke(default);
                return;
            }

            AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssetsAtlas<AAMTSpriteAtlas>(path, (atl) =>
            {
                var result = atl.GetSprite(spriteName);

                if (result != null) callBack?.Invoke(result as T);
                else callBack?.Invoke(default);
            }));
        }
    }
}