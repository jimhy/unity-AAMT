using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class ASpriteAtlas : Object
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
            foreach (Sprite sprite in request.allAssets)
            {
                _sprites[sprite.name] = sprite;
            }
        }
    }

    public class SpriteAtlasManager
    {
        private BundleManager _manager;
        private Dictionary<string, ASpriteAtlas> _atlasMap = new Dictionary<string, ASpriteAtlas>();

        public SpriteAtlasManager(BundleManager manager)
        {
            _manager = manager;
        }

        public void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite))
                AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
            else if (typeof(T) == typeof(ASpriteAtlas))
                AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssets2(path, callBack));
        }

        private IEnumerator<AssetBundleRequest> StartGetAssets2<T>(string path, Action<T> callBack) where T : Object
        {
            path = path.ToLower();
            if (!_manager.PathToBundle.ContainsKey(path))
            {
                Debug.LogErrorFormat("获取资源时，找不到对应的ab包。path:{0}", path);
                callBack?.Invoke(default);
                yield break;
            }

            var abName = _manager.PathToBundle[path];
            if (!_manager.Bundles.ContainsKey(abName))
            {
                callBack?.Invoke(default);
                yield break;
            }

            var atlasName = path;
            var n = path.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                atlasName = path.Substring(n + 1);
            }

            ASpriteAtlas atl;
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

            atl = new ASpriteAtlas();
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
            path = path.ToLower();
            var n = path.LastIndexOf("?", StringComparison.Ordinal);
            if (n == -1)
            {
                Debug.LogErrorFormat("获取Sprite资源格式错误");
                callBack?.Invoke(default);
                yield break;
            }

            var spriteName = path[(n + 1)..];
            path = path[..n];
            if (!_manager.PathToBundle.ContainsKey(path))
            {
                Debug.LogErrorFormat("获取资源时，找不到对应的ab包。path:{0}", path);
                callBack?.Invoke(default);
                yield break;
            }

            var abName = _manager.PathToBundle[path];
            if (!_manager.Bundles.ContainsKey(abName))
            {
                callBack?.Invoke(default);
                yield break;
            }

            var atlasName = path;
            n = path.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                atlasName = path[(n + 1)..];
            }

            ASpriteAtlas atl;
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
            atl = new ASpriteAtlas();
            _atlasMap.Add(atlasName, atl);
            atl.Add(request);
            result = atl.GetSprite(spriteName);
            if (result != null) callBack?.Invoke(result as T);
            else callBack?.Invoke(default);
            yield return request;
        }
    }
}