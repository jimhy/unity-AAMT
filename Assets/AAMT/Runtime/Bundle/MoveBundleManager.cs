using UnityEngine;

namespace AAMT
{
    public class MoveBundleManager
    {
        internal MoveBundleManager()
        {
        }

        internal void Start()
        {
            var path = $"{Application.streamingAssetsPath}/assets-info";
            var file = Tools.ReadTextFileData(path);
            if (string.IsNullOrEmpty(file))
            {
                Debug.LogErrorFormat("加载assets-info文件失败,path:{0}", path);
                return;
            }
            
        }
    }
}