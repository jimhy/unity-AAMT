using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class CreateSettingPanel : PlatformSettingPanel
    {
        private Button _createBtn;

        public CreateSettingPanel()
        {
            initPlatformData();
            updateUI();
        }

        private void initPlatformData()
        {
            _data          = ScriptableObject.CreateInstance<AssetSetting>();
            _data.fileName = "New platform";
        }

        protected override void initElements()
        {
            base.initElements();
            _deleteBtn.visible = false;

            _createBtn      = new Button();
            _createBtn.name = "CreateButton";
            _createBtn.text = "创建平台";
            this.Add(_createBtn);
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
            initPlatformData();
            initElementData();
            updateUI();
        }

        private string GetFilePath()
        {
            var path     = $"{WindowDefine.platformSettingPath}/{_data.fileName}.asset";
            var i        = 1;
            var fileName = _data.fileName;
            while (File.Exists(path))
            {
                _data.fileName = $"{fileName}({i++})";
                path           = $"{WindowDefine.platformSettingPath}/{_data.fileName}.asset";
            }

            return path;
        }

        public override void SetData(object o)
        {
        }
    }
}