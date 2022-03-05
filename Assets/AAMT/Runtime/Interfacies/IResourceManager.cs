using System;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AAMT
{
    interface IResourceManager
    {
        bool HasAssetsByPath(string path);
        void GetAssets<T>(string path, Action<T> callBack) where T : Object;
        void ChangeScene(string path,[CanBeNull] Action callBack);
        void Release(string path);
        void Destroy(string path);
    }
}