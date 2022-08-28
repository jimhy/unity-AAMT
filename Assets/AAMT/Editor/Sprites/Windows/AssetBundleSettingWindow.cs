using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace AAMT.Windos
{
    public class AssetBundleSettingWindow : OdinEditorWindow
    {
        [TableList]
        public List<FloadData> FloadDatas = new List<FloadData>();

        public AssetBundleSettingWindow()
        {
            FloadDatas.Add(new FloadData());
            FloadDatas.Add(new FloadData());
            FloadDatas.Add(new FloadData());
        }
    }

    public class FloadData
    {
        public bool            select;
        public string          name;
        public string          path;
        [TableList]
        public List<FloadData> child = new List<FloadData>();
        [LabelText("打包类型")]
        public  WindowDefine.ABType abType;
    }
}