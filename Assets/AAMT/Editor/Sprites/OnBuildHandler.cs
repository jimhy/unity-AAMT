using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AAMT.Editor.Sprites
{
    public class OnBuildHandler : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }

        /// <summary>
        /// 打包前处理
        /// </summary>
        /// <param name="report"></param>
        public void OnPreprocessBuild(BuildReport report)
        {
            MoveBundleToStreamingAssets();
        }

        /// <summary>
        /// 打包后处理
        /// </summary>
        /// <param name="report"></param>
        public void OnPostprocessBuild(BuildReport report)
        {
            var list = SettingManager.AssetSetting.GetStreamingAssetsPaths;
            foreach (var s in list)
            {
                var path = $"{Application.streamingAssetsPath}/{s}";
                if (Directory.Exists(path)) Directory.Delete(path, true);
            }
            AssetDatabase.Refresh();
        }

        private static void MoveBundleToStreamingAssets()
        {
            var fileNames = new string[] {"aamt.ab", "aamt.ab.manifest"};
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
                var targetPath = $"{Application.streamingAssetsPath}/{s}";
                EditorCommon.UpdateProgress("正在移动文件", ++i, list.Length, sourcePath);
                Debug.LogFormat("sourcePath:{0}----targetPath:{1}", sourcePath, targetPath);
                if (Directory.Exists(targetPath)) Directory.Delete(targetPath, true);
                CopyDirectory(sourcePath, targetPath, true);
            }

            EditorCommon.ClearProgressBar();
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