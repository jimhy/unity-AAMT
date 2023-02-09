using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AAMT.Editor
{
    public class OnPreprocessHandler : AssetPostprocessor, IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }

        /// <summary>
        /// 打包前处理
        /// </summary>
        /// <param name="report"></param>
        public void OnPreprocessBuild(BuildReport report)
        {
            var settting = SettingManager.assetSetting;
            if (settting.BuildPlatform == AssetSetting.BuildTarget.editor)
            {
                EditorUtility.DisplayDialog("温馨提示", "当前选择的BuildTarget为Editor,会自动设置为默认选择的平台。", "继续");
            }

            MoveAAMTBundles();
            var ok = EditorUtility.DisplayDialog("温馨提示", "打包之前需要把指定的资源移动到StreamAssets文件夹下，是否需要移动。", "移动", "不需要");
            if (!ok) return;
            if (Directory.Exists(Application.streamingAssetsPath))
                Directory.Delete(Application.streamingAssetsPath, true);
            
            AssetDatabase.Refresh();
            MoveAAMTBundles();
            settting.SetBuildTargetForBulidPlayer(EditorCommon.EditorToAamtTarget());
            if (settting.CurrentLoadType == AssetSetting.LoadType.Local)
            {
                MoveAllBundleToStreamingAssets();
                return;
            }

            MoveBundleToStreamingAssets();
            CreateBundleFilesDictionary();
            CreateStreamingAssetsVersionData();
        }

        /// <summary>
        /// 打包后处理
        /// </summary>
        /// <param name="report"></param>
        public void OnPostprocessBuild(BuildReport report)
        {
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                if (path.EndsWith(".asset") && path.IndexOf("AAMT/Data", StringComparison.Ordinal) != -1)
                {
                    SettingManager.ReloadAssetSetting();
                }
            }
        }

        public static void MoveBundleToStreamingAssets()
        {
            var targetPahtPre = $"{Application.streamingAssetsPath}/{SettingManager.assetSetting.BuildPlatform}".ToLower();
            var list          = GetMoveToStreamingAssetsPathList();
            var i             = 0;
            foreach (var s in list)
            {
                var sourcePath = $"{SettingManager.assetSetting.BuildPath}/{s}".ToLower();
                var targetPath = $"{targetPahtPre}/{s}".ToLower();
                EditorCommon.UpdateProgress("正在移动文件", ++i, list.Length, sourcePath);
                var targetDic = Path.GetDirectoryName(targetPath);
                if (!Directory.Exists(targetDic)) Directory.CreateDirectory(targetDic);
                if (File.Exists(targetPath)) File.Delete(targetPath);
                File.Copy(sourcePath, targetPath);
            }

            EditorCommon.ClearProgressBar();
        }

        private static void MoveAAMTBundles()
        {
            var fileNames     = new[] { $"{AAMTDefine.AAMT_BUNDLE_NAME}", $"{AAMTDefine.AAMT_BUNDLE_NAME}.manifest" };
            var targetPahtPre = $"{Application.streamingAssetsPath}/{SettingManager.assetSetting.BuildPlatform}".ToLower();
            if (!Directory.Exists(targetPahtPre))
                Directory.CreateDirectory(targetPahtPre);

            foreach (var fileName in fileNames)
            {
                var buildPath  = $"{SettingManager.assetSetting.BuildPath}/{fileName}".ToLower();
                var targetPath = $"{targetPahtPre}/{fileName}".ToLower();
                if (File.Exists(targetPath)) File.Delete(targetPath);
                File.Copy(buildPath, targetPath);
            }

            AssetDatabase.Refresh();
        }

        public static string[] GetMoveToStreamingAssetsPathList()
        {
            var abPath   = new List<string>();
            var list     = SettingManager.assetSetting.MoveToStreamingAssetsPathList;
            var fileList = new List<string>();
            foreach (var s in list)
            {
                var fs = Directory.GetFiles(s, "*", SearchOption.AllDirectories);
                fileList.AddRange(fs);
            }

            foreach (var s in fileList)
            {
                var item = AssetImporter.GetAtPath(s);
                if (item != null && !string.IsNullOrEmpty(item.assetBundleName))
                {
                    if (abPath.IndexOf(item.assetBundleName) == -1) abPath.Add(item.assetBundleName);
                }
            }

            return abPath.ToArray();
        }

        public static void MoveAllBundleToStreamingAssets()
        {
            var sourcePath = $"{SettingManager.assetSetting.BuildPath}".ToLower();
            var targetPath = $"{Application.streamingAssetsPath}/{SettingManager.assetSetting.BuildPlatform}".ToLower();
            if (Directory.Exists(targetPath)) Directory.Delete(targetPath, true);
            CopyDirectory(sourcePath, targetPath, true);
        }

        public static void CreateBundleFilesDictionary()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Debug.Log("不存在streamingAssetsPath目录");
                return;
            }

            var prePath = $"{Application.streamingAssetsPath}/{SettingManager.assetSetting.BuildPlatform}";
            var dicPath = $"{prePath}/{AAMTDefine.AAMT_BUNDLE_FILES_DICTIONARY}";
            var files   = Directory.GetFiles(prePath, "*", SearchOption.AllDirectories);
            if (File.Exists(dicPath)) File.Delete(dicPath);
            FileStream   fs       = new FileStream(dicPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw       = new StreamWriter(fs);
            var          i        = 0;
            var          tempPath = $"{prePath}\\";
            foreach (var file in files)
            {
                EditorCommon.UpdateProgress("正在写入文件列表", ++i, files.Length, file);
                if (file.EndsWith(".meta") || file.IndexOf(AAMTDefine.AAMT_BUNDLE_FILES_DICTIONARY, StringComparison.Ordinal) != -1) continue;
                var f = file.Replace(tempPath, "").Replace("\\", "/");
                sw.WriteLine(f);
            }

            sw.Close();
            fs.Close();
            EditorCommon.ClearProgressBar();
        }

        public static void CreateStreamingAssetsVersionData()
        {
            var filePath = $"{Application.streamingAssetsPath}/{SettingManager.assetSetting.BuildPlatform}/{AAMTDefine.AAMT_ASSET_VERSION}";
            var dirPath  = $"{Application.streamingAssetsPath}/{SettingManager.assetSetting.BuildPlatform}";
            EditorCommon.CreateVersionFile(filePath, dirPath);
        }

        /// <summary>
        /// 文件夹下所有内容copy
        /// </summary>
        /// <param name="sourcePath">要Copy的文件夹</param>
        /// <param name="destinationPath">要复制到哪个地方</param>
        /// <param name="overWriteExisting">是否覆盖</param>
        /// <returns></returns>
        public static bool CopyDirectory(string sourcePath, string destinationPath, bool overWriteExisting)
        {
            bool ret = false;
            try
            {
                sourcePath      = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath                + @"\";
                destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

                if (Directory.Exists(sourcePath))
                {
                    if (Directory.Exists(destinationPath) == false)
                        Directory.CreateDirectory(destinationPath);

                    foreach (string fls in Directory.GetFiles(sourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(destinationPath + flinfo.Name, overWriteExisting);
                    }

                    foreach (string drs in Directory.GetDirectories(sourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, destinationPath + drinfo.Name, overWriteExisting) == false)
                            ret = false;
                    }
                }

                ret = true;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("移动文件夹错误,{0}", ex.Message);
                ret = false;
            }

            return ret;
        }
    }
}