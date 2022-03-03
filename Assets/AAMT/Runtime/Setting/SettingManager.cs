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
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/AAMT/Data/SettingManager.asset");
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", "Assets/AAMT/Data/SettingManager.asset");
            instance = ScriptableObject.Instantiate(sprite) as SettingManager;
            if (instance != null) instance.assetSetting.Init();
#else
            var buildTag = GetBuildTagByCurrentPlatform();
            if (buildTag == string.Empty)
            {
                Debug.LogErrorFormat("当前平台不支持:{0}", Application.platform.ToString());
                return;
            }

            var path = $"{Application.streamingAssetsPath}/{buildTag}/BuildSetting.asset.ab".ToLower();
            Debug.LogFormat("Load buildSetting.assets bundle.path={0}", path);
            var bundle =
                AssetBundle.LoadFromFile(path);
            instance = bundle.LoadAsset<SettingManager>("buildsetting.asset");
            instance.assetSetting.Init();
#endif
        }


        private static string GetBuildTagByCurrentPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return AssetSetting.BuildTarget.android.ToString();
                case RuntimePlatform.IPhonePlayer:
                    return AssetSetting.BuildTarget.ios.ToString();
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return AssetSetting.BuildTarget.windows.ToString();
            }

            return string.Empty;
        }
    }
}