#if UNITY_EDITOR

using UnityEngine.UIElements;

namespace DevToolkit.AI.Editor
{
    internal class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
        
    }
}

#endif
