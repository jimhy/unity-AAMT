using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT
{
    public class BundleManager
    {
        internal AssetBundleManifest assetBundleManifest { get; private set; }
        internal Dictionary<string, string> pathToBundle { get; private set; }
        internal Dictionary<string, AssetBundle> bundles { get; private set; }

        public BundleManager()
        {
            bundles = new Dictionary<string, AssetBundle>();
            InitManifest();
            InitBundleMap();
        }

        private void InitManifest()
        {
            var mainBundle =
                AssetBundle.LoadFromFile(
                    $"{BuildSetting.AssetSetting.GetLoadPath}/{BuildSetting.AssetSetting.GetBuildTargetToString}");
            if (mainBundle != null)
            {
                assetBundleManifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            else
                Debug.LogError("mian ab资源加载错误");

            mainBundle.Unload(false);
        }

        private void InitBundleMap()
        {
            pathToBundle = new Dictionary<string, string>();
            var fileName = "assets-width-bundle";
            var path = $"{BuildSetting.AssetSetting.GetLoadPath}/{fileName}.txt";
            var content = ReadTextFileData(path);
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError("assets-width-bundle 资源加载错误");
                return;
            }

            var strAry = content.Replace("\r", "").Split("\n");
            foreach (var s in strAry)
            {
                if (string.IsNullOrEmpty(s)) continue;
                var ary = s.Split(",");
                pathToBundle[ary[0]] = ary[1];
            }
        }

        internal string ReadTextFileData(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            if (null == sr) return string.Empty;
            var str = sr.ReadToEnd();
            sr.Close();
            return str;
        }

        internal void AddBundle(AssetBundle ab)
        {
            // Debug.LogFormat("AddBundle:{0}", ab.name);
            if (!bundles.ContainsKey(ab.name)) bundles.Add(ab.name, ab);
            else Debug.LogErrorFormat("重复添加ab包，应该是出现了重复加载，会出现双份内存的情况，请检查。path:{0}", ab.name);
        }

        internal bool HasBundleByBundleName(string abName)
        {
            return bundles.ContainsKey(abName);
        }

        internal bool HasBundleByAssetsPath(string assetPath)
        {
            if (!pathToBundle.ContainsKey(assetPath))
            {
                Debug.LogFormat("找不到对应的ab包。assetPath:{0}", assetPath);
                return false;
            }

            var abName = pathToBundle[assetPath];
            return bundles.ContainsKey(abName);
        }

        public T GetAssets<T>(string path) where T : Object
        {
            path = path.ToLower();
            if (!pathToBundle.ContainsKey(path))
            {
                Debug.LogFormat("获取资源时，找不到对应的ab包。path:{0}", path);
                return default;
            }

            var abName = pathToBundle[path];
            if (!bundles.ContainsKey(abName)) return default;
            var itemName = path;
            var n = path.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                itemName = path.Substring(n + 1);
            }

            return bundles[abName].LoadAsset<T>(itemName);
        }

        public T GetAssets<T>(string abName, string itemName) where T : Object
        {
            abName = abName.ToLower();
            itemName = itemName.ToLower();
            if (!bundles.ContainsKey(abName)) return default;

            return bundles[abName].LoadAsset<T>(itemName);
        }
    }
}