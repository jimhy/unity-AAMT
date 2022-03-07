using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace AAMT
{
    public class MoveBundleManager
    {
        internal MoveBundleManager()
        {
        }

        internal void MoveAssets()
        {
            var path = $"{Application.streamingAssetsPath}/{AAMTDefine.TOKEN_BUNDLE_FILES_DICTIONARY}";
            var value = Tools.ReadTextFileData(path);
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogErrorFormat("加载assets-info文件失败,path:{0}", path);
                return;
            }

            value = value.Replace("\r", "");
            var files = value.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var file in files)
            {
                var sourcePath = $"{Application.streamingAssetsPath}/{file}";
                var newPath = $"{Application.persistentDataPath}/{file}";
                AAMTRuntime.Instance.StartCoroutine(MoveFile(newPath, sourcePath));
            }
        }

        private IEnumerator MoveFile(string newPath, string sourcePath)
        {
            var request = UnityWebRequest.Get(sourcePath);
            yield return request.SendWebRequest();
            if (request.downloadHandler.data != null)
            {
                WriteFile(newPath, request.downloadHandler.data);
            }
            else
            {
                Debug.LogErrorFormat("移动文件时，加载源文件失败!path:{0}", sourcePath);
                OnOneFileComplete(newPath, false);
            }
        }

        private async void WriteFile(string path, byte[] data)
        {
            var dirName = Path.GetDirectoryName(path);
            if (dirName != null && !Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            await OnWriteFile(path, data);
            OnOneFileComplete(path, true);
        }

        private Task OnWriteFile(string path, byte[] data)
        {
            if (File.Exists(path)) File.Delete(path);
            return File.WriteAllBytesAsync(path, data);
        }


        private void OnOneFileComplete(string path, bool isSuccess)
        {
            if (isSuccess) Debug.LogFormat("移动文件成功:{0}", path);
            else Debug.LogErrorFormat("移动文件失败:{0}", path);
        }
    }
}