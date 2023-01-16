using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class MenuTree : VisualElement
    {
        private VisualElement _leftContainer;
        private VisualElement _rightContainer;
        private IMenuItem _currentNode;
        private List<MenuTreeItem> _items = new List<MenuTreeItem>();
        private int _selectIndex;

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
        }

        public MenuTreeItem AddItem(string name, ContentNode node = null, Icon icon = null, bool defaultChildShow = false)
        {
            if (HasItemByName(name))
            {
                throw new Exception($"Had same name:\"{name}\" item. Can not have the same name item.");
            }

            var item = new MenuTreeItem(name, node, icon, defaultChildShow);
            _leftContainer.Add(item);
            item.OnSelected = OnSelected;
            _items.Add(item);
            return item;
        }

        private void OnSelected(VisualElement node)
        {
            if (!(node is IMenuItem) || _currentNode == node) return;
            var item = node as IMenuItem;
            if (_currentNode != null) _currentNode.SetBackgroundColor(StyleKeyword.Null);
            item.SetBackgroundColor(MiscUtils.HexToColor("#2F6A9B88"));
            _currentNode = item;
            _rightContainer.Clear();

            item.ShowContentNode(_rightContainer);
        }

        public void SelectByName(string name, string childItemName)
        {
            if (string.IsNullOrEmpty(name)) return;
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                if (item.name == name)
                {
                    if (!string.IsNullOrEmpty(childItemName))
                    {
                        var item1 = item.GetItemByName(childItemName);
                        if (item1 != null) OnSelected(item1);
                    }
                    else
                    {
                        OnSelected(item);
                    }

                    return;
                }
            }
        }

        private bool HasItemByName(string name)
        {
            foreach (var item in _items)
            {
                if (item.name == name) return true;
            }

            return false;
        }


        #region UxmlFactory

        public new class UxmlFactory : UxmlFactory<MenuTree, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        #endregion
    }
}