using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AAMT.Editor
{
    public class FolderDragHelper
    {
        private VisualElement         _root;
        public  string                BasePath { get; private set; }
        private DragAndDropVisualMode _dragAndDropVisualMode;
        public  Action<string[]>      OnDrop;

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
        }

        private void OnPerform(DragPerformEvent _)
        {
            OnDrop?.Invoke(DragAndDrop.paths);
        }

        private void OnDragEnter(DragEnterEvent _)
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
        }

        private void OnDragExit(DragExitedEvent _)
        {
            _dragAndDropVisualMode = DragAndDropVisualMode.Generic;
        }

        private void OnDragUpdate(DragUpdatedEvent _)
        {
            DragAndDrop.visualMode = _dragAndDropVisualMode;
        }
    }
}