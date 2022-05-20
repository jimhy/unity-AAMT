using System;
using UnityEngine.SceneManagement;

namespace AAMT
{
    public interface ILoader
    {
        AsyncHandler LoadAsync(string[] path);
    }
}