using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class FolderFieldList : Foldout
    {
        private FolderDragHelper  _dragHelper;
        private List<FolderField> _folderFields;

        public FolderFieldList()
        {
            Init();
        }

        private void Init()
        {
            style.paddingBottom = 5;

            _folderFields        = new List<FolderField>();
            _dragHelper          = new FolderDragHelper(this);
            _dragHelper.OnDrop   = OnDrop;
            _dragHelper.OnEnter  = OnEnter;
            _dragHelper.OnExited = OnExited;
        }

        private void OnDrop(DragPerformEvent e, string[] paths)
        {
            foreach (var path in paths)
            {
                if (!CheckSamePath(path))
                {
                    var folder = new FolderField(path);
                    _folderFields.Add(folder);
                    Add(folder);
                }
            }
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