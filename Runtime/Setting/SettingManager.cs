using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    public class SettingManager : ScriptableObject
    {
        [SerializeField]
        private AssetSetting editorAssetSetting;

        [SerializeField]
        private AssetSetting windowsAssetSetting;

        [SerializeField]
        private AssetSetting androidAssetSetting;

        [SerializeField]
        private AssetSetting iosAssetSetting;

        [SerializeField]
        private AssetSetting.BuildTarget buildTarget = AssetSetting.BuildTarget.editor;

        public AssetSetting EditorAssetSetting
        {
            get => editorAssetSetting;
            set
            {
                editorAssetSetting = value;
                Save();
            }
        }

        public AssetSetting WindowsAssetSetting
        {
            get => windowsAssetSetting;
            set
            {
                windowsAssetSetting = value;
                Save();
            }
        }

        public AssetSetting AndroidAssetSetting
        {
            get => androidAssetSetting;
            set
            {
                androidAssetSetting = value;
                Save();
            }
        }

        public AssetSetting IosAssetSetting
        {
            get => iosAssetSetting;
            set
            {
                iosAssetSetting = value;
                Save();
            }
        }

        public AssetSetting.BuildTarget BuildTarget
        {
            get => buildTarget;
            set
            {
                buildTarget = value;
                Save();
            }
        }

        private AssetSetting _currentAssetSetting;

        private static SettingManager _instance;

        public static SettingManager Instance
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
            LoadForEditor(buildTarget);
#else
            LoadForRuntime(buildTarget);
#endif
        }
#if UNITY_EDITOR
        private static void LoadForEditor(AssetSetting.BuildTarget? buildTarget = null)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<SettingManager>(AAMTDefine.AAMT_SETTING_MANAGER);
            if (sprite != null)
            {
                _instance = Instantiate(sprite);
            }
            else
            {
                var sm = CreateInstance<SettingManager>();
                var path = Path.GetDirectoryName(AAMTDefine.AAMT_SETTING_MANAGER);
                if (!Directory.Exists(path) && !string.IsNullOrEmpty(path)) Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(sm, AAMTDefine.AAMT_SETTING_MANAGER);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                _instance = sm;
            }

            if (buildTarget != null) _instance.BuildTarget = buildTarget.Value;

            switch (_instance.BuildTarget)
            {
                case AssetSetting.BuildTarget.editor:
                    _instance._currentAssetSetting = _instance.EditorAssetSetting;
                    break;
                case AssetSetting.BuildTarget.windows:
                    _instance._currentAssetSetting = _instance.WindowsAssetSetting;
                    break;
                case AssetSetting.BuildTarget.android:
                    _instance._currentAssetSetting = _instance.AndroidAssetSetting;
                    break;
                case AssetSetting.BuildTarget.ios:
                    _instance._currentAssetSetting = _instance.IosAssetSetting;
                    break;
            }

            if (_instance._currentAssetSetting == null)
            {
                _instance._currentAssetSetting = CreateInstance<AssetSetting>();
                Debug.LogError("当前平台没有设置配置文件，请在AAMT->Settings->打包设置->配置当前平台对应的平台资源配置，如果还没有创建平台设置，请在平台设置->创建新平台，进行创建相应的平台配置。");
            }


            _instance._currentAssetSetting.Init();
        }


        public void OnMacroChanged(AssetSetting data,string lastMacro)
        {
            if (data == null || _currentAssetSetting.name != data.name) return;
            if (!string.IsNullOrEmpty(lastMacro))
            {
                var macroList = lastMacro.Split(';');
                var list = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');
                var list1 = list.Except(macroList);
                var enumerable = list1 as string[] ?? list1.ToArray();
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", enumerable));
            }
            SetMacro();
        }
        
        public void SetMacro()
        {
            var macroList = new List<string>();
            string[] strList;
            if (_instance.EditorAssetSetting != null && !string.IsNullOrEmpty(_instance.EditorAssetSetting.Macro))
            {
                strList = _instance.EditorAssetSetting.Macro.Split(';');
                macroList.AddRange(strList.Where(s => !string.IsNullOrEmpty(s)));
            }

            if (_instance.WindowsAssetSetting != null && !string.IsNullOrEmpty(_instance.WindowsAssetSetting.Macro))
            {
                strList = _instance.WindowsAssetSetting.Macro.Split(';');
                macroList.AddRange(strList.Where(s => !string.IsNullOrEmpty(s)));
            }

            if (_instance.IosAssetSetting != null && !string.IsNullOrEmpty(_instance.IosAssetSetting.Macro))
            {
                strList = _instance.IosAssetSetting.Macro.Split(';');
                macroList.AddRange(strList.Where(s => !string.IsNullOrEmpty(s)));
            }

            if (_instance.AndroidAssetSetting != null && !string.IsNullOrEmpty(_instance.AndroidAssetSetting.Macro))
            {
                strList = _instance.AndroidAssetSetting.Macro.Split(';');
                macroList.AddRange(strList.Where(s => !string.IsNullOrEmpty(s)));
            }

            var currentMacroList = new List<string>();
            strList = _instance._currentAssetSetting != null && !string.IsNullOrEmpty(_instance._currentAssetSetting.Macro) ? _instance._currentAssetSetting.Macro.Split(';') : Array.Empty<string>();
            currentMacroList.AddRange(strList.Where(s => !string.IsNullOrEmpty(s)));
            var list = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');
            var list1 = list.Except(macroList);
            var enumerable = list1 as string[] ?? list1.ToArray();
            currentMacroList.AddRange(enumerable);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", currentMacroList));
        }
#endif
#if !UNITY_EDITOR
        private static void LoadForRuntime(AssetSetting.BuildTarget? buildTarget1)
        {
            var path = $"{Tools.PlatformToBuildTarget()}/{AAMTDefine.AAMT_BUNDLE_NAME}".ToLower();
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
                        _instance._currentAssetSetting = _instance.WindowsAssetSetting;
                        break;
                    case RuntimePlatform.Android:
                        _instance._currentAssetSetting = _instance.AndroidAssetSetting;
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        _instance._currentAssetSetting = _instance.IosAssetSetting;
                        break;
                }

                _instance._currentAssetSetting.Init();
            }
        }
#endif

        private void Save()
        {
#if UNITY_EDITOR
            var instance = CreateInstance<SettingManager>();
            instance.iosAssetSetting = iosAssetSetting;
            instance.androidAssetSetting = androidAssetSetting;
            instance.windowsAssetSetting = windowsAssetSetting;
            instance.editorAssetSetting = editorAssetSetting;
            instance.buildTarget = buildTarget;

            AssetDatabase.CreateAsset(instance, AAMTDefine.AAMT_SETTING_MANAGER);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

    }
}