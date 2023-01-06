using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace AAMT
{
    public static class Tools
    {
        internal static string FilterSpriteUri(string input)
        {
            return Regex.Replace(input, @"\?.+", "");
        }

        internal static string FilterSceneName(string input)
        {
            var n = input.LastIndexOf(".unity", StringComparison.Ordinal);
            if (n == -1)
            {
                return input;
            }

            n = input.LastIndexOf(".", StringComparison.Ordinal);
            var n1 = input.LastIndexOf("/", StringComparison.Ordinal);
            if (n1 == -1) n1 = 0;
            else n1++;

            return input.Substring(n1, n - n1);
        }

        internal static void ParsingLoadUri(string input, out string abName, out string itemName, out string spriteName)
        {
            ParsingLoadUri(input, out _, out abName, out itemName, out spriteName);
        }

        internal static void ParsingLoadUri(string     input, out string uri, out string abName, out string itemName,
                                            out string spriteName)
        {
            if (AAMTManager.Instance.resourceManager is LocalAssetManager)
            {
                ForEditor(input, out uri, out abName, out itemName, out spriteName);
            }
            else
            {
                ForBundle(input, out uri, out abName, out itemName, out spriteName);
            }
        }

        private static void ForBundle(string     input, out string uri, out string abName, out string itemName,
                                      out string spriteName)
        {
            var bundleManager = AAMTManager.Instance.resourceManager as BundleManager;
            input      = input.ToLower();
            uri        = input;
            abName     = null;
            spriteName = null;
            itemName   = null;
            if (bundleManager == null) return;
            var n = input.LastIndexOf("?", StringComparison.Ordinal);
            if (n != -1)
            {
                spriteName = input[(n + 1)..];
                uri        = input[..n];
            }

            if (!bundleManager.pathToBundle.ContainsKey(uri))
            {
                Debug.LogErrorFormat("获取资源时，找不到对应的ab包。path:{0}", uri);
                return;
            }

            abName = bundleManager.pathToBundle[uri];
            n      = uri.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                itemName = uri[(n + 1)..];
            }
        }

        private static void ForEditor(string     input, out string uri, out string abName, out string itemName,
                                      out string spriteName)
        {
            input      = input.ToLower();
            uri        = input;
            abName     = null;
            spriteName = null;
            itemName   = null;

            var n = input.LastIndexOf("?", StringComparison.Ordinal);
            if (n != -1)
            {
                spriteName = input[(n + 1)..];
                uri        = input[..n];
            }

            n = uri.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                itemName = uri[(n + 1)..];
            }
        }

        public static string ReadTextFileData(string path)
        {
            if (Application.platform                               == RuntimePlatform.Android &&
                path.IndexOf("file:///", StringComparison.Ordinal) == -1)
            {
                path = $"file:///{path}";
            }

            var request = UnityWebRequest.Get(path);
            request.SendWebRequest();
            while (!request.isDone)
            {
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogErrorFormat("ReadTextFileData error,errorCode:{0},path:{1}", request.result, path);
                return string.Empty;
            }

            return request.downloadHandler.text;
        }

        internal static AssetSetting.BuildTarget PlatformToBuildTarget()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return AssetSetting.BuildTarget.android;
                case RuntimePlatform.IPhonePlayer:
                    return AssetSetting.BuildTarget.ios;
                case RuntimePlatform.WindowsPlayer:
                    return AssetSetting.BuildTarget.windows;
            }

            return AssetSetting.BuildTarget.editor;
        }

        public static AssetBundle LoadBundle(string path)
        {
            if (path.ToLower().IndexOf("streamingassets", StringComparison.Ordinal) != -1)
                return LoadBundleByWebRequest(path);
            else
                return AssetBundle.LoadFromFile(path);
        }

        public static void LoadBundleAsync(string path, Action<AssetBundle> cb)
        {
            if (path.ToLower().IndexOf("streamingassets", StringComparison.Ordinal) != -1)
                AAMTRuntime.Instance.StartCoroutine(LoadBundleByWebRequestAsync(path, cb));
            else
                AAMTRuntime.Instance.StartCoroutine(LoadBundleFromeFile(path, cb));
        }

        private static IEnumerator LoadBundleFromeFile(string path, Action<AssetBundle> cb)
        {
            Debug.LogFormat("LoadBundleFromeFile:{0}", path);
            var r = AssetBundle.LoadFromFileAsync(path);
            yield return r;
            Debug.LogFormat("Load Bundle Success!! path:{0}", path);
            cb?.Invoke(r.assetBundle);
        }

        private static IEnumerator LoadBundleByWebRequestAsync(string path, Action<AssetBundle> cb)
        {
            Debug.LogFormat("LoadBundleByWebRequestAsync:{0}", path);
            var request   = UnityWebRequest.Get(path);
            var operation = request.SendWebRequest();
            yield return operation;
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogErrorFormat(request.error);
                cb?.Invoke(null);
                yield break;
            }

            var bundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
            Debug.LogFormat("Load Bundle Success!! path:{0}", path);
            cb?.Invoke(bundle);
        }


        public static AssetBundle LoadBundleByWebRequest(string path)
        {
            Debug.LogFormat("LoadBundleByWebRequest:{0}", path);
            var request = UnityWebRequest.Get(path);
            request.SendWebRequest();
            while (!request.isDone)
            {
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogErrorFormat(request.error);
                return null;
            }

            var bundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
            Debug.LogFormat("Load Bundle Success!! path:{0}", path);
            return bundle;
        }
    }
}