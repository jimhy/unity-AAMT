using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AAMT
{
    public class AAMTDownloadManager
    {
        internal AAMTDownloadManager()
        {
            Init();
        }

        private void Init()
        {
            if(SettingManager.AssetSetting.GetLoadType != AssetSetting.LoadType.Remote) return;
            const string fileName = "assets-info.txt";
            var localFile = GetLocalFile(fileName);
            var remoteFile = GetRemoteFile(fileName);
        }

        private string GetLocalFile(string fileName)
        {
            var ast = SettingManager.AssetSetting;
            var path = $"{ast.GetLoadPath}/{fileName}";
            var fileVal = Tools.ReadTextFileData(path);
            if (string.IsNullOrEmpty(fileVal))
            {
                var path1 = $"{Application.streamingAssetsPath}/{ast.GetBuildTarget}/{fileName}";
                fileVal = Tools.ReadTextFileData(path1);
            }

            return fileVal;
        }

        private string GetRemoteFile(string path)
        {
            return Tools.ReadTextFileData(path);
        }

        internal void UpdateAssets()
        {
        }

        internal void MoveBundles()
        {
        }
    }
}