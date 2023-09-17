#if UNITY_EDITOR

using UnityEngine.UIElements;

namespace AI.Tree.Editor
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
        
    }
}

#endif
