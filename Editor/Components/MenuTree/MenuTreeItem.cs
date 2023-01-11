using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor.Components
{
    public class MenuTreeItem : VisualElement
    {
        private readonly string _menuTreeItemclassName = "menu-tree-item";
        private Label _nameLabel;
        private Image _icon;
        private Image _arrow;
        private VisualElement _treeListContainer;
        private bool _showList = true;
        public Action<VisualElement> OnSelected;

        public MenuTreeItem()
        {
            AddToClassList(_menuTreeItemclassName);
            name = "MenuTreeItem";
            CreateTopItem();
        }

        private void CreateTopItem()
        {
            var root = new VisualElement();
            Add(root);
            root.name = "TopItem";
            root.AddToClassList("top-item");
            root.AddToClassList("item-border");
            root.AddToClassList("itemHover");

            _icon                   = new Image();
            _icon.image             = Icons.GetTextureByName(Icons.HOME);
            _icon.style.width       = 20;
            _icon.style.height      = 20;
            _icon.style.marginRight = 10;
            root.Add(_icon);


            _nameLabel                      = new Label();
            _nameLabel.text                 = "Name Label";
            _nameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            _nameLabel.style.flexGrow       = 1;
            root.Add(_nameLabel);

            _arrow              = new Image();
            _arrow.image        = Icons.GetTextureByName(Icons.DROP_DOWN_ARROW);
            _arrow.style.width  = 20;
            _arrow.style.height = 20;
            root.Add(_arrow);

            _treeListContainer = new VisualElement();
            Add(_treeListContainer);
            _treeListContainer.name = "MenuTreeList";

            root.RegisterCallback<ClickEvent>(OnTopClick);

            CreateItem();
            CreateItem();
            CreateItem();
        }

        private void OnTopClick(ClickEvent evt)
        {
            _showList = !_showList;
            var angle = _showList ? 0 : -90;
            _arrow.style.rotate = new StyleRotate(new Rotate(angle));
            var display = _showList ? DisplayStyle.Flex : DisplayStyle.None;
            _treeListContainer.style.display = new StyleEnum<DisplayStyle>(display);
            OnClick(evt);
        }

        private void CreateItem()
        {
            var listItem = new VisualElement();
            listItem.name = "MenuTreeListItem";
            listItem.AddToClassList("itemHover");
            listItem.AddToClassList("list-item");
            listItem.AddToClassList("item-border");
            _treeListContainer.Add(listItem);
            var label = new Label();
            label.text = "nameLabel";
            listItem.Add(label);
            listItem.RegisterCallback<ClickEvent>(OnClick);
        }

        private void OnClick(ClickEvent evt)
        {
            OnSelected.Invoke(evt.currentTarget as VisualElement);
        }
    }
}