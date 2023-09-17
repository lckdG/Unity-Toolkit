using System.Threading.Tasks;
using UnityEditor;

namespace AI.Tree.Editor
{
    public static class BehaviorTreeEditorUtility
    {
        private static System.Action AssetDatabasePostprocessCompleted;

        public static void RegisterAssetPostprocessCallback( System.Action callback )
        {
            if ( callback == null ) return;
            AssetDatabasePostprocessCompleted += callback;
        }

        public static void UnregisterAssetPostprocessCallback( System.Action callback )
        {
            AssetDatabasePostprocessCompleted -= callback;
        }

        public static void OnAssetPostProcessed()
        {
            AssetDatabasePostprocessCompleted?.Invoke();
        }

        public static async void DelayRefreshAssetDatabase( string assetPath )
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( assetPath );

            await Delay();
            AssetDatabase.ForceReserializeAssets( new string[] { assetPath } );

            await Delay();
            AssetDatabase.Refresh();

            Selection.activeObject = asset;
        }

        private static async Task Delay()
        {
            await Task.Delay( 100 );
        }
    }
}
