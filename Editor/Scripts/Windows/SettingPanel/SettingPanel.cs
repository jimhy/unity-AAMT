using AAMT.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Scripts.Windows.SettingPanel
{
    public class SettingPanel : ContentNode
    {
        private Toggle _showFolder;

        public SettingPanel()
        {
            var uml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathManager.SettingPanelPath);
            uml.CloneTree(this);

            initElements();
            AddEvents();
        }

        private void initElements()
        {
            _showFolder       = this.Q<Toggle>("ShowFloderIcon");
            _showFolder.value = EditorCommon.ShowFolderIcon;
        }

        private void AddEvents()
        {
            _showFolder.RegisterValueChangedCallback(OnShowFolderChanged);
        }

        private void OnShowFolderChanged(ChangeEvent<bool> evt)
        {
            EditorCommon.ShowFolderIcon = evt.newValue;
        }
    }
}