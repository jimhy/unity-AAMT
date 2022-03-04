using System;
using UnityEngine.SceneManagement;

namespace AAMT
{
    public class LocalLoader : ILoader
    {
        public LoaderHandler Load(string[] path)
        {
            return LoadLocalTask.GetTask(path).Run();
        }

        public void LoadScene(string path, LoadSceneMode mode, Action callBack)
        {
        }

        public void LoadScene(int buildSceneId, LoadSceneMode mode, Action callBack)
        {
        }
    }
}