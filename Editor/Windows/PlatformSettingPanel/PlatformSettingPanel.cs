using System;
using System.Collections.Generic;
using System.Reflection;
using AAMT;
using AAMT.Editor.Components;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class PlatformSettingPanel : ContentNode
    {
        private ToolbarButton _deleteBtn;
        private TextField _nameLabel;
        private DropdownField _platform;
        private TextField _buildPath;
        private DropdownField _loadType;
        private TextField _remoteUrl;
        private VisualElement _bottom;
        private AssetSetting _data;

        public PlatformSettingPanel()
        {
            var uml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathManager.PlatformSettingPanelPath);
            uml.CloneTree(this);

            initElements();
        }

        private void initElements()
        {
            _deleteBtn = this.Q<ToolbarButton>("DeleteBtn");
            _nameLabel = this.Q<TextField>("NameLabel");
            _platform  = this.Q<DropdownField>("Platform");
            _buildPath = this.Q<TextField>("BuildPath");
            _loadType  = this.Q<DropdownField>("LoadType");
            _remoteUrl = this.Q<TextField>("RemoteUrl");
            _bottom    = this.Q<VisualElement>("Bottom");

            _platform.choices = MiscUtils.EnumToStringList(typeof(AssetSetting.BuildTarget));
            _platform.index   = 0;
            _buildPath.value  = "{UnityEngine.Application.dataPath}/../Build/";
            _remoteUrl.value  = "http://localhost:80";
            _loadType.choices = MiscUtils.EnumToStringList(typeof(AssetSetting.LoadType));
            _loadType.index   = 0;

            AddEvents();
        }

        private void AddEvents()
        {
            _deleteBtn.clicked += OnDeleteClick;
            _platform.RegisterValueChangedCallback(OnPlatformChanged);
            _loadType.RegisterValueChangedCallback(OnLoadTypeChanged);
        }

        private void OnLoadTypeChanged(ChangeEvent<string> evt)
        {
            var i = MiscUtils.StringToEnum<AssetSetting.LoadType>(evt.newValue);
            if (i == -1) return;
            var p = (AssetSetting.LoadType)i;
            SetDataProperty("loadType", p);
            updateUI();
        }

        private void OnPlatformChanged(ChangeEvent<string> evt)
        {
            var i = MiscUtils.StringToEnum<AssetSetting.BuildTarget>(evt.newValue);
            if (i == -1) return;
            var p = (AssetSetting.BuildTarget)i;
            SetDataProperty("buildPlatform", p);
            updateUI();
        }

        private void OnDeleteClick()
        {
            Debug.Log("OnDeleteClick...");
        }

        public override void SetData(object o)
        {
            if (!(o is AssetSetting)) return;
            _data = o as AssetSetting;
            updateUI();
        }

        private void updateUI()
        {
            _nameLabel.value = _data.fileName;
            _platform.value  = _data.GetBuildPlatform.ToString();
            _loadType.value  = _data.getLoadType.ToString();

            var displayType1 = DisplayStyle.Flex;
            var displayType2 = DisplayStyle.Flex;

            if (_data.GetBuildPlatform == AssetSetting.BuildTarget.editor) displayType1                                                     = DisplayStyle.None;
            if (_data.GetBuildPlatform == AssetSetting.BuildTarget.editor || _data.getLoadType == AssetSetting.LoadType.Local) displayType2 = DisplayStyle.None;

            _buildPath.style.display = displayType1;
            _loadType.style.display  = displayType1;
            _remoteUrl.style.display = displayType1;
            _bottom.style.display    = displayType2;
        }

        private void SetDataProperty(string propertyName, object val)
        {
            var t     = _data.GetType();
            var field = t.GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) field.SetValue(_data, val);
        }
    }
}