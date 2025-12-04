using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace AAMT.Editor
{
    public class BuildSettingPanel : ContentNode
    {
        private SettingManager _settingManager;
        private ObjectField _editorAssetSetting;
        private ObjectField _windowsAssetSetting;
        private ObjectField _androidAssetSetting;
        private ObjectField _iosAssetSetting;
        private VisualElement _bottom;
        private Button _buildAssetsBtn;
        private Button _moveNeedAssetToStreamingAssetsFolderBtn;
        private Button _moveAllAssetToStreamingAssetsFolderBtn;

        private DropdownField _buildTarget;

        public BuildSettingPanel()
        {
            CreateElements();
            InitData();
        }

        private void InitData()
        {
            _settingManager = SettingManager.Instance;
            _buildTarget.index = MiscUtils.StringToEnum<AssetSetting.BuildTarget>(_settingManager.BuildTarget.ToString());
            _androidAssetSetting.value = _settingManager.AndroidAssetSetting;
            _iosAssetSetting.value = _settingManager.IosAssetSetting;
            _windowsAssetSetting.value = _settingManager.WindowsAssetSetting;
            _editorAssetSetting.value = _settingManager.EditorAssetSetting;

            UpdateBottomDisplay();
        }

        private void CreateElements()
        {
            _windowsAssetSetting = new ObjectField();
            _androidAssetSetting = new ObjectField();
            _iosAssetSetting = new ObjectField();
            _buildTarget = new DropdownField();
            _editorAssetSetting = new ObjectField();

            _bottom = new VisualElement();
            _buildAssetsBtn = new Button();
            _moveNeedAssetToStreamingAssetsFolderBtn = new Button();
            _moveAllAssetToStreamingAssetsFolderBtn = new Button();

            Add(_editorAssetSetting);
            Add(_windowsAssetSetting);
            Add(_androidAssetSetting);
            Add(_iosAssetSetting);
            Add(_buildTarget);
            Add(_bottom);
            _bottom.Add(_buildAssetsBtn);
            _bottom.Add(_moveNeedAssetToStreamingAssetsFolderBtn);
            _bottom.Add(_moveAllAssetToStreamingAssetsFolderBtn);

            _windowsAssetSetting.objectType = typeof(AssetSetting);
            _androidAssetSetting.objectType = typeof(AssetSetting);
            _iosAssetSetting.objectType = typeof(AssetSetting);
            _editorAssetSetting.objectType = typeof(AssetSetting);

            _editorAssetSetting.label = "Editor平台配置";
            _windowsAssetSetting.label = "Windows平台配置";
            _androidAssetSetting.label = "安卓平台配置";
            _iosAssetSetting.label = "IOS平台配置";
            _buildTarget.label = "当前平台类型";

            _buildAssetsBtn.text = "打包资源";
            _moveNeedAssetToStreamingAssetsFolderBtn.text = "移动所需资源到Streaming Assets 文件夹";
            _moveAllAssetToStreamingAssetsFolderBtn.text = "移动所有资源到Streaming Assets 文件夹";

            _windowsAssetSetting.name = "windows";
            _androidAssetSetting.name = "android";
            _iosAssetSetting.name = "ios";


            _buildTarget.choices = MiscUtils.EnumToStringList(typeof(AssetSetting.BuildTarget));
            _buildTarget.index = 0;

            AddEvents();
        }

        private void AddEvents()
        {
            _buildTarget.RegisterValueChangedCallback(OnBuildTargetChanged);
            _editorAssetSetting.RegisterValueChangedCallback(OnSettingDataChanged);
            _windowsAssetSetting.RegisterValueChangedCallback(OnSettingDataChanged);
            _androidAssetSetting.RegisterValueChangedCallback(OnSettingDataChanged);
            _iosAssetSetting.RegisterValueChangedCallback(OnSettingDataChanged);

            _buildAssetsBtn.clicked += BuildAssetBundle;
            _moveNeedAssetToStreamingAssetsFolderBtn.clicked += MoveBundleToStreamingAssets;
            _moveAllAssetToStreamingAssetsFolderBtn.clicked += MoveAllBundleToStreamingAssets;
        }

        private void OnSettingDataChanged(ChangeEvent<Object> evt)
        {
            AssetSetting result = evt.newValue as AssetSetting;

            if (evt.target == _windowsAssetSetting) _settingManager.WindowsAssetSetting = result;
            else if (evt.target == _androidAssetSetting) _settingManager.AndroidAssetSetting = result;
            else if (evt.target == _iosAssetSetting) _settingManager.IosAssetSetting = result;
            else if (evt.target == _editorAssetSetting) _settingManager.EditorAssetSetting = result;
        }

        private void OnBuildTargetChanged(ChangeEvent<string> evt)
        {
            _buildTarget.index = MiscUtils.StringToEnum<AssetSetting.BuildTarget>(evt.newValue);
            _settingManager.BuildTarget = (AssetSetting.BuildTarget)_buildTarget.index;
            UpdateBottomDisplay();
            SettingManager.ReloadAssetSetting();
            _settingManager.SetMacro();
        }

        private void UpdateBottomDisplay()
        {
            _bottom.visible = _settingManager.BuildTarget != AssetSetting.BuildTarget.editor;
        }

        private void BuildAssetBundle()
        {
            AssetsBundleBuilder.BuildAssetsBundles();
        }

        private void MoveBundleToStreamingAssets()
        {
            SettingManager.ReloadAssetSetting(EditorCommon.EditorToAamtTarget());
            OnPreprocessHandler.MoveBundleToStreamingAssets();
            OnPreprocessHandler.CreateStreamingAssetsVersionData();
            OnPreprocessHandler.CreateBundleFilesDictionary();
            AssetDatabase.Refresh();
        }

        private void MoveAllBundleToStreamingAssets()
        {
            SettingManager.ReloadAssetSetting(EditorCommon.EditorToAamtTarget());
            OnPreprocessHandler.MoveAllBundleToStreamingAssets();
            OnPreprocessHandler.CreateBundleFilesDictionary();
            AssetDatabase.Refresh();
        }
    }
}