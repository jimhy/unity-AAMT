using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;

namespace AAMT
{
    public class EditorCommon
    {
        internal static void UpdateProgress(string title, int progress, int progressMax, string desc)
        {
            title = title + "...[" + progress + " - " + progressMax + "]";
            float value = (float) progress / (float) progressMax;
            EditorUtility.DisplayProgressBar(title, desc, value);
        }

        internal static void ClearProgressBar()
        {
            EditorUtility.ClearProgressBar();
        }

        internal static void ClearConsole()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        internal static bool CheckPath(string path)
        {
            if (!path.StartsWith("assets/") ||
                path.LastIndexOf(".", StringComparison.Ordinal) == -1 ||
                path.EndsWith(".meta") ||
                path.EndsWith(".cs") ||
                path.EndsWith(".xml") ||
                path.EndsWith(".txt") ||
                path.EndsWith(".tpsheet")
               )
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