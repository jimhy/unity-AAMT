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
#if UNITY_EDITOR
                if (Application.isPlaying && instance == null || !Application.isPlaying)
                {
                    LoadAssetSetting();
                }
#else
                if (instance == null) LoadAssetSetting();
#endif

                return instance.assetSetting;
            }
        }

        private static void LoadAssetSetting()
        {
#if UNITY_EDITOR
            var sprite =
                AssetDatabase.LoadAssetAtPath<Object>("Assets/AAMT/Data/SettingManager.asset");
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", "Assets/AAMT/Data/SettingManager.asset");
            instance = Instantiate(sprite) as SettingManager;
            if (instance != null) instance.assetSetting.Init();
#else

            var path = $"{Application.streamingAssetsPath}/aamt.ab".ToLower();
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", path);
            var bundle =
                AssetBundle.LoadFromFile(path);
            instance = bundle.LoadAsset<SettingManager>("SettingManager.asset");
            instance.assetSetting.Init();
#endif
        }
    }
}