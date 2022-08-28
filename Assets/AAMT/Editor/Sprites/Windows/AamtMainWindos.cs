using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AAMT.Windos
{
    public class AamtMainWindos : OdinMenuEditorWindow
    {
        private SettingManagerWindow     _settingManager;
        private AssetBundleSettingWindow _abWindow;

        [MenuItem("AAMT/Settings")]
        private static void OpenWindow()
        {
            var window = GetWindow<AamtMainWindos>();
            window.position          = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
            window.titleContent.text = "AAMT Settings";
            var platformSettingPath = WindowDefine.platformSettingPath;
            if (!Directory.Exists(platformSettingPath)) Directory.CreateDirectory(platformSettingPath);
            AssetDatabase.Refresh();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            initSettingManager();
            initAssetBundlerPanel();

            OdinMenuTree tree = new OdinMenuTree()
            {
                {"平台设置", null, EditorIcons.SmartPhone},
                {"打包设置", _settingManager, EditorIcons.SettingsCog},
                {"AB资源设置", _abWindow, EditorIcons.HamburgerMenu},
            };

            tree.AddAllAssetsAtPath("平台设置", WindowDefine.platformSettingPath, typeof(AssetSetting));
            tree.Add("平台设置/创建新平台", new CreateSettings());

            return tree;
        }

        private void initAssetBundlerPanel()
        {
            if (_abWindow == null) _abWindow = new AssetBundleSettingWindow();
        }

        private void initSettingManager()
        {
            if (_settingManager == null) _settingManager = new SettingManagerWindow();
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            var selected = MenuTree.Selection;
            if (!(selected.SelectedValue is AssetSetting)) return;
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("删除"))
                {
                    var data = selected.SelectedValue as AssetSetting;
                    var b    = EditorUtility.DisplayDialog("温馨提示", $"是否删除{data.name}", "确定", "取消");
                    if (b)
                    {
                        var path = AssetDatabase.GetAssetPath(data);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        protected override void OnEndDrawEditors()
        {
            base.OnEndDrawEditors();
            var selected = MenuTree.Selection;
            if (!(selected.SelectedValue is AssetSetting)) return;
            var assetSetting = selected.SelectedValue as AssetSetting;
            if (assetSetting.GetBuildPlatform == AssetSetting.BuildTarget.editor) return;
            var buildPath = AAMTRuntimeProperties.EvaluateString(assetSetting.WindowGetSourceBuildPath);
            if (assetSetting.getLoadType == AssetSetting.LoadType.Remote)
            {
                if (!WindowDefine.httpRegex.IsMatch(assetSetting.getRemotePath))
                {
                    EditorGUILayout.HelpBox("远程加载必须要填写远程服务器路径地址,需要带http(s)://", MessageType.Error);
                }
            }
        }

        private void OnLostFocus()
        {
            if (MenuTree == null) return;
            var selected = MenuTree.Selection;
            if (!(selected.SelectedValue is AssetSetting)) return;
            var assetSetting = selected.SelectedValue as AssetSetting;
            var path         = AssetDatabase.GetAssetPath(assetSetting);
            var fileName     = Path.GetFileName(path);
            if (fileName != assetSetting.name)
            {
                AssetDatabase.RenameAsset(path, assetSetting.name);
                AssetDatabase.SaveAssets();
            }
        }

        private void MoveAb(AssetSetting.BuildTarget target)
        {
            SettingManager.ReloadAssetSetting(target);
            OnPreprocessHandler.MoveBundleToStreamingAssets();
            OnPreprocessHandler.CreateBundleFilesDictionary();
            OnPreprocessHandler.CreateStreamingAssetsVersionData();
            AssetDatabase.Refresh();
        }

        private void MoveAllAb(AssetSetting.BuildTarget target)
        {
            SettingManager.ReloadAssetSetting(target);
            OnPreprocessHandler.MoveAllBundleToStreamingAssets();
            AssetDatabase.Refresh();
        }

        public class CreateSettings
        {
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public AssetSetting assetSetting;

            public CreateSettings()
            {
                assetSetting      = new AssetSetting();
                assetSetting.name = "New Platform";
            }

            [Button("创建")]
            private void Create()
            {
                AssetDatabase.CreateAsset(assetSetting,
                    $"{WindowDefine.platformSettingPath}/{assetSetting.name}.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}