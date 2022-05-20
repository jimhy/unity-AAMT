using System;
using UnityEngine.SceneManagement;

namespace AAMT
{
    public class LocalLoader : ILoader
    {
        public AsyncHandler LoadAsync(string[] path)
        {
            return LoadLocalTask.GetTask(path).Run();
        }
    }
}