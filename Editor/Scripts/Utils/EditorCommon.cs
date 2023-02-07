using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AAMT.Editor;
using LitJsonAAMT;
using UnityEditor;
using UnityEngine;

namespace AAMT.Editor
{
    public static class EditorCommon
    {
        private static EventDispatcher _eventBus;

        public static EventDispatcher EventBus => _eventBus ??= new EventDispatcher();
        private static AssetBundlePackageData _packageData;
        public static AssetBundlePackageData PackageData => _packageData ??= new AssetBundlePackageData();
 
 
        private static int _showFolderIcon = 0;

        public static bool ShowFolderIcon
        {
            get
            {
                if (_showFolderIcon != 0) return _showFolderIcon == 1;
                _showFolderIcon = PlayerPrefs.HasKey("showFolderIcon") ? PlayerPrefs.GetInt("showFolderIcon") : 1;

                return _showFolderIcon == 1;
            }
            set
            {
                _showFolderIcon = value ? 1 : 2;
                PlayerPrefs.SetInt("showFolderIcon", _showFolderIcon);
            }
        }

        internal static void UpdateProgress(string title, int progress, int progressMax, string desc)
        {
            title = title + "...[" + progress + " - " + progressMax + "]";
            var value = (float)progress / progressMax;
            EditorUtility.DisplayProgressBar(title, desc, value);
        }

        internal static void ClearProgressBar()
        {
            EditorUtility.ClearProgressBar();
        }

        internal static void ClearConsole()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
            var type     = assembly.GetType("UnityEditor.LogEntries");
            var method   = type.GetMethod("Clear");
            method?.Invoke(new object(), null);
        }

        internal static bool CheckPath(string path)
        {
            return path.StartsWith("assets/") && path.LastIndexOf(".", StringComparison.Ordinal) != -1 && !path.EndsWith(".meta") && !path.EndsWith(".cs") && !path.EndsWith(".xml") &&
                   !path.EndsWith(".txt")     && !path.EndsWith(".tpsheet");
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string Md5ByFile(string file)
        {
            try
            {
                var fs     = new FileStream(file, FileMode.Open);
                var md5    = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(fs);
                fs.Close();

                var sb = new StringBuilder();
                foreach (var t in retVal)
                {
                    sb.Append(t.ToString("x2"));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        public static void CreateVersionFile(string filePath, string dirPath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
            var sw = new StreamWriter(fs);

            var files       = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
            var i           = 1;
            var versionData = new VersionData();
            foreach (var file in files)
            {
                if (!CheckFile(file) || file.IndexOf(AAMTDefine.AAMT_ASSET_VERSION, StringComparison.Ordinal) != -1) continue;
                UpdateProgress("正在计算文件", i++, files.Length, file);
                var fileInfo = new FileInfo(file);
                var newPath  = file.Replace(dirPath, "").Replace("\\", "/");
                newPath = newPath.Substring(1, newPath.Length - 1);
                versionData.Add(newPath, Md5ByFile(file), (uint)fileInfo.Length);
            }

            var json = JsonMapper.ToJson(versionData);
            sw.Write(json);
            sw.Close();
            fs.Close();

            EditorUtility.ClearProgressBar();
        }

        private static bool CheckFile(string file)
        {
            return !(file.EndsWith(".meta") || file.EndsWith(".apk") || file.EndsWith(".manifest") || file.EndsWith(".idea"));
        }

        internal static BuildTarget AamtToEditorTarget()
        {
            return SettingManager.assetSetting.GetBuildPlatform switch
            {
                AssetSetting.BuildTarget.windows => BuildTarget.StandaloneWindows,
                AssetSetting.BuildTarget.android => BuildTarget.Android,
                AssetSetting.BuildTarget.ios     => BuildTarget.iOS,
                _                                => EditorUserBuildSettings.activeBuildTarget
            };
        }

        internal static AssetSetting.BuildTarget EditorToAamtTarget()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return AssetSetting.BuildTarget.windows;
                case BuildTarget.Android:
                    return AssetSetting.BuildTarget.android;
                case BuildTarget.iOS:
                    return AssetSetting.BuildTarget.ios;
            }

            return AssetSetting.BuildTarget.editor;
        }
    }
}