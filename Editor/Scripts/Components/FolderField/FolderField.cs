using System;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

namespace AAMT.Editor
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public class FolderField : VisualElement
    {
        private Label            _label;
        private TextField        _textField;
        private Button           _openDirButton;
        private Button           _deleteButton;
        private FolderDragHelper _dragHelper;

        public Action<FolderField> OnValueChanged;
        public Action<FolderField> OnDeleted;

        public FolderField()
        {
            InitElements();
        }

        public FolderField(string path, string label = "")
        {
            InitElements();

            Label = label;
            Path  = path;
        }

        private void InitElements()
        {
            style.flexDirection = FlexDirection.Row;
            style.marginLeft    = style.marginRight = 5;
            var borderColor                         = MiscUtils.HexToColor("#1F1F1F");
            style.borderTopColor          = borderColor;
            style.borderRightColor        = borderColor;
            style.borderBottomColor       = borderColor;
            style.borderLeftColor         = borderColor;
            style.borderTopWidth          = 1;
            style.borderRightWidth        = 1;
            style.borderBottomWidth       = 1;
            style.borderLeftWidth         = 1;
            style.borderTopRightRadius    = 3;
            style.borderTopLeftRadius     = 3;
            style.borderBottomRightRadius = 3;
            style.borderBottomLeftRadius  = 3;

            _label         = new Label();
            _textField     = new TextField();
            _openDirButton = new Button();

            Add(_label);
            Add(_textField);
            Add(_openDirButton);

            _label.style.unityTextAlign = TextAnchor.MiddleRight;
            _label.style.marginLeft     = _label.style.marginRight = 5;

            _openDirButton.style.backgroundImage = Icons.FOLDER.Texture2D;
            _openDirButton.style.width           = _openDirButton.style.height = 20;

            _textField.style.flexGrow = 1;
            _textField.RegisterValueChangedCallback(OnTextFieldValueChanged);

            _openDirButton.clicked += OnOpenDir;

            _dragHelper          = new FolderDragHelper(this);
            _dragHelper.OnDrop   = OnDrop;
            _dragHelper.OnEnter  = OnEnter;
            _dragHelper.OnExited = OnExited;
        }

        private void OnEnter(DragEnterEvent obj)
        {
            this.style.backgroundColor = MiscUtils.HexToColor("#505050");
        }

        private void OnExited()
        {
            this.style.backgroundColor = new StyleColor(StyleKeyword.Null);
        }

        public void AddDeleteButton()
        {
            _deleteButton      = new Button();
            _deleteButton.text = "x";
            Add(_deleteButton);
            _deleteButton.clicked += OnDeleteClick;
        }

        private void OnDeleteClick()
        {
            var b = EditorUtility.DisplayDialog("删除节点", "您是否要删除当前节点？", "删除", "不删");
            if (b)
            {
                OnDeleted?.Invoke(this);
                this.parent.Remove(this);
            }
        }

        private void OnDrop(DragPerformEvent e, string[] path)
        {
            if (path.Length <= 0) return;
            e.StopPropagation();
            Path = $"{_dragHelper.BasePath}/{path[0]}";
        }

        private void OnTextFieldValueChanged(ChangeEvent<string> evt)
        {
            OnValueChanged?.Invoke(this);
        }

        private void OnOpenDir()
        {
            Path = EditorUtility.OpenFolderPanel("选择文件夹", Application.dataPath, "");
        }

        public string Label
        {
            get => _label.text;
            set
            {
                _label.text = value;
                _label.setActive(!string.IsNullOrEmpty(value));
            }
        }

        public string Path
        {
            get => _textField.value;
            set { _textField.value = value; }
        }


        #region UxmlFactory

        public new class UxmlFactory : UxmlFactory<FolderField, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlStringAttributeDescription _label;

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                if (!(ve is FolderField folderField)) return;
                folderField.Label = _label.GetValueFromBag(bag, cc);
            }

            public UxmlTraits()
            {
                var description = new UxmlStringAttributeDescription();
                description.name         = "label";
                description.defaultValue = "FolderField";
                _label                   = description;
            }
        }

        #endregion
    }
}