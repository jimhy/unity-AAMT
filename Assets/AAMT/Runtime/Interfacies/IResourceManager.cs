using System;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AAMT
{
    interface IResourceManager
    {
        bool HasAssetsByPath(string path);
        void GetAssets<T>(string path, Action<T> callBack) where T : Object;
        void Release(string path);
        void Destroy(string path);
    }
}