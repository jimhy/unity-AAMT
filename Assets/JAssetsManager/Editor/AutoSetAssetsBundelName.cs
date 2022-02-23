﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JAssetsManager
{
    public class AutoSetAssetsBundelName : AssetPostprocessor
    {
        private static string dbName = "autoNameAB";
        public static UnityEngine.Object File;
        public static readonly string ResourcePath = Application.dataPath + "/Resources/autoNameAB";
        public static string RoleFbxPath = Application.dataPath + "/Res/Role/fbx";

        [MenuItem("Assets/AutoNamingAB/自动命名autoNameAB", false, 51)]
        public static void AutoAddAbName()
        {
            StartAutoName(true);
        }

        [MenuItem("Assets/AutoNamingAB/删除ab名字", false, 51)]
        public static void RemoveAbName()
        {
            StartAutoName(false);
        }

        private static void StartAutoName(bool isSetName = true)
        {
            var files = new List<String>();
            foreach (var item in Selection.objects)
            {
                var p = AssetDatabase.GetAssetPath(item);
                var f = Path.GetFileName(p);
                if (f.LastIndexOf(".", StringComparison.Ordinal) != -1) continue;
                var fs = Directory.GetFiles(p, "*", SearchOption.AllDirectories);
                files.AddRange(fs);
            }

            var total = files.Count;
            var i = 0;
            foreach (var item in files)
            {
                EditorCommon.UpdateProgress("命名autoNameAB", ++i, total, item);
                AutoName(item,isSetName);
            }

            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Assets/AutoNamingAB/复制命名autoNameAB规则文件(所有子目录)", false, 10)]
        private static void CopyFileAll()
        {
            var selects = Selection.objects;
            foreach (var item in selects)
            {
                string path = AssetDatabase.GetAssetPath(item);
                string[] all = Directory.GetDirectories(path, "*", SearchOption.AllDirectories); //获取全部的文件夹名字
                string f = path + "/" + "autoNameAB";
                System.IO.File.Copy(ResourcePath, f, true);
                foreach (string target in all)
                {
                    f = target + "/" + "autoNameAB";
                    System.IO.File.Copy(ResourcePath, f, true);
                }

                AssetDatabase.Refresh();
            }
        }

        [MenuItem("Assets/AutoNamingAB/复制命名autoNameAB规则文件(1级子目录)", false, 11)]
        private static void CopyFile1()
        {
            CopyFile(1);
        }

        [MenuItem("Assets/AutoNamingAB/复制命名autoNameAB规则文件(2级子目录)", false, 12)]
        private static void CopyFile2()
        {
            CopyFile(2);
        }

        [MenuItem("Assets/AutoNamingAB/复制命名autoNameAB规则文件(3级子目录)", false, 13)]
        private static void CopyFile3()
        {
            CopyFile(3);
        }

        [MenuItem("Assets/AutoNamingAB/复制命名autoNameAB规则文件(4级子目录)", false, 14)]
        private static void CopyFile4()
        {
            CopyFile(4);
        }

        private static void CopyFile(int num)
        {
            var selects = Selection.objects;
            foreach (var item in selects)
            {
                string path = AssetDatabase.GetAssetPath(item);
                StartCopyFile(path, num);
                AssetDatabase.Refresh();
            }
        }

        private static void StartCopyFile(string path, int num)
        {
            if (num <= 0) return;
            string f = path + "/" + "autoNameAB";
            System.IO.File.Copy(ResourcePath, f, true);
            if (--num > 0)
            {
                string[] all = Directory.GetDirectories(path); //获取全部的文件夹名字
                foreach (string target in all)
                {
                    StartCopyFile(target, num);
                }
            }
        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            return; //TODO:这里暂时注释掉，项目有大量资源的时候，这里会导致一直重新刷新资源
#if !UNITY_IOS
            foreach (var str in importedAssets)
            {
                if (str.IndexOf("Assets/Res") != -1 && str.LastIndexOf(".", StringComparison.Ordinal) == -1)
                {
                    //这里设计成，只要是目录就给目录添加autoNameAB文件
                    if (Directory.Exists(str))
                    {
                        var f = str + "/" + "autoNameAB";
                        System.IO.File.Copy(ResourcePath, f, true);
                    }

                    continue;
                }

                AutoName(str,true, Path.GetDirectoryName(str));
            }

            foreach (var path in movedAssets)
            {
                AutoName(path);
            }
#endif
        }

        private static void AutoName(string path, bool isSetName = true, string rootPath = "")
        {
            path = path.Replace("\\", "/").ToLower();
            rootPath = rootPath.Replace("\\", "/").ToLower();
            if (!CheckPath(path)) return;
            var tempPath = path.Replace("assets/res/", "");
            if (!string.IsNullOrEmpty(rootPath))
            {
                int pos = rootPath.IndexOf("assets/res/", StringComparison.Ordinal);
                if (pos > 0)
                {
                    rootPath = rootPath.Substring(pos);
                    rootPath = rootPath.Replace("assets/res/", "");
                }
            }

            var p = GetBundleName(tempPath, rootPath);
            if (p != null) tempPath = p;
            AssetImporter item = AssetImporter.GetAtPath(path);
            if (path.EndsWith(".unity") || !isSetName) item.assetBundleName = null;
            else item.assetBundleName = tempPath + ".ab";
        }

        private static string GetBundleName(string path, string rootPath = "")
        {
            var currentPath = Path.GetDirectoryName(path)?.Replace("\\", "/");
            if (string.IsNullOrEmpty(currentPath)) currentPath = path;
            var directoryName = Path.GetFileName(currentPath).Replace("\\", "/");
            var configFile = $"assets/res/{currentPath}/{dbName}";
            if (!System.IO.File.Exists(configFile))
            {
                if (currentPath != rootPath && rootPath != String.Empty)
                {
                    int pos = currentPath.LastIndexOf("/", StringComparison.Ordinal);
                    if (pos != -1)
                    {
                        currentPath = currentPath.Substring(0, pos + 1);
                    }
                    else
                    {
                        //Debug.LogError($"{rootPath}文件夹下面没有autoNameAB文件");
                        return null;
                    }

                    return GetBundleName(currentPath, rootPath);
                }

                path = currentPath.Replace(directoryName, "");
                // if (path.Length != 0 && path[path.Length - 1] == '/') path = path.Remove(path.Length - 1);
                if (string.IsNullOrEmpty(path)) return null;
                return GetBundleName(path);
            }

            int type = GetType(configFile);
            if (type == 3) return "CommonRes";
            if (type != 2) return null; //类型:1:表示命名单个文件;2:命名整个文件夹包括子文件夹;3:表示公共资源

            return currentPath;
        }

        private static int GetType(string configFile)
        {
            var lines = System.IO.File.ReadAllLines(configFile);
            foreach (var line in lines)
            {
                if (line[0] == '#') continue;
                if (line.IndexOf("type=", StringComparison.Ordinal) != -1)
                {
                    var s = line.Replace("type=", "");
                    var n = int.Parse(s);
                    return n;
                }
            }

            return -1;
        }

        private static string GetNewType(string path)
        {
            if (path.Contains("_mat") || path.Contains("_an") || path.Contains("_fbx") || path.Contains("_png") ||
                path.Contains("_tga")) return "";
            if (path.EndsWith(".perfab")) return ""; //预制体不同命名
            else if (path.EndsWith(".mat")) return "_mat";
            else if (path.EndsWith(".anim")) return "_an";
            else if (path.EndsWith(".FBX") || path.EndsWith(".fbx")) return "_fbx";
            else if (path.EndsWith(".png")) return "_png";
            else if (path.EndsWith(".tga")) return "_tga";
            else return "";
        }

        private static bool CheckPath(string path)
        {
            if (!path.StartsWith("assets/res/") ||
                path.LastIndexOf(".", StringComparison.Ordinal) == -1 ||
                path.EndsWith(".meta") ||
                path.EndsWith(".cs") ||
                path.EndsWith(".xml") ||
                // path.EndsWith(".shader") ||
                //path.EndsWith(".tpsheet") ||
                path.EndsWith(".txt")
                //path.EndsWith(".TTF") ||
                //path.EndsWith(".otf")
               )
                return false;
            return true;
        }
    }
}