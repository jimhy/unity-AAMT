using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

namespace JAssetsManager
{
    public class AssetsBundleBuilder
    {
        public static bool buildHalfPackage = true;
        public static bool debugBuild = true;
        public static bool development = true;
        public static bool allowDebugging = true;
        public static bool strictMode = true;
        public static bool connectWithProfiler = false;
        public static bool isFullApk = false;
        public static bool EnableDeepProfilingSupport = false;
        public static bool WaitForPlayerConnection = false;

        // [MenuItem("Build/Build All Resource", false, 2)]
        // public static void buildAllResource(BuildPanel.EBuildType buildType)
        // {
        //     copyXmlFiles();
        //
        //     if (buildType == BuildPanel.EBuildType.AutoBuild)
        //     {
        //         if (Packager.IsResState)
        //         {
        //             buildAssetsBundles();
        //             Packager.SaveResMd5();
        //         }
        //     }else
        //     {
        //         buildAssetsBundles();
        //     }
        //
        //     if (buildType == BuildPanel.EBuildType.AutoBuild)
        //     {
        //         if (Packager.IsLuaState)
        //         {
        //             Packager.startToBuildLua();
        //             Packager.SaveLuaMd5();
        //         }
        //     }else
        //     {
        //         Packager.startToBuildLua();
        //     }
        //     ABNameMapCreater.creatUIABMap();
        //     createAssetsListFile();
        //     AssetDatabase.Refresh();
        // }


        [MenuItem("Build/Build Asset Bundles", false, 51)]
        public static void buildAssetsBundles()
        {
            var path = BuildSetting.AssetSetting.GetBuildPath;
            Debug.LogFormat("Start build assets bundles to path:{0}", path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression,
                EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();

            excuseOther();
        }

        private static void excuseOther()
        {
            var abBundle = AssetBundle.LoadFromFile($"{BuildSetting.AssetSetting.GetBuildPath}/{BuildSetting.AssetSetting.GetBuildTag}");

            AssetDatabase.Refresh();

            abBundle.Unload(true);
            createAssetsListFile();
            Debug.Log("Assets bundle build complete!!");
        }

        [MenuItem("Build/Build Asset Bundles To StreamingAssets", false, 102)]
        public static void copyAllResourceFiles()
        {
            //var targetPath = ReadConfig.readConfig("editorTestPath") + "Assets/StreamingAssets/";
            var targetPath = Application.streamingAssetsPath;
            if (targetPath == null)
            {
                Debug.LogError("config.txt文件找不到editorTestPath路径");
                return;
            }

            var sourcePath = BuildSetting.AssetSetting.GetBuildPath;
            if (!Directory.Exists(sourcePath)) Directory.CreateDirectory(sourcePath);
            if (Directory.Exists(targetPath))
            {
                Directory.Delete(targetPath, true);
            }

            var files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            var index = 1;
            var totalNum = files.Length;
            foreach (var file in files)
            {
                FileInfo info = new FileInfo(file);
                index++;
                if (file.EndsWith(".meta")) continue;
                if (file.EndsWith(".manifest") && info.Name != "ab.manifest") continue;
                var dest = file.Replace(sourcePath, targetPath);
                var dir = Path.GetDirectoryName(dest);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                EditorCommon.UpdateProgress("正在复制文件", index, totalNum, dir);
                File.Copy(file, dest, true);
            }

            EditorUtility.ClearProgressBar();
            Debug.Log("Copy AssetsBundle Files Finished!!");
            AssetDatabase.Refresh();
        }

        public static void createAssetsListFile()
        {
            var txtPath = BuildSetting.AssetSetting.GetBuildPath + "/assetsInfo.txt";
            if (File.Exists(txtPath)) File.Delete(txtPath);

            FileStream fs = new FileStream(txtPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            var files = Directory.GetFiles(BuildSetting.AssetSetting.GetBuildPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith(".meta") || file.IndexOf("assetsInfo") != -1 || file.IndexOf("apk") != -1 ||
                    file.IndexOf(".idea") != -1) continue;
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension == ".manifest" && fileInfo.Name.Trim() != "ab.manifest") continue;
                var newPath = file.Replace(BuildSetting.AssetSetting.GetBuildPath, "").Replace("\\", "/");
                sw.WriteLine(newPath);
            }

            sw.Close();
            fs.Close();

            Debug.Log("创建文件列表完成!!");
            AssetDatabase.Refresh();
        }

        public static void createAssetsListFileToStreamingAsset()
        {
            var txtPath = Application.streamingAssetsPath + "/assetsInfo.txt";
            if (File.Exists(txtPath)) File.Delete(txtPath);

            FileStream fs = new FileStream(txtPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            var files = Directory.GetFiles(Application.streamingAssetsPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith(".meta") || file.IndexOf("assetsInfo") != -1 || file.IndexOf("apk") != -1 ||
                    file.IndexOf(".idea") != -1) continue;
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension == ".manifest" && fileInfo.Name.Trim() != "ab.manifest") continue;
                var newPath = file.Replace(Application.streamingAssetsPath, "").Replace("\\", "/");
                sw.WriteLine(newPath);
            }

            sw.Close();
            fs.Close();

            Debug.Log("创建文件列表完成!!");
            AssetDatabase.Refresh();
        }

        public static void createStreamingAssetsListFile()
        {
            var txtPath = BuildSetting.AssetSetting.GetBuildPath + "/assetsInfo.txt";
            if (File.Exists(txtPath)) File.Delete(txtPath);

            FileStream fs = new FileStream(txtPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            var files = Directory.GetFiles(BuildSetting.AssetSetting.GetBuildPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith(".meta") || file.IndexOf("assetsInfo") != -1 || file.IndexOf("apk") != -1 ||
                    file.IndexOf(".idea") != -1) continue;
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension == ".manifest" && fileInfo.Name.Trim() != "ab.manifest") continue;
                var newPath = file.Replace(BuildSetting.AssetSetting.GetBuildPath, "").Replace("\\", "/");
                sw.WriteLine(newPath);
            }

            sw.Close();
            fs.Close();

            Debug.Log("创建文件列表完成!!");
            AssetDatabase.Refresh();
        }
    }
}