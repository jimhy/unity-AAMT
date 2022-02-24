using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AAMT.Editor
{
    public static class AssetsBundleBuilder
    {
        private static readonly string assetsWidthBundleName = "assetsWidthBundle";


        [MenuItem("UAAM/Build Asset Bundles", false, 51)]
        public static void BuildAssetsBundles()
        {
            EditorCommon.ClearConsole();
            var path = BuildSetting.AssetSetting.GetBuildPath;
            Debug.LogFormat("Start build assets bundles to path:{0}", path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression,
                EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();

            CreateManifestMapFile();
            CreateAssetsListFile();
            Debug.Log("Assets bundle build complete!!");
        }

        /// <summary>
        /// 创建资源列表文件,用于拷贝资源文件到Application.persistentDataPath文件夹里面
        /// </summary>
        public static void CreateAssetsListFile()
        {
            var assetsInfoName = "assetsInfo.txt";
            var txtPath = $"{BuildSetting.AssetSetting.GetBuildPath}/{assetsInfoName}";
            if (File.Exists(txtPath)) File.Delete(txtPath);

            FileStream fs = new FileStream(txtPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            var files = Directory.GetFiles(BuildSetting.AssetSetting.GetBuildPath, "*", SearchOption.AllDirectories);
            int i = 1;
            foreach (var file in files)
            {
                if (!CheckFile(file) || file.IndexOf(assetsInfoName, StringComparison.Ordinal) != -1) continue;
                EditorCommon.UpdateProgress("正在计算文件", i++, files.Length, file);
                FileInfo fileInfo = new FileInfo(file);
                var newPath = file.Replace(BuildSetting.AssetSetting.GetBuildPath, "")
                    .Replace("\\", "/");
                newPath = newPath.Substring(1, newPath.Length - 1);
                newPath = $"{newPath}?{Md5ByFile(file)}";
                sw.WriteLine(newPath);
            }

            sw.Close();
            fs.Close();

            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建资源文件对应的Bundle文件，用于资源加载是，查找资源对于的Bundle文件名
        /// </summary>
        [MenuItem("UAAM/createTem", false, 52)]
        private static void CreateManifestMapFile()
        {
            var files = Directory.GetFiles(BuildSetting.AssetSetting.GetBuildPath, "*.manifest",
                SearchOption.AllDirectories);
            var mapPath = $"{BuildSetting.AssetSetting.GetBuildPath}/{assetsWidthBundleName}.txt";
            if (File.Exists(mapPath)) File.Delete(mapPath);

            FileStream fs = new FileStream(mapPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            var regex = new Regex(@"- Assets/Res/([\w\/\.]+)");
            var buildPath = BuildSetting.AssetSetting.GetBuildPath + "/";
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

        private static bool CheckFile(string file)
        {
            return !(file.EndsWith(".meta") ||
                     file.EndsWith(".apk") ||
                     file.EndsWith(".manifest") ||
                     file.EndsWith(".idea"));
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string Md5ByFile(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                var md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }
    }
}