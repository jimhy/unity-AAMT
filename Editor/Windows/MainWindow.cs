using AAMT.Editor.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class MainWindow : EditorWindow
    {
        private ListView _menusContainer;
        private VisualElement _contentContainer;

        [MenuItem("AAMT/MainWindow")]
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
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathManager.MainPanelPath);
            visualTree.CloneTree(root);
        }
    }
}