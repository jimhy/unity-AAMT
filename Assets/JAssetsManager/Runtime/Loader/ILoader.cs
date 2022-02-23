using System;

namespace JAssetsManager
{
    public interface ILoader
    {
        void Load(string path, Action callBack);
    }
}