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
    public class EditorCommon
    {
        public static EventDispatcher EventBus;
        public static int _showFolderIcon = 0;

        public static bool ShowFolderIcon
        {
            get
            {
                if (_showFolderIcon == 0)
                {
                    if (PlayerPrefs.HasKey("showFolderIcon")) _showFolderIcon = PlayerPrefs.GetInt("showFolderIcon");
                    else _showFolderIcon                                      = 1;
                }

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
            float value = (float)progress / (float)progressMax;
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
            method.Invoke(new object(), null);
        }

        internal static bool CheckPath(string path)
        {
            if (!path.StartsWith("assets/") || path.LastIndexOf(".", StringComparison.Ordinal) == -1 || path.EndsWith(".meta") || path.EndsWith(".cs") || path.EndsWith(".xml") ||
                path.EndsWith(".txt")       || path.EndsWith(".tpsheet"))
                return false;
            return true;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string Md5ByFile(string file)
        {
            try
            {
                FileStream fs     = new FileStream(file, FileMode.Open);
                var        md5    = new MD5CryptoServiceProvider();
                byte[]     retVal = md5.ComputeHash(fs);
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

        public static void CreateVersionFile(string filePath, string dirPath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            FileStream   fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            var files       = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
            int i           = 1;
            var versionData = new VersionData();
            foreach (var file in files)
            {
                if (!CheckFile(file) || file.IndexOf(AAMTDefine.AAMT_ASSET_VERSION, StringComparison.Ordinal) != -1) continue;
                EditorCommon.UpdateProgress("正在计算文件", i++, files.Length, file);
                FileInfo fileInfo = new FileInfo(file);
                var      newPath  = file.Replace(dirPath, "").Replace("\\", "/");
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
            switch (SettingManager.assetSetting.GetBuildPlatform)
            {
                case AssetSetting.BuildTarget.windows:
                    return BuildTarget.StandaloneWindows;
                case AssetSetting.BuildTarget.android:
                    return BuildTarget.Android;
                case AssetSetting.BuildTarget.ios:
                    return BuildTarget.iOS;
            }

            return EditorUserBuildSettings.activeBuildTarget;
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