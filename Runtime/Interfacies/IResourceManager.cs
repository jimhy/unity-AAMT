using System;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AAMT
{
    interface IResourceManager
    {
        bool HasAssetsByPath(string path);
        void GetAssetsAsync<T>(string path, Action<T> callBack) where T : Object;
        void GetAllAssetsAsync(string path, Action<Object[]> callBack);
        void ChangeScene(string path, LoadSceneMode sceneMode, [CanBeNull] Action callBack);
        void Release(string path);
        void Destroy(string path);
    }
}