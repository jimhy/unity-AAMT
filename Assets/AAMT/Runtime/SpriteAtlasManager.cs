using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class AAMTSpriteAtlas : Object
    {
        private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

        public Sprite GetSprite(string spriteName)
        {
            if (_sprites.ContainsKey(spriteName)) return _sprites[spriteName];
            return null;
        }

        public bool HasSprite(string spriteName)
        {
            return _sprites.ContainsKey(spriteName);
        }

        public Sprite[] GetSprites()
        {
            var sprites = new Sprite[_sprites.Count];
            var i = 0;
            foreach (var key in _sprites)
            {
                sprites[i++] = key.Value;
            }

            return sprites;
        }

        internal void Add(AssetBundleRequest request)
        {
            foreach (var o in request.allAssets)
            {
                var sprite = o as Sprite;
                if (sprite != null) _sprites[sprite.name] = sprite;
            }
        }
    }

    public class SpriteAtlasManager
    {
        private BundleManager _manager;
        private Dictionary<string, AAMTSpriteAtlas> _atlasMap = new Dictionary<string, AAMTSpriteAtlas>();

        public SpriteAtlasManager(BundleManager manager)
        {
            _manager = manager;
        }

        public void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite))
                AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
            else if (typeof(T) == typeof(AAMTSpriteAtlas))
                AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssets2(path, callBack));
        }

        private IEnumerator<AssetBundleRequest> StartGetAssets2<T>(string path, Action<T> callBack) where T : Object
        {
            Tools.ParsingLoadUri(path, out var abName, out var atlasName, out _);
            if (abName == null || atlasName == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0},atlasName:{1}", abName, atlasName);
                callBack?.Invoke(default);
                yield break;
            }

            AAMTSpriteAtlas atl;
            if (_atlasMap.ContainsKey(atlasName))
            {
                atl = _atlasMap[atlasName];
                callBack?.Invoke(atl as T);
                yield break;
            }

            var request = _manager.Bundles[abName].LoadAssetWithSubAssetsAsync<Sprite>(atlasName);
            yield return request;
            if (request.allAssets.Length == 0)
            {
                callBack?.Invoke(default);
                yield break;
            }

            atl = new AAMTSpriteAtlas();
            _atlasMap.Add(atlasName, atl);
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
        private IEnumerator<AssetBundleRequest> StartGetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            Tools.ParsingLoadUri(path, out var abName, out var atlasName, out string spriteName);
            if (abName == null || atlasName == null || spriteName == null)
            {
                Debug.LogErrorFormat("加载资源失败,abName:{0},atlasName:{1},spriteName:{2}", abName, atlasName, spriteName);
                callBack?.Invoke(default);
                yield break;
            }

            AAMTSpriteAtlas atl;
            if (_atlasMap.ContainsKey(atlasName))
            {
                atl = _atlasMap[atlasName];
                if (!atl.HasSprite(spriteName)) callBack?.Invoke(_atlasMap[atlasName].GetSprite(spriteName) as T);
                else callBack?.Invoke(default);
                yield break;
            }

            var request = _manager.Bundles[abName].LoadAssetWithSubAssetsAsync<T>(atlasName);
            yield return request;
            if (request.allAssets.Length == 0)
            {
                callBack?.Invoke(default);
                yield break;
            }

            Sprite result = default;
            atl = new AAMTSpriteAtlas();
            _atlasMap.Add(atlasName, atl);
            atl.Add(request);
            result = atl.GetSprite(spriteName);

            if (result != null) callBack?.Invoke(result as T);
            else callBack?.Invoke(default);
            yield return request;
        }
    }
}