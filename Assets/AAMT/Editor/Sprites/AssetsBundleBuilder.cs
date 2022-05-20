using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    public static class AssetsBundleBuilder
    {
        [MenuItem("AAMT/Build Asset Bundles", false, 51)]
        public static void BuildAssetsBundles()
        {
            EditorCommon.ClearConsole();
            SetSettingManagerAbName();
            var path = SettingManager.assetSetting.getBuildPath.ToLower();
            Debug.LogFormat("Start build assets bundles to path:{0}", path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression,
                EditorCommon.AamtToEditorTarget());

            CreateManifestMapFile();
            CreateVersionFile();
            AssetDatabase.Refresh();
            Debug.Log("Assets bundle build complete!!");
        }

        private static void SetSettingManagerAbName()
        {
            var filePath = $"{Application.dataPath}/AAMT/Data";
            var files = Directory.GetFiles(filePath, "*asset", SearchOption.AllDirectories);
            foreach (var path in files)
            {
                var p = path.Replace(Application.dataPath, "assets");
                AssetImporter item = AssetImporter.GetAtPath(p);
                item.assetBundleName = AAMTDefine.AAMT_BUNDLE_NAME;
            }
        }


        /// <summary>
        /// 创建资源列表文件,用于拷贝资源文件到Application.persistentDataPath文件夹里面
        /// </summary>
        [MenuItem("AAMT/CreateVersionFile", false, 54)]
        public static void CreateVersionFile()
        {
            var dirPath = SettingManager.assetSetting.getBuildPath;
            var txtPath = $"{dirPath}/{AAMTDefine.AAMT_ASSET_VERSION}";
            EditorCommon.CreateVersionFile(txtPath, dirPath);
        }

        /// <summary>
        /// 创建资源文件对应的Bundle文件，用于资源加载时，查找资源对应的Bundle文件名
        /// </summary>
        [MenuItem("AAMT/CreateManifestMapFile", false, 55)]
        private static void CreateManifestMapFile()
        {
            var files = Directory.GetFiles(SettingManager.assetSetting.getBuildPath, "*.manifest",
                SearchOption.AllDirectories);

            var mapPath = Path.Combine(SettingManager.assetSetting.getBuildPath,
                AAMTDefine.AAMT_ASSETS_WIDTH_BUNDLE_NAME);
            if (File.Exists(mapPath)) File.Delete(mapPath);

            FileStream fs = new FileStream(mapPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            var regex = new Regex(@"- Assets/([\w\/\.]+)");
            var buildPath = $"{SettingManager.assetSetting.getBuildPath}/";
            var i = 1;
            foreach (var file in files)
            {
                EditorCommon.UpdateProgress("正在计算文件", i++, files.Length, file);
                var f = file.Replace("\\", "/");
                var source = File.ReadAllText(f);
                var ary = regex.Matches(source);
                var abName = f.Replace(buildPath, "").Replace(".manifest", "");
                foreach (Match o in ary)
                {
                    var fn = o.Groups[1].Value.ToLower();
                    sw.WriteLine($"{fn},{abName}");
                }
            }

            EditorUtility.ClearProgressBar();
            sw.Close();
            fs.Close();
        }

        [MenuItem("AAMT/Remove Bundle Cache", false, 52)]
        private static void RemoveBundleCache()
        {
            var path = SettingManager.assetSetting.getBuildPath;
            Debug.LogFormat("path={0}", path);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                File.Delete($"{path}.meta");
                Debug.LogFormat("Remove bundle cache at path:{0}", path);
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("AAMT/Remove PersistentDataPath Cache", false, 53)]
        private static void RemovePersistentDataPathCache()
        {
            var path = Application.persistentDataPath;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Debug.LogFormat("Remove PersistentDataPath at path:{0}", path);
            }
        }
    }
}