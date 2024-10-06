#if UNITY_EDITOR
using UnityEditor;

namespace DevToolkit.AI.Editor
{
    public class BehaviorTreeAssetPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            BehaviorTreeEditorUtility.OnAssetPostProcessed();
        }
    }
}
#endif
