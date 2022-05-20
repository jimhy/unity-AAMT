using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class LocalAssetManager : IResourceManager
    {
        private Dictionary<string, Object> assetsList;
        private readonly SpriteAtlasManager _atlasManager;

        internal LocalAssetManager()
        {
            assetsList = new Dictionary<string, Object>();
            _atlasManager = new SpriteAtlasManagerEditor();
        }

        internal void AddAsset(string path, Object o)
        {
            if (assetsList.ContainsKey(path))
            {
                Debug.LogErrorFormat("重复加载资源,path:{0}", path);
                return;
            }

            assetsList.Add(path, o);
        }

        public void GetAssetsAsync<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(AAMTSpriteAtlas))
                _atlasManager.GetAssetsAsync(path, callBack);
            else
                AAMTRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
        }

        public void GetAllAssetsAsync(string path, Action<Object[]> callBack)
        {
            AAMTRuntime.Instance.StartCoroutine(StartGetAllAssetsAsync(path, callBack));
        }

        private Object[] GetAllAssets(string path)
        {
            var objs = new List<Object>();
            var dirpath = path;
            if (path.LastIndexOf(".") != -1)
            {
                dirpath = Path.GetDirectoryName(path);
            }

            foreach (var keyValuePair in assetsList)
            {
                if (keyValuePair.Key.StartsWith(dirpath))
                {
                    objs.Add(keyValuePair.Value);
                }
            }

            return objs.ToArray();
        }

        public void ChangeScene(string path, Action callBack)
        {
            AAMTRuntime.Instance.StartCoroutine(LoadScene(path, callBack));
        }

        private IEnumerator LoadScene(string path, Action callBack)
        {
            var sceneName = Tools.FilterSceneName(path);
            yield return SceneManager.LoadSceneAsync(sceneName);
            callBack?.Invoke();
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
            yield return 0; //这里是模拟真实ab包加载环境

            if (!assetsList.ContainsKey(path))
            {
                callBack?.Invoke(default);
                yield break;
            }

            callBack?.Invoke(assetsList[path] as T);
        }

        private IEnumerator StartGetAllAssetsAsync(string path, Action<Object[]> callBack)
        {
            yield return 0; //这里是模拟真实ab包加载环境
            var objs = GetAllAssets(path);
            callBack?.Invoke(objs);
        }

        public bool HasAssetsByPath(string path)
        {
            return assetsList.ContainsKey(path);
        }

        public void Release(string path)
        {
            if (!assetsList.ContainsKey(path)) return;
            assetsList.Remove(path);
        }

        public void Destroy(string path)
        {
        }
    }
}