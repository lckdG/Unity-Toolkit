#if UNITY_EDITOR

using UnityEngine.UIElements;
using UnityEditor;

namespace BehaviorTreeAI
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private Editor editor;
        public InspectorView() {  }

        internal void UpdateSelection( UnityEngine.Object view)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate( editor );
            editor = Editor.CreateEditor( view );
            IMGUIContainer container = new IMGUIContainer( () => {
                if ( editor.target )
                {
                    editor.OnInspectorGUI();
                }
            } );
            Add(container);
        }
    }
}

#endif