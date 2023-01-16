using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

        public FolderField(string label, string dirPath)
        {
            InitElements();

            Label   = label;
            DirPath = dirPath;
        }

        private void InitElements()
        {
            style.flexDirection = FlexDirection.Row;
            style.marginLeft    = style.marginRight = 5;

            _label         = new Label();
            _textField     = new TextField();
            _openDirButton = new Button();

            Add(_label);
            Add(_textField);
            Add(_openDirButton);

            _label.style.unityTextAlign = TextAnchor.MiddleRight;

            _openDirButton.style.backgroundImage = Icons.FOLDER.Texture2D;
            _openDirButton.style.width           = _openDirButton.style.height = 20;

            _textField.style.flexGrow = 1;
            _textField.RegisterValueChangedCallback(OnTextFieldValueChanged);

            _openDirButton.clicked += OnOpenDir;

            _dragHelper        = new FolderDragHelper(this);
            _dragHelper.OnDrop = OnDrop;
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
            OnDeleted?.Invoke(this);
        }

        private void OnDrop(string[] path)
        {
            if (path.Length <= 0) return;
            DirPath = $"{_dragHelper.BasePath}/{path[0]}";
        }

        private void OnTextFieldValueChanged(ChangeEvent<string> evt)
        {
            OnValueChanged?.Invoke(this);
        }

        private void OnOpenDir()
        {
            DirPath = EditorUtility.OpenFolderPanel("选择文件夹", Application.dataPath, "");
        }

        public string Label
        {
            get => _label.text;
            set
            {
                _label.text    = value;
                _label.visible = !string.IsNullOrEmpty(value);
            }
        }

        public string DirPath
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