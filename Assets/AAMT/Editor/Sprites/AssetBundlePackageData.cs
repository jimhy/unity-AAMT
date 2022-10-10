using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    public class AssetBundlePackageData : ScriptableObject
    {
        [SerializeField]
        public List<PackageData> _guids = new List<PackageData>();

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
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void CloneData(AssetBundlePackageData source)
        {
            foreach (var packageData in source._guids)
            {
                _guids.Add(new PackageData(packageData));
            }
        }
    }

    [Serializable]
    public class PackageData
    {
        public string Guid;
        public string Path;
        public WindowDefine.ABType AbType;

        public PackageData(string guid, string path, WindowDefine.ABType abType)
        {
            Guid   = guid;
            Path   = path;
            AbType = abType;
        }

        public PackageData(PackageData data)
        {
            Guid   = data.Guid;
            Path   = data.Path;
            AbType = data.AbType;
        }
    }
}