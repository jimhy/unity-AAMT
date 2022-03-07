using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAMT
{
    [CreateAssetMenu(fileName = "SettingManager", menuName = "AAMT/SettingManager", order = 0)]
    public class SettingManager : ScriptableObject
    {
        public AssetSetting assetSetting;
        private static SettingManager instance;

        public static AssetSetting AssetSetting
        {
            get
            {
                if (instance == null) LoadAssetSetting();
                return instance.assetSetting;
            }
        }

#if UNITY_EDITOR
        public static void ReloadAssetSetting()
        {
            LoadAssetSetting();
        }
#endif

        private static void LoadAssetSetting()
        {
#if !UNITY_EDITOR
            var sprite =
                AssetDatabase.LoadAssetAtPath<Object>("Assets/AAMT/Data/SettingManager.asset");
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", "Assets/AAMT/Data/SettingManager.asset");
            instance = Instantiate(sprite) as SettingManager;
            if (instance != null) instance.assetSetting.Init();
#else
            var path = $"{Application.streamingAssetsPath}/{AAMTDefine.AAMT_BUNDLE_NAME}".ToLower();
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", path);
            var bundle =
                AssetBundle.LoadFromFile(path);
            instance = bundle.LoadAsset<SettingManager>("SettingManager.asset");
            instance.assetSetting.Init();
#endif
        }
    }
}