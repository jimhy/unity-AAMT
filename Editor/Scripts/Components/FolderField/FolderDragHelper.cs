using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class FolderDragHelper
    {
        private VisualElement                      _root;
        public  string                             BasePath { get; private set; }
        private DragAndDropVisualMode              _dragAndDropVisualMode;
        public  Action<DragPerformEvent, string[]> OnDrop;
        public  Action<DragEnterEvent>             OnEnter;
        public  Action                             OnExited;

        public FolderDragHelper(VisualElement root)
        {
            _root = root;
            Init();
        }

        private void Init()
        {
            BasePath = Application.dataPath.Replace("/Assets", "");
            _root.RegisterCallback<DragEnterEvent>(OnDragEnter);
            _root.RegisterCallback<DragPerformEvent>(OnPerform);
            _root.RegisterCallback<DragExitedEvent>(OnDragExit);
            _root.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
            _root.RegisterCallback<DragLeaveEvent>(OnDragLeave);
        }

        private void OnDragLeave(DragLeaveEvent e)
        {
            _dragAndDropVisualMode = DragAndDropVisualMode.Generic;
            OnExited?.Invoke();
        }

        private void OnPerform(DragPerformEvent e)
        {
            OnDrop?.Invoke(e, DragAndDrop.paths);
        }

        private void OnDragEnter(DragEnterEvent e)
        {
            var b = true;

            foreach (var path in DragAndDrop.paths)
            {
                var p = $"{BasePath}/{path}";
                if (!Directory.Exists(p))
                {
                    b = false;
                    break;
                }
            }

            _dragAndDropVisualMode = b ? DragAndDropVisualMode.Generic : DragAndDropVisualMode.Rejected;
            OnEnter?.Invoke(e);
        }

        private void OnDragExit(DragExitedEvent e)
        {
            _dragAndDropVisualMode = DragAndDropVisualMode.Generic;
            OnExited?.Invoke();
        }

        private void OnDragUpdate(DragUpdatedEvent _)
        {
            DragAndDrop.visualMode = _dragAndDropVisualMode;
        }
    }
}