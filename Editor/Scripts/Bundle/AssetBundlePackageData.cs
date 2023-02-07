using System.Collections.Generic;
using System.Linq;
using LitJsonAAMT;
using UnityEditor;
using File = System.IO.File;

namespace AAMT.Editor
{
    public class AssetBundlePackageData
    {
        public List<PackageData> Guids { get; private set; } = new();

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
                Guids = data;
            }
        }

        public void Set(string path, WindowDefine.ABType abType)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            var data = GetDataByGuid(guid);
            if (data != null) data.AbType = abType;
            else Guids.Add(new PackageData(guid, path, abType));
            Save();
        }

        public void RemoveByPath(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            RemoveByGuid(guid);
        }

        public void RemoveByGuid(string guid)
        {
            foreach (var packageData in Guids.Where(packageData => packageData.Guid == guid))
            {
                Guids.Remove(packageData);
                Save();
                return;
            }
        }

        public PackageData GetDataByPath(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            return GetDataByGuid(guid);
        }

        public PackageData GetDataByGuid(string guid)
        {
            return Guids.FirstOrDefault(packageData => packageData.Guid == guid);
        }

        private void Save()
        {
            var json = JsonMapper.ToJson(Guids);
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
            Path   = path.Replace("\\", "/");
            AbType = abType;
        }
    }
}