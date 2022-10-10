using System.Collections.Generic;

namespace AAMT
{
    public class VersionFileData
    {
        public string fileName;
        public string md5;
        public uint size;

        public VersionFileData()
        {
            
        }

        public VersionFileData(string fileName, string md5, uint size)
        {
            this.fileName = fileName;
            this.md5 = md5;
            this.size = size;
        }
    }
    
    public class VersionData
    {
        public Dictionary<string, VersionFileData> fileDatas;

        public VersionData()
        {
            fileDatas = new Dictionary<string, VersionFileData>();
        }

        public void Add(string fileName, string md5, uint size)
        {
            var fileData = new VersionFileData(fileName, md5, size);
            fileDatas.Add(fileName, fileData);
        }

        public VersionFileData Get(string fileName)
        {
            if (!fileDatas.ContainsKey(fileName)) return null;
            return fileDatas[fileName];
        }
    }
}