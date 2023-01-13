using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor.Components
{
    public class MenuTreeItem : VisualElement, IMenuItem
    {
        private readonly string _menuTreeItemclassName = "menu-tree-item";
        private Label _nameLabel;
        private Image _iconNode;
        private Image _arrow;
        private VisualElement _treeListContainer;
        private bool _showList;
        public Action<VisualElement> OnSelected;
        private ContentNode _contentNode;
        private Icon _icon;
        private VisualElement _root;
        private List<ChildNode> _children = new List<ChildNode>();

        public MenuTreeItem(string name, ContentNode contentNode, Icon icon, bool defaultChildShow = false)
        {
            AddToClassList(_menuTreeItemclassName);
            this.name    = name;
            _contentNode = contentNode;
            _icon        = icon;
            _showList    = defaultChildShow;
            CreateTopItem();
        }

        private void CreateTopItem()
        {
            _root = new VisualElement();
            Add(_root);
            _root.name = "TopItem";
            _root.AddToClassList("top-item");
            _root.AddToClassList("item-border");
            _root.AddToClassList("itemHover");

            if (_icon != null)
            {
                _iconNode                   = new Image();
                _iconNode.image             = _icon.Texture;
                _iconNode.style.width       = 20;
                _iconNode.style.height      = 20;
                _iconNode.style.marginRight = 10;
                _root.Add(_iconNode);
            }

            _nameLabel                      = new Label();
            _nameLabel.text                 = this.name;
            _nameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            _nameLabel.style.flexGrow       = 1;
            _root.Add(_nameLabel);

            _root.RegisterCallback<ClickEvent>(OnTopClick);
        }

        private void OnTopClick(ClickEvent evt)
        {
            _showList = !_showList;

            childContainerDisplayUpdate();
            OnSelected.Invoke(this);
        }

        public void AddItem(string name, ContentNode contentNode = null, object data = null)
        {
            createChildContainer();
            var listItem = new ChildNode(name, contentNode, data);
            _treeListContainer.Add(listItem);
            _children.Add(listItem);
            listItem.RegisterCallback<ClickEvent>(OnChildItemClick);
        }

        public void AddAllAssetsAtPath(string assetFolderPath, ContentNode contentNode, Type type, bool includeSubDirectories = false)
        {
            assetFolderPath = (assetFolderPath ?? "").TrimEnd('/') + "/";
            string lower = assetFolderPath.ToLower();

            if (!lower.StartsWith("assets/") && !lower.StartsWith("packages/")) assetFolderPath = "Assets/" + assetFolderPath;

            var fullPath = Application.dataPath.Replace("Assets", "");
            assetFolderPath = $"{fullPath}{assetFolderPath}";
            var paths = Directory.GetFiles(assetFolderPath, "*", SearchOption.AllDirectories);
            foreach (string str1 in paths)
            {
                var p = str1.Replace(fullPath, "");
                var o = AssetDatabase.LoadAssetAtPath(p, type);
                if (o != null)
                {
                    AddItem(o.name, contentNode, o);
                }
            }
        }

        private void createChildContainer()
        {
            if (_treeListContainer != null) return;
            _arrow              = new Image();
            _arrow.image        = Icons.DROP_DOWN_ARROW.Texture;
            _arrow.style.width  = 20;
            _arrow.style.height = 20;
            _root.Add(_arrow);

            _treeListContainer = new VisualElement();
            Add(_treeListContainer);
            _treeListContainer.name = "MenuTreeList";

            childContainerDisplayUpdate();
        }

        private void childContainerDisplayUpdate()
        {
            if (_treeListContainer == null) return;
            var angle = _showList ? 0 : -90;
            _arrow.style.rotate = new StyleRotate(new Rotate(angle));
            var display = _showList ? DisplayStyle.Flex : DisplayStyle.None;
            _treeListContainer.style.display = new StyleEnum<DisplayStyle>(display);
        }

        private void OnChildItemClick(ClickEvent evt)
        {
            OnSelected.Invoke(evt.currentTarget as VisualElement);
        }

        public VisualElement GetItemByName(string childItemName)
        {
            foreach (var child in _children)
            {
                if (child.name == childItemName) return child;
            }

            return null;
        }

        public void ShowContentNode(VisualElement parent)
        {
            if (_contentNode == null) return;
            parent.Add(_contentNode);
        }

        public void SetBackgroundColor(StyleColor styleColor)
        {
            _root.style.backgroundColor = styleColor;
        }
    }

    public class ChildNode : VisualElement, IMenuItem
    {
        public ContentNode _contentNode { get; set; }
        private object _data;

        public ChildNode(string name, ContentNode contentNode, object data)
        {
            this.name = "MenuTreeListItem";
            AddToClassList("itemHover");
            AddToClassList("list-item");
            AddToClassList("item-border");

            var label = new Label();
            label.text = name;
            Add(label);

            _contentNode = contentNode;
            _data        = data;
            if (_contentNode != null) _contentNode.style.flexGrow = 1;
        }


        public void ShowContentNode(VisualElement parent)
        {
            if (_contentNode == null) return;
            parent.Add(_contentNode);
            _contentNode.SetData(_data);
        }

        public void SetBackgroundColor(StyleColor styleColor)
        {
            style.backgroundColor = styleColor;
        }
    }
}