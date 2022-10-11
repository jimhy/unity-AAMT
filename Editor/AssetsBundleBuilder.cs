using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAMT
{
    public static class AssetsBundleBuilder
    {
        [MenuItem("AAMT/Build Asset Bundles", false, 51)]
        public static void BuildAssetsBundles()
        {
            EditorCommon.ClearConsole();
            var path = SettingManager.assetSetting.getBuildPath.ToLower();
            Debug.LogFormat("Start build assets bundles to path:{0}", path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var abs  = new List<AssetBundleBuild> { GetSettingManagerAbName() };
            GetAssetBundleBuilds(abs);
            BuildPipeline.BuildAssetBundles(path, abs.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorCommon.AamtToEditorTarget());

            CreateManifestMapFile();
            CreateVersionFile();
            AssetDatabase.Refresh();
            Debug.Log("Assets bundle build complete!!");
        }

        private static AssetBundlePackageData _assetBundlePackageData;

        private static void GetAssetBundleBuilds(List<AssetBundleBuild> abs)
        {
            _assetBundlePackageData = new AssetBundlePackageData();
            if (_assetBundlePackageData != null)
                foreach (var pd in _assetBundlePackageData._guids)
                {
                    if (pd.AbType == WindowDefine.ABType.PACKAGE)
                    {
                        var builds = GetDicFiles(pd.Path);
                        if (builds != null && builds.Count != 0)
                        {
                            var abb = new AssetBundleBuild();
                            abb.assetBundleName = pd.Path.ToLower().Replace("assets/", "") + ".ab";
                            abb.assetNames      = builds.ToArray();
                            abs.Add(abb);
                        }
                    }
                    else if (pd.AbType == WindowDefine.ABType.SINGLE)
                    {
                        var builds = GetDicFiles(pd.Path);
                        if (builds != null && builds.Count != 0)
                        {
                            foreach (var build in builds)
                            {
                                var abb = new AssetBundleBuild();
                                abb.assetBundleName = build.Replace("assets/", "") + ".ab";
                                abb.assetNames      = new[] { build };
                                abs.Add(abb);
                            }
                        }
                    }
                }
        }

        private static List<string> GetDicFiles(string pdPath)
        {
            var extension = Path.GetExtension(pdPath);
            if (!string.IsNullOrEmpty(extension)) return null;
            var list        = new List<string>();
            var files       = Directory.GetFiles(pdPath, "*", SearchOption.TopDirectoryOnly);
            var directories = Directory.GetDirectories(pdPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                var file = f.ToLower();
                if (!EditorCommon.CheckPath(file)) continue;
                list.Add(file);
            }

            foreach (var d in directories)
            {
                var directory = d.ToLower();
                var guidData  = _assetBundlePackageData.GetData(directory);
                if (guidData == null || guidData.AbType != WindowDefine.ABType.PARENT) continue;
                var fs = GetDicFiles(directory);
                if (fs != null && fs.Count != 0) list.AddRange(fs);
            }

            return list;
        }

        private static AssetBundleBuild GetSettingManagerAbName()
        {
            var abb = new AssetBundleBuild();
            abb.assetBundleName = AAMTDefine.AAMT_BUNDLE_NAME;

            var filePath         = $"{Application.dataPath}/AAMT/Data";
            var files            = Directory.GetFiles(filePath, "*asset", SearchOption.AllDirectories);
            var assetBundleNames = new List<string>();
            var pn               = AAMTDefine.AAMT_BUNDLE_PACKAGE_DATA.ToLower();
            foreach (var path in files)
            {
                var p = path.Replace(Application.dataPath, "assets").Replace("\\", "/").ToLower();
                if (p == pn) continue;
                assetBundleNames.Add(p);
            }

            abb.assetNames = assetBundleNames.ToArray();
            return abb;
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
            var files = Directory.GetFiles(SettingManager.assetSetting.getBuildPath, "*.manifest", SearchOption.AllDirectories);

            var mapPath = Path.Combine(SettingManager.assetSetting.getBuildPath, AAMTDefine.AAMT_ASSETS_WITH_BUNDLE_NAME);
            if (File.Exists(mapPath)) File.Delete(mapPath);

            FileStream   fs        = new FileStream(mapPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw        = new StreamWriter(fs);
            var          regex     = new Regex(@"- Assets/([\w\/\.]+)");
            var          buildPath = $"{SettingManager.assetSetting.getBuildPath}/";
            var          i         = 1;
            foreach (var file in files)
            {
                EditorCommon.UpdateProgress("正在计算文件", i++, files.Length, file);
                var f      = file.Replace("\\", "/");
                var source = File.ReadAllText(f);
                var ary    = regex.Matches(source);
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