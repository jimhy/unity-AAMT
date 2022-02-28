using System;

namespace AAMT
{
    public interface ILoader
    {
        void Load(string[] path, Action<object> callBack,object data);
    }
}