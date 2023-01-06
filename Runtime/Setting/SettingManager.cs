using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace AAMT
{
    public class SettingManager : ScriptableObject
    {
        public AssetSetting windowsAssetSetting;
        public AssetSetting androidAssetSetting;
        public AssetSetting iosAssetSetting;
        public AssetSetting.BuildTarget buildTarget = AssetSetting.BuildTarget.editor;

        private AssetSetting _currentAssetSetting;

        private static SettingManager _instance;

        internal static SettingManager Instance
        {
            get
            {
                if (_instance == null) LoadAssetSetting();
                return _instance;
            }
        }

        public static AssetSetting assetSetting => Instance._currentAssetSetting;
#if UNITY_EDITOR
        public static void ReloadAssetSetting(AssetSetting.BuildTarget? buildTarget = null)
        {
            LoadAssetSetting(buildTarget);
        }
#endif

        private static void LoadAssetSetting(AssetSetting.BuildTarget? buildTarget = null)
        {
#if UNITY_EDITOR

            var sprite = AssetDatabase.LoadAssetAtPath<Object>(AAMTDefine.AAMT_SETTING_MANAGER);
            if (sprite == null)
            {
                var sm   = CreateInstance<SettingManager>();
                var path = Path.GetDirectoryName(AAMTDefine.AAMT_SETTING_MANAGER);
                if (!Directory.Exists(path) && !string.IsNullOrEmpty(path)) Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(sm, AAMTDefine.AAMT_SETTING_MANAGER);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                _instance = sm;
            }
            else
            {
                _instance = Instantiate(sprite) as SettingManager;
            }

            if (_instance != null)
            {
                if (buildTarget != null) _instance.buildTarget = buildTarget.Value;

                switch (_instance.buildTarget)
                {
                    case AssetSetting.BuildTarget.editor:
                        _instance._currentAssetSetting = ScriptableObject.CreateInstance<AssetSetting>();
                        break;
                    case AssetSetting.BuildTarget.windows:
                        _instance._currentAssetSetting = _instance.windowsAssetSetting;
                        break;
                    case AssetSetting.BuildTarget.android:
                        _instance._currentAssetSetting = _instance.androidAssetSetting;
                        break;
                    case AssetSetting.BuildTarget.ios:
                        _instance._currentAssetSetting = _instance.iosAssetSetting;
                        break;
                }

                if (_instance._currentAssetSetting == null)
                {
                    _instance._currentAssetSetting = ScriptableObject.CreateInstance<AssetSetting>();
                    Debug.LogError("当前平台没有设置配置文件，请在AAMT->Settings->打包设置->配置当前平台对应的平台资源配置，如果还没有创建平台设置，请在平台设置->创建新平台，进行创建相应的平台配置。");
                }

                _instance._currentAssetSetting.Init();
            }
#else
            var path =
                $"{Tools.PlatformToBuildTarget()}/{AAMTDefine.AAMT_BUNDLE_NAME}"
                    .ToLower();
            path = $"{Application.streamingAssetsPath}/{path}";
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", path);
            var bundle = Tools.LoadBundle(path);
            if (bundle == null)
            {
                Debug.LogErrorFormat("Load AAMT bundle Faile!!path:{0}", path);
                return;
            }

            _instance = bundle.LoadAsset<SettingManager>("SettingManager.asset");
            if (_instance != null)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer:
                        _instance._currentAssetSetting = _instance.windowsAssetSetting;
                        break;
                    case RuntimePlatform.Android:
                        _instance._currentAssetSetting = _instance.androidAssetSetting;
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        _instance._currentAssetSetting = _instance.iosAssetSetting;
                        break;
                }

                _instance._currentAssetSetting.Init();
            }
#endif
        }
    }
}