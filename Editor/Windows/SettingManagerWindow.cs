using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace AAMT.Editor.Windows
{
    public class SettingManagerWindow : OdinEditorWindow
    {
        private SettingManager _settingManager;

        [LabelText("window平台资源配置")]
        [OnValueChanged("onValueChanged")]
        public AssetSetting windowsAssetSetting;

        [LabelText("安卓平台资源配置")]
        [OnValueChanged("onValueChanged")]
        public AssetSetting androidAssetSetting;

        [LabelText("IOS平台资源配置")]
        [OnValueChanged("onValueChanged")]
        public AssetSetting iosAssetSetting;

        [LabelText("当前平台类型")]
        [OnValueChanged("onValueChanged")]
        public AssetSetting.BuildTarget buildTarget = AssetSetting.BuildTarget.editor;

        protected override void OnEnable()
        {
            InitSettingManager();
            base.OnEnable();
        }

        private void InitSettingManager()
        {
            if (_settingManager == null)
            {
                var path = $"{WindowDefine.dataPath}/SettingManager.asset";
                _settingManager = AssetDatabase.LoadAssetAtPath<SettingManager>(path);
                if (_settingManager == null)
                {
                    _settingManager = CreateInstance<SettingManager>();
                    AssetDatabase.CreateAsset(_settingManager, path);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    buildTarget         = _settingManager.buildTarget;
                    androidAssetSetting = _settingManager.androidAssetSetting;
                    iosAssetSetting     = _settingManager.iosAssetSetting;
                    windowsAssetSetting = _settingManager.windowsAssetSetting;
                }
            }
        }

        private void onValueChanged()
        {
            _settingManager.buildTarget         = buildTarget;
            _settingManager.androidAssetSetting = androidAssetSetting;
            _settingManager.iosAssetSetting     = iosAssetSetting;
            _settingManager.windowsAssetSetting = windowsAssetSetting;
            SettingManager.ReloadAssetSetting(buildTarget);
        }

        [HideIf("buildTarget", AssetSetting.BuildTarget.editor)]
        [Button("打包资源")]
        private void BuildAssetBundle()
        {
            AssetsBundleBuilder.BuildAssetsBundles();
        }

        [HideIf("buildTarget", AssetSetting.BuildTarget.editor)]
        [Button("移动所需资源到Streaming Assets 文件夹")]
        private void MoveBundleToStreamingAssets()
        {
            SettingManager.ReloadAssetSetting(EditorCommon.EditorToAamtTarget());
            OnPreprocessHandler.MoveBundleToStreamingAssets();
            OnPreprocessHandler.CreateStreamingAssetsVersionData();
            OnPreprocessHandler.CreateBundleFilesDictionary();
            AssetDatabase.Refresh();
        }

        [HideIf("buildTarget", AssetSetting.BuildTarget.editor)]
        [Button("移动所有资源到Streaming Assets 文件夹")]
        private void MoveAllBundleToStreamingAssets()
        {
            SettingManager.ReloadAssetSetting(EditorCommon.EditorToAamtTarget());
            OnPreprocessHandler.MoveAllBundleToStreamingAssets();
            AssetDatabase.Refresh();
        }
    }
}