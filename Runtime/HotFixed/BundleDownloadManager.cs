using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LitJsonAAMT;
using UnityEngine;
using UnityEngine.Networking;

namespace AAMT
{
    public class BundleDownloadManager
    {
        internal AsyncHandler Handler;
        private readonly Queue<string> _loadFiles;
        private string _remoteVersionFile;
        private readonly string _persistentVersionPath;
        private readonly int _downloadThreadCount = 25;
        private readonly int _errorTimes = 5;
        private readonly Dictionary<string, int> _errorLoadTimesList;

        public BundleDownloadManager()
        {
            Handler                = new AsyncHandler();
            _loadFiles             = new Queue<string>();
            _persistentVersionPath = AAMTDefine.AAMT_PERSISTENT_VERSION_PATH;
            _errorLoadTimesList    = new Dictionary<string, int>();
        }

        public async void Start()
        {
            await GetDownloadFiles();
            StartDownload();
        }

        private void StartDownload()
        {
            if (_loadFiles.Count > 0)
            {
                for (int i = 0; i < _loadFiles.Count; i++)
                {
                    if (_downloadThreadCount != -1 && i >= _downloadThreadCount) return;
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
            if (_loadFiles.Count <= 0) return;
            var loadFile = _loadFiles.Dequeue();
            StartRequestHttp(loadFile);
        }

        private void StartRequestHttp(string loadFile)
        {
            var url        = $"{SettingManager.assetSetting.getRemotePath}{loadFile}";
            var targetPath = $"{Application.persistentDataPath}/{SettingManager.assetSetting.GetBuildPlatform}/{loadFile}".ToLower();
            if (File.Exists(targetPath)) File.Delete(targetPath);
            var uwr = UnityWebRequest.Get(url);
            Debug.LogFormat("downloading-->url{0},targetPath:{1}", url, targetPath);
            uwr.downloadHandler = new DownloadHandlerFile(targetPath);
            var operation = uwr.SendWebRequest();
            operation.completed += OnDownLoadComplete;
        }

        private void OnDownLoadComplete(AsyncOperation o)
        {
            if (o is not UnityWebRequestAsyncOperation operation) return;

            if (operation.webRequest.result != UnityWebRequest.Result.Success)
            {
                if (ReDownload(operation.webRequest.url)) return;
                Debug.LogError($"download file error:{operation.webRequest.url}");
            }

            if (Handler == null) return;
            Handler.currentBytes += (uint)operation.webRequest.downloadedBytes;
            Handler.currentCount++;
            if (Handler.currentCount >= Handler.totalCount)
            {
                Handler.currentCount = Handler.totalCount;
                Handler.OnProgress();
                OnAllDownloadComplete();
                return;
            }

            OnLoad();
            Handler.OnProgress();
        }

        private bool ReDownload(string webRequestURL)
        {
            var count = 0;

            if (_errorLoadTimesList.ContainsKey(webRequestURL)) count = _errorLoadTimesList[webRequestURL];
            if (++count >= _errorTimes) return false;
            _errorLoadTimesList[webRequestURL] = count;
            var loadFile = webRequestURL.Replace($"{SettingManager.assetSetting.getRemotePath}", "");

            AAMTRuntime.Instance.CallOnNextFrame(o =>
            {
                if (o is not string s) return;
                Debug.Log($"downloading error file-->{webRequestURL}");
                StartRequestHttp(s);
            }, 60, loadFile);
            return true;
        }

        private void OnAllDownloadComplete()
        {
            if (Handler.totalCount > 0)
            {
                if (File.Exists(_persistentVersionPath)) File.Delete(_persistentVersionPath);
                File.WriteAllText(_persistentVersionPath, _remoteVersionFile);
            }

            Handler.OnComplete();
            Handler = null;
        }

        private async Task GetDownloadFiles()
        {
            var localFiles = await GetLocalVersionFiles();
            _remoteVersionFile = await GetRemoteVersionFiles();
            CompareVersionFiles(localFiles, _remoteVersionFile);
        }

        /// <summary>
        /// 加载远程资源版本文件
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetRemoteVersionFiles()
        {
            var path = $"{SettingManager.assetSetting.getRemotePath}/{AAMTDefine.AAMT_ASSET_VERSION}?{System.DateTime.Now.Ticks}";
            Debug.LogFormat("get remote version files path:{0}", path);
            var uwr = UnityWebRequest.Get(path);
            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                await Task.Delay(100);
            }

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogErrorFormat("download remote version files error,result:{0},path:{1}", uwr.result, uwr.url);
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
            var path  = _persistentVersionPath;
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
                    Handler.totalBytes += remoteFile.size;
                    _loadFiles.Enqueue(remoteFile.fileName);
                }
            }

            Handler.totalCount = _loadFiles.Count;
        }
    }
}