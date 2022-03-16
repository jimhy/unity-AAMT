using System;
using UnityEngine.SceneManagement;

namespace AAMT
{
    public interface ILoader
    {
        AsyncHandler Load(string[] path);
    }
}