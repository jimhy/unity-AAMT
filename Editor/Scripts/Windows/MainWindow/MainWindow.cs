using AAMT;
using AAMT.Editor;
using Editor.Scripts.Windows.SettingPanel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EventDispatcher = AAMT.Editor.EventDispatcher;

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

        [MenuItem("AAMT/CloseMainWindow")]
        public static void CloseMainWindow()
        {
            var wnd = GetWindow<MainWindow>();
            wnd.Close();
        }

        public void CreateGUI()
        {
            EditorCommon.EventBus = new EventDispatcher();

            VisualElement root = rootVisualElement;

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
            EditorCommon.EventBus = null;
        }
    }
}