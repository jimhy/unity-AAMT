using System;
using System.Collections;
using System.Collections.Generic;
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

        public void GetAssets<T>(string path, Action<T> callBack) where T : Object
        {
            if (typeof(T) == typeof(Sprite) || typeof(T) == typeof(AAMTSpriteAtlas))
                _atlasManager.GetAssets(path, callBack);
            else
                AAMTRuntime.Instance.StartCoroutine(StartGetAssets(path, callBack));
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
            yield return 0;

            if (!assetsList.ContainsKey(path))
            {
                callBack?.Invoke(default);
                yield break;
            }

            callBack?.Invoke(assetsList[path] as T);
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