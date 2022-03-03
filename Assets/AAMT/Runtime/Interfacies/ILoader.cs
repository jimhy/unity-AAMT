using System;

namespace AAMT
{
    public interface ILoader
    {
        LoaderHandler Load(string[] path);
    }
}