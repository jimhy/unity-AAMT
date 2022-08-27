using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static string dataPath = "Assets/AAMT/Data";
        private static string platformSettingPath = $"{dataPath}/Platforms";

        [MenuItem("AAMT/Settings")]
        private static void OpenWindow()
        {
            var window = GetWindow<AamtMainWindos>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
            window.titleContent.text = "AAMT Settings";
            if (!Directory.Exists(platformSettingPath)) Directory.CreateDirectory(platformSettingPath);
            AssetDatabase.Refresh();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree()
            {
                {"平台设置", null, EditorIcons.SmartPhone},
            };

            tree.AddAllAssetsAtPath("平台设置", platformSettingPath, typeof(AssetSetting));
            tree.Add("平台设置/创建新平台", new CreateSettings());
            tree.SortMenuItemsByName();

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            var selected = MenuTree.Selection;
            if(!(selected.SelectedValue is AssetSetting)) return;
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("删除"))
                {
                    var data = selected.SelectedValue as AssetSetting;
                    var b = EditorUtility.DisplayDialog("温馨提示", $"是否删除{data.name}", "确定", "取消");
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

        public class CreateSettings
        {
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public AssetSetting assetSetting;

            public CreateSettings()
            {
                assetSetting = new AssetSetting();
                assetSetting.name = "New Platform";
            }

            [Button("创建")]
            private void Create()
            {
                AssetDatabase.CreateAsset(assetSetting, $"{platformSettingPath}/{assetSetting.name}.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}