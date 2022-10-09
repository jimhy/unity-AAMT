using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LitJsonAAMT;
using UnityEngine;
using UnityEngine.Networking;

namespace AAMT
{
    public class BundleDowndloadManager
    {
        internal AsyncHandler handler;
        private Queue<string> loadFiles;
        private string remoteVersionFile;
        private string persistentVersionPath;
        private int downloadThreadCount = 25;

        public BundleDowndloadManager()
        {
            handler               = new AsyncHandler();
            loadFiles             = new Queue<string>();
            persistentVersionPath = AAMTDefine.AAMT_PERSISTENT_VERSION_PATH;
        }

        public async void Start()
        {
            await GetDownloadFiles();
            StartDownload();
        }

        private void StartDownload()
        {
            if (loadFiles.Count > 0)
            {
                for (int i = 0; i < loadFiles.Count; i++)
                {
                    if (downloadThreadCount != -1 && i >= downloadThreadCount) return;
                    OnLoad();
                }
            }
            else
            {
                OnAllDownloadComplete();
            }
        }

        private void OnLoad()
        {
            if (loadFiles.Count <= 0) return;
            var loadFile   = loadFiles.Dequeue();
            var url        = $"{SettingManager.assetSetting.getRemotePath}/{loadFile}";
            var targetPath = $"{Application.persistentDataPath}/{SettingManager.assetSetting.GetBuildPlatform}/{loadFile}".ToLower();
            var uwr        = UnityWebRequest.Get(url);
            if (File.Exists(targetPath)) File.Delete(targetPath);
            Debug.LogFormat("downloading-->url{0},targetPath:{1}", url, targetPath);
            uwr.downloadHandler = new DownloadHandlerFile(targetPath);
            var operation = uwr.SendWebRequest();
            operation.completed += OnDownLoadComplete;
        }

        private void OnDownLoadComplete(AsyncOperation o)
        {
            if (o is UnityWebRequestAsyncOperation operation)
            {
                if (handler == null) return;
                handler.currentBytes += (uint)operation.webRequest.downloadedBytes;
                handler.currentCount++;
                if (handler.currentCount >= handler.totalCount)
                {
                    handler.currentCount = handler.totalCount;
                    handler.OnProgress();
                    OnAllDownloadComplete();
                    return;
                }

                OnLoad();
                handler.OnProgress();
            }
        }

        private void OnAllDownloadComplete()
        {
            if (handler.totalCount > 0)
            {
                if (File.Exists(persistentVersionPath)) File.Delete(persistentVersionPath);
                File.WriteAllText(persistentVersionPath, remoteVersionFile);
            }

            handler.OnComplete();
            handler = null;
        }

        private async Task GetDownloadFiles()
        {
            var localFiles = await GetLocalVersionFiles();
            remoteVersionFile = await GetRemoteVersionFiles();
            CompareVersionFiles(localFiles, remoteVersionFile);
        }

        /// <summary>
        /// 加载远程资源版本文件
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetRemoteVersionFiles()
        {
            var path = Path.Combine(SettingManager.assetSetting.getRemotePath, AAMTDefine.AAMT_ASSET_VERSION);
            Debug.LogFormat("load path:{0}", path);
            var uwr = UnityWebRequest.Get(path);
            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                await Task.Delay(100);
            }

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogErrorFormat("文件下载失败,result:{0}", uwr.result);
                return "";
            }

            return uwr.downloadHandler.text;
        }

        /// <summary>
        /// 加载本地资源版本文件
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetLocalVersionFiles()
        {
            var path  = persistentVersionPath;
            var path1 = $"{Application.streamingAssetsPath}/{AAMTDefine.AAMT_ASSET_VERSION}";

            if (File.Exists(path))
            {
                Debug.LogFormat("load from persistentDataPath:{0}", path);
                var r = File.ReadAllTextAsync(path);
                await r;
                return r.Result;
            }

            Debug.LogFormat("load from streamingAssetsPath:{0}", path1);
            var req = UnityWebRequest.Get(path1);
            req.SendWebRequest();
            while (!req.isDone)
            {
                await Task.Delay(100);
            }

            if (req.result != UnityWebRequest.Result.Success) return "";

            return req.downloadHandler.text;
        }


        private void CompareVersionFiles(string localText, string removeText)
        {
            VersionData localData                           = null;
            if (!string.IsNullOrEmpty(localText)) localData = JsonMapper.ToObject<VersionData>(localText);
            var remoteData                                  = JsonMapper.ToObject<VersionData>(removeText);
            foreach (var k in remoteData.fileDatas)
            {
                var             remoteFile       = k.Value;
                VersionFileData localFile        = null;
                if (localData != null) localFile = localData.Get(remoteFile.fileName);
                if (localFile == null || localFile.md5 != remoteFile.md5)
                {
                    handler.totalBytes += remoteFile.size;
                    loadFiles.Enqueue(remoteFile.fileName);
                }
            }

            handler.totalCount = loadFiles.Count;
        }
    }
}