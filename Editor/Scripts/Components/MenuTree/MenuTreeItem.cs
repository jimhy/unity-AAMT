using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class MenuTreeItem : VisualElement, IMenuItem
    {
        private const string _menuTreeItemClassName = "menu-tree-item";
        private Label _nameLabel;
        private Image _iconNode;
        private Image _arrow;
        private VisualElement _treeListContainer;
        private bool _showList;
        public Action<VisualElement> OnSelected;
        private readonly ContentNode _contentNode;
        private readonly Icon _icon;
        private VisualElement _root;
        private string _assetFolderPath;
        private ContentNode _childContentNode;
        private Type _childAssetsType;
        private readonly List<ChildNode> _children = new();
        private ChildNode _selected;

        public MenuTreeItem(string name, ContentNode contentNode, Icon icon, bool defaultChildShow = false)
        {
            AddToClassList(_menuTreeItemClassName);
            this.name    = name;
            _contentNode = contentNode;
            _icon        = icon;
            _showList    = defaultChildShow;
            CreateTopItem();
            AddEvents();
            RegisterCallback<DetachFromPanelEvent>(OnRemoveFromParent);
        }

        private void OnRemoveFromParent(DetachFromPanelEvent evt)
        {
            RemoveEvents();
        }

        private void AddEvents()
        {
            EditorCommon.EventBus.addEventListener(EventType.MOVED_ASSETS, OnFileChanged);
            EditorCommon.EventBus.addEventListener(EventType.DELETED_ASSETS, OnFileChanged);
            EditorCommon.EventBus.addEventListener(EventType.IMPORTED_ASSETS, OnFileChanged);
        }

        private void RemoveEvents()
        {
            EditorCommon.EventBus.removeEventListener(EventType.MOVED_ASSETS, OnFileChanged);
            EditorCommon.EventBus.removeEventListener(EventType.DELETED_ASSETS, OnFileChanged);
            EditorCommon.EventBus.removeEventListener(EventType.IMPORTED_ASSETS, OnFileChanged);
        }
        
        private void OnFileChanged(Event e)
        {
            if (_childContentNode == null) return;
            if (!CheckAssetsPath(e.data as string[])) return;
            _treeListContainer?.Clear();
            UpdateAllAssetsAtPath();
            if (_selected != null) OnSelected.Invoke(_selected);
        }

        private bool CheckAssetsPath(IReadOnlyCollection<string> eData)
        {
            if (eData == null || eData.Count == 0) return false;
            return eData.Any(s => s.IndexOf(_assetFolderPath, StringComparison.Ordinal) != -1);
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

        public ChildNode AddItem(string name, ContentNode contentNode = null, object data = null)
        {
            createChildContainer();
            var listItem = new ChildNode(name, contentNode, data);
            _treeListContainer.Add(listItem);
            _children.Add(listItem);
            listItem.RegisterCallback<ClickEvent>(OnChildItemClick);
            return listItem;
        }

        public void AddAllAssetsAtPath(string assetFolderPath, ContentNode contentNode, Type type)
        {
            if (_childContentNode != null)
            {
                throw new Exception("不能重复添加资源路径");
            }

            if (string.IsNullOrEmpty(assetFolderPath) || contentNode == null || type == null)
            {
                throw new Exception("参数不能为空");
            }

            _assetFolderPath  = assetFolderPath;
            _childContentNode = contentNode;
            _childAssetsType  = type;
            UpdateAllAssetsAtPath();
        }

        private void UpdateAllAssetsAtPath()
        {
            var    assetFolderPath = (_assetFolderPath ?? "").TrimEnd('/') + "/";
            string lower           = assetFolderPath.ToLower();

            if (!lower.StartsWith("assets/") && !lower.StartsWith("packages/")) assetFolderPath = "Assets/" + assetFolderPath;

            var fullPath = Application.dataPath.Replace("Assets", "");
            assetFolderPath = $"{fullPath}{assetFolderPath}";
            if (!Directory.Exists(assetFolderPath)) return;
            var paths = Directory.GetFiles(assetFolderPath, "*", SearchOption.AllDirectories);
            foreach (string str1 in paths)
            {
                var p = str1.Replace(fullPath, "");
                var o = AssetDatabase.LoadAssetAtPath(p, _childAssetsType);
                if (o != null)
                {
                    var child = AddItem(o.name, _childContentNode, o);
                    child.Guid = AssetDatabase.AssetPathToGUID(p);
                    if (_selected != null && child.Guid == _selected.Guid)
                    {
                        _selected = child;
                    }
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
            _treeListContainer.style.display = display;
        }

        private void OnChildItemClick(ClickEvent evt)
        {
            _selected = evt.currentTarget as ChildNode;
            OnSelected.Invoke(_selected);
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
        public object Data { get; set; }
        public string Guid { get; set; }

        public ChildNode(string name, ContentNode contentNode, object data)
        {
            this.name = name;
            AddToClassList("itemHover");
            AddToClassList("list-item");
            AddToClassList("item-border");

            var label = new Label();
            label.text = name;
            Add(label);

            _contentNode = contentNode;
            Data         = data;
            if (_contentNode != null) _contentNode.style.flexGrow = 1;
        }

        public void ShowContentNode(VisualElement parent)
        {
            if (_contentNode == null) return;
            parent.Add(_contentNode);
            _contentNode.SetData(Data);
        }

        public void SetBackgroundColor(StyleColor styleColor)
        {
            style.backgroundColor = styleColor;
        }
    }
}