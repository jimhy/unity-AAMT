using UnityEngine;

namespace AAMT
{
    public class AAMTDefine
    {
        public const string AAMT_BUNDLE_FILES_DICTIONARY = "bundle-files-dictionary.txt";
        public const string AAMT_ASSETS_WITH_BUNDLE_NAME = "assets-with-bundle.txt";
        public const string AAMT_SETTING_MANAGER = "Assets/AAMT/Data/SettingManager.asset";
        public const string AAMT_BUNDLE_NAME = "aamt.ab";
        public const string AAMT_ASSET_VERSION = "version.json";
        public const string AAMT_MOVE_FILES_TO_PERSISTENT_COMPLETE_KEY = "moveFilesToPersistentCompleteKey";
        public static string AAMT_PERSISTENT_VERSION_PATH = $"{Application.persistentDataPath}/{SettingManager.assetSetting.BuildPlatform}/{AAMT_ASSET_VERSION}";
        public static string AAMT_BUNDLE_PACKAGE_DATA   = "Assets/AAMT/Data/BundlePackageData.json";
        public static bool IsMoveFilesToPersistentComplete()
        {
            return PlayerPrefs.HasKey(AAMT_MOVE_FILES_TO_PERSISTENT_COMPLETE_KEY);
        }

        public static void SetMoveFilesToPersistentComplete(bool v)
        {
            if (v) PlayerPrefs.SetInt(AAMT_MOVE_FILES_TO_PERSISTENT_COMPLETE_KEY, 1);
            else PlayerPrefs.DeleteKey(AAMT_MOVE_FILES_TO_PERSISTENT_COMPLETE_KEY);
        }
    }
}