using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class PlatformSettingPanel : ContentNode
    {
        protected ToolbarButton _deleteBtn;
        protected TextField _nameLabel;
        protected DropdownField _platform;
        protected TextField _buildPath;
        protected DropdownField _loadType;
        protected TextField _remoteUrl;
        protected VisualElement _bottom;
        protected AssetSetting _data;
        protected FolderFieldList _assetsFolders;
        protected TextField _macro;

        public PlatformSettingPanel()
        {
            var uml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathManager.PlatformSettingPanelPath);
            uml.CloneTree(this);

            InitElements();
            AddEvents();
        }

        protected virtual void InitElements()
        {
            _deleteBtn = this.Q<ToolbarButton>("DeleteBtn");
            _nameLabel = this.Q<TextField>("NameLabel");
            _platform = this.Q<DropdownField>("Platform");
            _buildPath = this.Q<TextField>("BuildPath");
            _loadType = this.Q<DropdownField>("LoadType");
            _remoteUrl = this.Q<TextField>("RemoteUrl");
            _bottom = this.Q<VisualElement>("Bottom");
            _assetsFolders = this.Q<FolderFieldList>("AssetsFolders");
            _macro = this.Q<TextField>("Macro");

            _nameLabel.userData = "fileName";
            _buildPath.userData = "buildPath";
            _remoteUrl.userData = "remotePath";

            InitElementData();
        }

        protected virtual void InitElementData()
        {
            _platform.choices = MiscUtils.EnumToStringList(typeof(AssetSetting.BuildTarget));
            _platform.index = 0;
            _buildPath.value = "{UnityEngine.Application.dataPath}/../Build/";
            _remoteUrl.value = "http://localhost:80";
            _loadType.choices = MiscUtils.EnumToStringList(typeof(AssetSetting.LoadType));
            _loadType.index = 0;
            _macro.value = "";
        }

        protected virtual void AddEvents()
        {
            _deleteBtn.clicked += OnDeleteClick;
            _platform.RegisterCallback<ChangeEvent<string>>(OnPlatformChanged);
            _loadType.RegisterCallback<ChangeEvent<string>>(OnLoadTypeChanged);
            _nameLabel.RegisterCallback<FocusOutEvent>(OnNameChanged);
            _buildPath.RegisterCallback<FocusOutEvent>(OnBuildPathChanged);
            _remoteUrl.RegisterCallback<FocusOutEvent>(OnRemotePathChanged);
            _macro.RegisterCallback<FocusOutEvent>(OnMacroChanged);
            _assetsFolders.OnValueChanged = OnFoldersChanged;
        }

        protected virtual void RemoveEvents()
        {
            _deleteBtn.clicked -= OnDeleteClick;
            _platform.UnregisterValueChangedCallback(OnPlatformChanged);
            _loadType.UnregisterValueChangedCallback(OnLoadTypeChanged);
            _nameLabel.UnregisterCallback<FocusOutEvent>(OnNameChanged);
            _buildPath.UnregisterCallback<FocusOutEvent>(OnBuildPathChanged);
            _remoteUrl.UnregisterCallback<FocusOutEvent>(OnRemotePathChanged);
            _macro.UnregisterCallback<FocusOutEvent>(OnMacroChanged);
            _assetsFolders.OnValueChanged = null;
        }

        private void OnNameChanged(FocusOutEvent evt)
        {
            if (evt.target is not TextField label || _data.name == label.value) return;
            var filePath = $"{WindowDefine.platformSettingPath}/{_data.name}.asset";
            var labelText = label.value;
            var newName = $"{labelText}.asset";
            _data.name = label.value;
            AssetDatabase.RenameAsset(filePath, newName);
        }

        private void OnBuildPathChanged(FocusOutEvent evt)
        {
            if (evt.target is not TextField label || _data.BuildPathSetting == label.value) return;
            _data.BuildPathSetting = label.value;
            _data.Save();
        }

        private void OnRemotePathChanged(FocusOutEvent evt)
        {
            if (evt.target is not TextField label || _data.RemotePath == label.value) return;
            _data.RemotePath = label.value;
            _data.Save();
        }

        private void OnMacroChanged(FocusOutEvent evt)
        {
            if (evt.target is not TextField label || _data.Macro == label.value) return;
            var lastMacro = _data.Macro;
            _data.Macro = label.value;
            _data.Save();
            SettingManager.Instance.OnMacroChanged(_data,lastMacro);
        }

        private void OnFoldersChanged()
        {
            _data.MoveToStreamingAssetsPathList = _assetsFolders.Data;
            _data.Save();
        }

        protected virtual void OnLoadTypeChanged(ChangeEvent<string> evt)
        {
            var i = MiscUtils.StringToEnum<AssetSetting.LoadType>(evt.newValue);
            if (i == -1) return;
            _data.CurrentLoadType = (AssetSetting.LoadType)i;
            _data.Save();
            UpdateUI();
        }

        protected virtual void OnPlatformChanged(ChangeEvent<string> evt)
        {
            var i = MiscUtils.StringToEnum<AssetSetting.BuildTarget>(evt.newValue);
            if (i == -1) return;
            _data.BuildPlatform = (AssetSetting.BuildTarget)i;
            _data.Save();
            UpdateUI();
        }

        protected virtual void OnDeleteClick()
        {
            if (EditorUtility.DisplayDialog("删除平台配置数据", $"是否确定删除平台配置数据  {_data.name} ？", "删除"))
            {
                var path = AssetDatabase.GUIDToAssetPath(_data.Guid);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }

        public override void SetData(object o)
        {
            if (!(o is AssetSetting)) return;
            RemoveEvents();
            _data = o as AssetSetting;
            _assetsFolders.Data = _data.MoveToStreamingAssetsPathList;
            UpdateUI();
            EditorCommon.DelayCallBack(.01f, _ => AddEvents());
        }

        protected virtual void UpdateUI()
        {
            _nameLabel.value = _data.name;
            _platform.value = _data.BuildPlatform.ToString();
            _loadType.value = _data.CurrentLoadType.ToString();
            _remoteUrl.value = _data.RemotePath;
            _macro.value = _data.Macro;
            var displayType1 = DisplayStyle.Flex;
            var displayType2 = DisplayStyle.Flex;

            if (_data.BuildPlatform == AssetSetting.BuildTarget.editor) displayType1 = DisplayStyle.None;
            if (_data.BuildPlatform == AssetSetting.BuildTarget.editor || _data.CurrentLoadType == AssetSetting.LoadType.Local) displayType2 = DisplayStyle.None;

            _buildPath.style.display = displayType1;
            _loadType.style.display = displayType1;
            _remoteUrl.style.display = displayType1;
            _bottom.style.display = displayType2;
        }
    }
}