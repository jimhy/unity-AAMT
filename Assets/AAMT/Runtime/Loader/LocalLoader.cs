using System;
using UnityEngine.SceneManagement;

namespace AAMT
{
    public class LocalLoader : ILoader
    {
        public AsyncHandler Load(string[] path)
        {
            return LoadLocalTask.GetTask(path).Run();
        }
    }
}