using System.Collections.Generic;
using LitJsonAAMT;
using UnityEditor;
using File = System.IO.File;

namespace AAMT.Editor
{
    public class AssetBundlePackageData
    {
        public List<PackageData> _guids = new List<PackageData>();

        public AssetBundlePackageData()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (!File.Exists(AAMTDefine.AAMT_BUNDLE_PACKAGE_DATA)) return;
            var text = File.ReadAllText(AAMTDefine.AAMT_BUNDLE_PACKAGE_DATA);
            var data = JsonMapper.ToObject<List<PackageData>>(text);
            if (data != null)
            {
                _guids = data;
            }
        }

        public void Set(string path, WindowDefine.ABType abType)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            var data = getData(guid);
            if (data != null) data.AbType = abType;
            else _guids.Add(new PackageData(guid, path, abType));
            Save();
        }

        public void Remove(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            foreach (var packageData in _guids)
            {
                if (packageData.Guid == guid)
                {
                    _guids.Remove(packageData);
                    Save();
                    return;
                }
            }
        }

        public PackageData GetData(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            return getData(guid);
        }

        private PackageData getData(string guid)
        {
            foreach (var packageData in _guids)
            {
                if (packageData.Guid == guid) return packageData;
            }

            return null;
        }

        private void Save()
        {
            var json = JsonMapper.ToJson(_guids);
            File.WriteAllText(AAMTDefine.AAMT_BUNDLE_PACKAGE_DATA, json);
        }
    }

    public class PackageData
    {
        public string Guid;
        public string Path;
        public WindowDefine.ABType AbType;

        public PackageData()
        {
            
        }

        public PackageData(string guid, string path, WindowDefine.ABType abType)
        {
            Guid   = guid;
            Path   = path.Replace("\\","/");
            AbType = abType;
        }
    }
}