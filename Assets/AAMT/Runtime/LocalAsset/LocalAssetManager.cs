using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class LocalAssetManager : IResourceManager
    {
        internal Dictionary<string, Object> Assets { get; }
        private readonly SpriteAtlasManager _atlasManager;

        internal LocalAssetManager()
        {
            Assets = new Dictionary<string, Object>();
            _atlasManager = new SpriteAtlasManagerEditor();
        }

        internal void AddAsset(string path, Object o)
        {
            if (Assets.ContainsKey(path))
            {
                Debug.LogErrorFormat("重复加载资源,path:{0}", path);
                return;
            }

            Assets.Add(path, o);
        }

        public void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(AAMTSpriteAtlas))
                _atlasManager.GetAssets(path, callBack);
            else
                AssetsManagerRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
        }

        /// <summary>
        /// 这里是模拟真实ab包加载环境
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEnumerator StartGetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            yield return 0;

            if (!Assets.ContainsKey(path))
            {
                callBack?.Invoke(default);
                yield break;
            }

            callBack?.Invoke(Assets[path] as T);
        }

        public bool HasAssetsByPath(string path)
        {
            return Assets.ContainsKey(path);
        }

        public void Release(string path)
        {
            if (!Assets.ContainsKey(path)) return;
            Assets.Remove(path);
        }

        public void Destroy(string path)
        {
        }
    }
}