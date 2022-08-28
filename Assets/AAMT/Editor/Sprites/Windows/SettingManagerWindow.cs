using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace AAMT.Windos
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
            initSettingManager();
            base.OnEnable();
        }

        private void initSettingManager()
        {
            if (_settingManager == null)
            {
                var path = $"{WindowDefine.dataPath}/SettingManager.asset";
                _settingManager = AssetDatabase.LoadAssetAtPath<SettingManager>(path);
                if (_settingManager == null)
                {
                    _settingManager = new SettingManager();
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
        }

        [HideIf("buildTarget", AssetSetting.BuildTarget.editor)]
        [Button("打包资源")]
        private void BuildAssetBundle()
        {
            AssetsBundleBuilder.BuildAssetsBundles();
        }
    }
}