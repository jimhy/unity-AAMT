using UnityEditor;
using UnityEngine;

namespace AAMT
{
    [CreateAssetMenu(fileName = "SettingManager", menuName = "AAMT/SettingManager", order = 0)]
    public class SettingManager : ScriptableObject
    {
        public AssetSetting editorAssetSetting;
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
            var sprite = AssetDatabase.LoadAssetAtPath<Object>("Assets/AAMT/Data/SettingManager.asset");
            _instance = Instantiate(sprite) as SettingManager;
            if (buildTarget != null) _instance.buildTarget = buildTarget.Value;
            if (_instance != null)
            {
                switch (_instance.buildTarget)
                {
                    case AssetSetting.BuildTarget.editor:
                        _instance._currentAssetSetting = _instance.editorAssetSetting;
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

                _instance._currentAssetSetting.Init();
            }
#else
            var path =
                $"{Application.streamingAssetsPath}/{Tools.PlatformToBuildTarget()}/{AAMTDefine.AAMT_BUNDLE_NAME}"
                    .ToLower();
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", path);
            var bundle = AssetBundle.LoadFromFile(path);
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