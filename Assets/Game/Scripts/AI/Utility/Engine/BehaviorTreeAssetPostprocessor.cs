using UnityEngine;
using UnityEditor;

namespace AI.Tree.Editor
{
    public class BehaviorTreeAssetPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            BehaviorTreeEditorUtility.OnAssetPostProcessed();
        }
    }
}

