using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor.Components
{
    public class MenuTree : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<MenuTree, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        private VisualElement _leftContainer;
        private VisualElement _rightContainer;
        private VisualElement _lastNode;

        public MenuTree()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PathManager.MenuTreeUssPath);
            styleSheets.Add(styleSheet);

            var root = new VisualElement();
            root.name            = "Root";
            _leftContainer       = new ScrollView();
            _leftContainer.name  = "LeftContainer";
            _rightContainer      = new ScrollView();
            _rightContainer.name = "RightContainer";

            Add(root);
            root.Add(_leftContainer);
            root.Add(_rightContainer);

            AddTreeItem();
            AddTreeItem();
        }

        private void AddTreeItem()
        {
            var item = new MenuTreeItem();
            _leftContainer.Add(item);
            item.OnSelected = OnSelected;
        }

        private void OnSelected(VisualElement node)
        {
            if (_lastNode != null) _lastNode.style.backgroundColor = StyleKeyword.Null;
            node.style.backgroundColor = ColorUtils.HexToColor("#17357288");
            _lastNode                  = node;
        }
    }
}