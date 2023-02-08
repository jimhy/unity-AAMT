using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public sealed class CreateSettingPanel : PlatformSettingPanel
    {
        private Button _createBtn;

        public CreateSettingPanel()
        {
            InitPlatformData();
            UpdateUI();
        }

        private void InitPlatformData()
        {
            _data      = ScriptableObject.CreateInstance<AssetSetting>();
            _data.name = "New platform";
        }

        protected override void InitElements()
        {
            base.InitElements();
            _deleteBtn.visible = false;

            _createBtn = new Button { name = "CreateButton", text = "创建平台" };
            Add(_createBtn);
        }

        protected override void AddEvents()
        {
            base.AddEvents();
            _createBtn.clicked += OnCreateData;
        }

        private void OnCreateData()
        {
            if (!Directory.Exists(WindowDefine.platformSettingPath)) Directory.CreateDirectory(WindowDefine.platformSettingPath);
            var path = GetFilePath();
            AssetDatabase.CreateAsset(_data, path);
            AssetDatabase.SaveAssets();
            _data.Guid = AssetDatabase.AssetPathToGUID(path);
            _data.Save();
            InitPlatformData();
            InitElementData();
            UpdateUI();
        }

        private string GetFilePath()
        {
            var fileName = _nameLabel.value;
            var path     = $"{WindowDefine.platformSettingPath}/{fileName}.asset";
            var i        = 1;
            while (File.Exists(path))
            {
                _data.name = $"{fileName}({i++})";
                path           = $"{WindowDefine.platformSettingPath}/{fileName}.asset";
            }

            return path;
        }

        public override void SetData(object o)
        {
        }
    }
}