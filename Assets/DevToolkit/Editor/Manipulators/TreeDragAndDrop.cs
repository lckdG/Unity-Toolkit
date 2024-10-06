#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace DevToolkit.AI.Editor
{
    internal class TreeDragAndDrop : PointerManipulator
    {
        private BehaviorTreeView view;
        private Object droppedObject;
        private string assetPath = string.Empty;

        public TreeDragAndDrop(VisualElement root)
        {
            target = root.Q<BehaviorTreeView>();
            view = target as BehaviorTreeView;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
            target.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            target.RegisterCallback<DragPerformEvent>(OnDragPerformed);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
            target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated);
            target.UnregisterCallback<DragPerformEvent>(OnDragPerformed);
        }

        private void OnDragLeave(DragLeaveEvent evt)
        {
            assetPath = string.Empty;
            droppedObject = null;
        }

        private void OnDragUpdated(DragUpdatedEvent evt)
        {
            if (DragAndDrop.objectReferences.Length <= 0)
            {
                return;
            }

            var @object = DragAndDrop.objectReferences[0];
            if (@object is BehaviorTree && view.GetCurrentTree() != @object)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
        }

        private void OnDragPerformed(DragPerformEvent evt)
        {
            droppedObject = DragAndDrop.objectReferences[0];
            BehaviorTree tree = droppedObject as BehaviorTree;

            if (tree != null)
            {
                view.DropTree(tree, evt.mousePosition);
            }
        }
    }
}
#endif