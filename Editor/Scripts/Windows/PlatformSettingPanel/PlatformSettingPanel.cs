﻿using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class PlatformSettingPanel : ContentNode
    {
        protected ToolbarButton _deleteBtn;
        protected TextField     _nameLabel;
        protected DropdownField _platform;
        protected TextField     _buildPath;
        protected DropdownField _loadType;
        protected TextField     _remoteUrl;
        protected VisualElement _bottom;
        protected AssetSetting  _data;

        public PlatformSettingPanel()
        {
            var uml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathManager.PlatformSettingPanelPath);
            uml.CloneTree(this);

            initElements();
            AddEvents();
        }

        protected virtual void initElements()
        {
            _deleteBtn = this.Q<ToolbarButton>("DeleteBtn");
            _nameLabel = this.Q<TextField>("NameLabel");
            _platform  = this.Q<DropdownField>("Platform");
            _buildPath = this.Q<TextField>("BuildPath");
            _loadType  = this.Q<DropdownField>("LoadType");
            _remoteUrl = this.Q<TextField>("RemoteUrl");
            _bottom    = this.Q<VisualElement>("Bottom");

            _nameLabel.userData = "fileName";
            _buildPath.userData = "buildPath";
            _remoteUrl.userData = "remotePath";

            initElementData();
        }

        protected virtual void initElementData()
        {
            _platform.choices = MiscUtils.EnumToStringList(typeof(AssetSetting.BuildTarget));
            _platform.index   = 0;
            _buildPath.value  = "{UnityEngine.Application.dataPath}/../Build/";
            _remoteUrl.value  = "http://localhost:80";
            _loadType.choices = MiscUtils.EnumToStringList(typeof(AssetSetting.LoadType));
            _loadType.index   = 0;
        }

        protected virtual void AddEvents()
        {
            _deleteBtn.clicked += OnDeleteClick;
            _platform.RegisterValueChangedCallback(OnPlatformChanged);
            _loadType.RegisterValueChangedCallback(OnLoadTypeChanged);
            _nameLabel.RegisterValueChangedCallback(OnLabelChanged);
            _buildPath.RegisterValueChangedCallback(OnLabelChanged);
            _remoteUrl.RegisterValueChangedCallback(OnLabelChanged);
        }

        protected virtual void OnLabelChanged(ChangeEvent<string> evt)
        {
            if (!(evt.target is TextField)) return;
            var label = evt.target as TextField;
            FileSaveUtils.SetDataProperty(_data, label.userData.ToString(), evt.newValue);
        }

        protected virtual void OnLoadTypeChanged(ChangeEvent<string> evt)
        {
            var i = MiscUtils.StringToEnum<AssetSetting.LoadType>(evt.newValue);
            if (i == -1) return;
            var p = (AssetSetting.LoadType) i;
            FileSaveUtils.SetDataProperty(_data, "loadType", p);
            updateUI();
        }

        protected virtual void OnPlatformChanged(ChangeEvent<string> evt)
        {
            var i = MiscUtils.StringToEnum<AssetSetting.BuildTarget>(evt.newValue);
            if (i == -1) return;
            var p = (AssetSetting.BuildTarget) i;
            FileSaveUtils.SetDataProperty(_data, "buildPlatform", p);
            updateUI();
        }

        protected virtual void OnDeleteClick()
        {
            if (EditorUtility.DisplayDialog("删除平台配置数据", $"是否确定删除平台配置数据  {_data.fileName} ？", "删除"))
            {
                AssetDatabase.DeleteAsset($"{WindowDefine.platformSettingPath}/{_data.fileName}.asset");
                AssetDatabase.SaveAssets();
            }
        }

        public override void SetData(object o)
        {
            if (!(o is AssetSetting)) return;
            _data = o as AssetSetting;
            updateUI();
        }

        protected virtual void updateUI()
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
    }
}