using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class DragList : Foldout
    {
        public  bool             IsDir { get; private set; }
        private FolderDragHelper _dragHelper;

        public DragList()
        {
            IsDir = false;
            Init();
        }

        public DragList(bool isDir = false)
        {
            IsDir = isDir;
            Init();
        }

        private void Init()
        {
            _dragHelper        = new FolderDragHelper(this);
            _dragHelper.OnDrop = OnDrop;
        }

        private void OnDrop(string[] paths)
        {
            
        }

        private void CreateFolderField(string path)
        {
            
        }

        private void SetIsDirValue(bool v)
        {
            this.IsDir = v;
        }

        #region UxmlFactory

        public new class UxmlFactory : UxmlFactory<DragList, UxmlTraits>
        {
        }

        public new class UxmlTraits : Foldout.UxmlTraits
        {
            private UxmlBoolAttributeDescription isDir;

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                if (!(ve is DragList dragList)) return;
                dragList.SetIsDirValue(this.isDir.GetValueFromBag(bag, cc));
            }

            public UxmlTraits()
            {
                var description = new UxmlBoolAttributeDescription();
                description.name         = "is directory";
                description.defaultValue = false;
                this.isDir               = description;
            }
        }

        #endregion
    }
}