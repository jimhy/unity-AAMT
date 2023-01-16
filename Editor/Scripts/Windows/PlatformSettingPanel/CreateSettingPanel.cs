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
            AssetDatabase.CreateAsset(_data, $"{WindowDefine.platformSettingPath}/{_data.fileName}.asset");
            AssetDatabase.SaveAssets();
            initPlatformData();
            initElementData();
            updateUI();
        }

        public override void SetData(object o)
        {
        }
    }
}