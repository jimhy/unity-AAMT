using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class FolderFieldList : Foldout
    {
        private FolderDragHelper  _dragHelper;
        private List<FolderField> _folderFields;
        public  Action            OnValueChanged;
        private List<string>      _limitRootPaths;

        public FolderFieldList()
        {
            Init();
        }

        private void Init()
        {
            style.paddingBottom = 5;

            _folderFields        = new List<FolderField>();
            _dragHelper          = new FolderDragHelper(this);
            _limitRootPaths      = new List<string>();
            _dragHelper.OnDrop   = OnDrop;
            _dragHelper.OnEnter  = OnEnter;
            _dragHelper.OnExited = OnExited;
        }

        private void AddLimitRootPath(string path)
        {
            _limitRootPaths.Add(path);
        }

        public string[] Data
        {
            get
            {
                var list = new string[_folderFields.Count];
                for (var i = 0; i < _folderFields.Count; i++)
                {
                    var folder = _folderFields[i];
                    list[i] = folder.Path;
                }

                return list;
            }
            set
            {
                Clear();
                _folderFields.Clear();
                CreateFolders(value);
            }
        }

        private void OnDrop(DragPerformEvent e, string[] paths)
        {
            CreateFolders(paths);
        }

        private void CreateFolders(string[] paths)
        {
            foreach (var path in paths)
            {
                if (!CheckSamePath(path))
                {
                    var folder = new FolderField(path);
                    folder.OnDeleted      = OnDeleted;
                    folder.OnValueChanged = OnFolderValueChanged;
                    folder.AddDeleteButton();
                    _folderFields.Add(folder);
                    Add(folder);
                }
            }

            OnValueChanged?.Invoke();
        }

        private void OnFolderValueChanged(FolderField obj)
        {
            OnValueChanged?.Invoke();
        }

        private void OnDeleted(FolderField folder)
        {
            _folderFields.Remove(folder);
            OnValueChanged?.Invoke();
        }

        private bool CheckSamePath(string path)
        {
            foreach (var field in _folderFields)
            {
                if (field.Path == path) return true;
            }

            return false;
        }

        private void OnEnter(DragEnterEvent obj)
        {
            this.style.backgroundColor = MiscUtils.HexToColor("#505050");
        }

        private void OnExited()
        {
            this.style.backgroundColor = new StyleColor(StyleKeyword.Null);
        }

        #region UxmlFactory

        public new class UxmlFactory : UxmlFactory<FolderFieldList, UxmlTraits>
        {
        }

        public new class UxmlTraits : Foldout.UxmlTraits
        {
        }

        #endregion
    }
}