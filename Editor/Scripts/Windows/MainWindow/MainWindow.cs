using AAMT;
using AAMT.Editor;
using Editor.Scripts.Windows.SettingPanel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class MainWindow : EditorWindow
    {
        private ListView _menusContainer;
        private VisualElement _contentContainer;

        [MenuItem("Window/AAMT")]
        public static void CreateMainWindow()
        {
            var wnd = GetWindow<MainWindow>();
            wnd.titleContent = new GUIContent("AAMT Settings");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathManager.MainPanelPath);
            visualTree.CloneTree(root);

            var menuTree = root.Q<MenuTree>("MenuTree");
            menuTree.AddItem("运行", new BuildSettingPanel(), Icons.PLAY);
            menuTree.AddItem("平台", new CreateSettingPanel(), Icons.PHONE).AddAllAssetsAtPath(WindowDefine.platformSettingPath, new PlatformSettingPanel(), typeof(AssetSetting));
            menuTree.AddItem("设置", new SettingPanel(), Icons.SETTING);
        }

        private void OnDestroy()
        {
            EditorCommon.EventBus.destory();
        }
    }
}