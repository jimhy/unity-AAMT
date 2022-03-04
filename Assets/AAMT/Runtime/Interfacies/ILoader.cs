using System;
using UnityEngine.SceneManagement;

namespace AAMT
{
    public interface ILoader
    {
        LoaderHandler Load(string[] path);
    }
}