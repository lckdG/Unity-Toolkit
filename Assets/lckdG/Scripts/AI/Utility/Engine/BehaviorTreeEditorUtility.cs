#if UNITY_EDITOR
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DevToolkit.AI.Editor
{
    public static class BehaviorTreeEditorUtility
    {
        private static System.Action AssetDatabasePostprocessCompleted;

        public static void RegisterAssetPostprocessCallback(System.Action callback)
        {
            if (callback == null) return;
            AssetDatabasePostprocessCompleted += callback;
        }

        public static void UnregisterAssetPostprocessCallback(System.Action callback)
        {
            AssetDatabasePostprocessCompleted -= callback;
        }

        public static void OnAssetPostProcessed()
        {
            AssetDatabasePostprocessCompleted?.Invoke();
        }

        public async static Task CreateRoot(BehaviorTree tree)
        {
            if (tree.root != null) return;

            Root root = ScriptableObject.CreateInstance<Root>();
            root.name = "Root";

            AssetDatabase.AddObjectToAsset(root, tree);
            AssetDatabase.SaveAssets();

            await Delay();
            tree.root = root;
            tree.nodes.Add(root);
            
            await Delay();
            string assetPath = AssetDatabase.GetAssetPath(tree);
            AssetDatabase.ForceReserializeAssets(new string[] { assetPath });
        }

        public async static Task UpdateBlackboard(BehaviorTree tree)
        {
            if (tree.HasBlackboard()) return;

            Blackboard blackboard = ScriptableObject.CreateInstance<Blackboard>();
            blackboard.name = "Blackboard";

            AssetDatabase.AddObjectToAsset(blackboard, tree);
            AssetDatabase.SaveAssets();

            await Delay();
            tree.AssignBlackboard(blackboard);

            await Delay();
            string assetPath = AssetDatabase.GetAssetPath(tree);
            AssetDatabase.ForceReserializeAssets(new string[] { assetPath });
        }

        private static async Task Delay()
        {
            await Task.Delay(200);
        }
    }
}
#endif
