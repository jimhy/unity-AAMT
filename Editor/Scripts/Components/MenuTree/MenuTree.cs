using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class MenuTree : VisualElement
    {
        private readonly VisualElement _leftContainer;
        private readonly VisualElement _rightContainer;
        private IMenuItem _currentNode;
        private readonly List<MenuTreeItem> _items = new();
        private int _selectIndex;

        public MenuTree()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PathManager.MenuTreeUssPath);
            styleSheets.Add(styleSheet);

            var root = new VisualElement { name = "Root" };
            _leftContainer       = new ScrollView();
            _leftContainer.name  = "LeftContainer";
            _rightContainer      = new ScrollView();
            _rightContainer.name = "RightContainer";

            Add(root);
            root.Add(_leftContainer);
            root.Add(_rightContainer);
        }

        public MenuTreeItem AddItem(string itemName, ContentNode node = null, Icon icon = null, bool defaultChildShow = false)
        {
            if (HasItemByName(itemName))
            {
                throw new Exception($"Had same name:\"{itemName}\" item. Can not have the same name item.");
            }

            var item = new MenuTreeItem(itemName, node, icon, defaultChildShow);
            _leftContainer.Add(item);
            item.OnSelected = OnSelected;
            _items.Add(item);
            return item;
        }

        private void OnSelected(VisualElement node)
        {
            if (node is not IMenuItem item || _currentNode == node) return;
            _currentNode?.SetBackgroundColor(StyleKeyword.Null);
            item.SetBackgroundColor(MiscUtils.HexToColor("#2F6A9B88"));
            _currentNode = item;
            _rightContainer.Clear();

            item.ShowContentNode(_rightContainer);
        }

        public void SelectByName(string name, string childItemName)
        {
            if (string.IsNullOrEmpty(name)) return;
            foreach (var item in _items.Where(item => item.name == name))
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

        private bool HasItemByName(string itemName)
        {
            return _items.Any(item => item.name == itemName);
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