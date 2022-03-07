using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AAMT.Editor.Sprites
{
    public class OnPreprocessHandler : AssetPostprocessor
    {
        public int callbackOrder { get; }

        /// <summary>
        /// 打包前处理
        /// </summary>
        /// <param name="report"></param>
        private void OnPreprocessBuild(BuildReport report)
        {
            MoveBundleToStreamingAssets();
            CreateBundleFilesDictionary();
        }

        /// <summary>
        /// 打包后处理
        /// </summary>
        /// <param name="report"></param>
        private void OnPostprocessBuild(BuildReport report)
        {
            DeleteTempFiles();
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                if (path.EndsWith(".asset") && path.IndexOf("AAMT/Data", StringComparison.Ordinal) != -1)
                {
                    SettingManager.ReloadAssetSetting();
                }
            }
        }


        private static void DeleteTempFiles()
        {
            var list = SettingManager.AssetSetting.GetStreamingAssetsPaths;
            foreach (var s in list)
            {
                var n = s.IndexOf("/", StringComparison.Ordinal);
                var dir = s;
                if (n != -1)
                {
                    dir = s[..n];
                }

                var path = $"{Application.streamingAssetsPath}/{dir}";
                var meta = $"{path}.meta";
                if (Directory.Exists(path)) Directory.Delete(path, true);
                if (File.Exists(meta)) File.Delete(meta);
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("AAMT/MoveBundleToStreamingAssets")]
        private static void MoveBundleToStreamingAssets()
        {
            var fileNames = new string[] {$"{AAMTDefine.AAMT_BUNDLE_NAME}", $"{AAMTDefine.AAMT_BUNDLE_NAME}.manifest"};
            foreach (var fileName in fileNames)
            {
                var buildPath = $"{SettingManager.AssetSetting.GetBuildPath}/{fileName}";
                var targetPath = $"{Application.streamingAssetsPath}/{fileName}";
                if (File.Exists(targetPath)) File.Delete(targetPath);
                File.Copy(buildPath, targetPath);
            }

            var list = SettingManager.AssetSetting.GetStreamingAssetsPaths;
            var i = 0;
            foreach (var s in list)
            {
                var sourcePath = $"{SettingManager.AssetSetting.GetBuildPath}/{s}";
                var targetPath = $"{Application.streamingAssetsPath}/{SettingManager.AssetSetting.GetBuildTarget}/{s}";
                EditorCommon.UpdateProgress("正在移动文件", ++i, list.Length, sourcePath);
                if (Directory.Exists(targetPath)) Directory.Delete(targetPath, true);
                CopyDirectory(sourcePath, targetPath, true);
            }

            EditorCommon.ClearProgressBar();
        }

        [MenuItem("AAMT/CreateBundleFilesDictionary")]
        private static void CreateBundleFilesDictionary()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Debug.Log("不存在streamingAssetsPath目录");
                return;
            }

            var dicPath = $"{Application.streamingAssetsPath}/{AAMTDefine.TOKEN_BUNDLE_FILES_DICTIONARY}";
            var files = Directory.GetFiles(Application.streamingAssetsPath, "*", SearchOption.AllDirectories);
            if (File.Exists(dicPath)) File.Delete(dicPath);
            FileStream fs = new FileStream(dicPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            var tmpPath = $"{Application.streamingAssetsPath}\\";
            var i = 0;
            foreach (var file in files)
            {
                EditorCommon.UpdateProgress("正在写入文件列表", ++i, files.Length, file);
                if (file.EndsWith(".meta") ||
                    file.IndexOf(AAMTDefine.TOKEN_BUNDLE_FILES_DICTIONARY, StringComparison.Ordinal) != -1) continue;
                var f = file.Replace(tmpPath, "").Replace("\\", "/");
                sw.WriteLine(f);
            }

            sw.Close();
            fs.Close();
            EditorCommon.ClearProgressBar();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 文件夹下所有内容copy
        /// </summary>
        /// <param name="sourcePath">要Copy的文件夹</param>
        /// <param name="destinationPath">要复制到哪个地方</param>
        /// <param name="overWriteExisting">是否覆盖</param>
        /// <returns></returns>
        private static bool CopyDirectory(string sourcePath, string destinationPath, bool overWriteExisting)
        {
            bool ret = false;
            try
            {
                sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
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