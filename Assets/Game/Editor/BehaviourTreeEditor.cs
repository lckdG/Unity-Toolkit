#if UNITY_EDITOR
using System.IO;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

namespace AI.Tree.Editor
{
    public class BehaviourTreeEditor : EditorWindow
    {
        public static string editorPath { 
            get { 

                DirectoryInfo folder = new DirectoryInfo( Directory.GetCurrentDirectory() );
                FileInfo[] files = folder.GetFiles( "BehaviourTreeEditor.cs", SearchOption.AllDirectories );

                if ( files.Length > 0 )
                {
                    string absolutePath = files[0].DirectoryName;

                    int relativePathStart = absolutePath.IndexOf( "\\Assets\\" );
                    _editorPath = absolutePath.Substring( relativePathStart + 1 ) + "\\";
                }

                return _editorPath;
            }

            private set
            {
                _editorPath = value;
            }
        }
        
        private static string _editorPath = "";
        private BehaviourTreeView treeView;
        private InspectorView inspectorNodeView;
        private InspectorView inspectorBlackboardView;
        Blackboard blackboardProperty;

        [MenuItem("Window/AI/Behaviour Tree Editor")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset( int instanceId, int line )
        {
            if ( Selection.activeObject is BehaviourTree )
            {
                OpenWindow();
                return true;
            }

            return false;
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(editorPath + "BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(editorPath + "BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorNodeView = root.Query<InspectorView>("node-inspector");
            inspectorBlackboardView = root.Q<InspectorView>("blackboard-inspector");

            treeView.OnNodeSelected = OnNodeSelectionChanged;

            OnSelectionChange();

            if ( blackboardProperty != null )
            {
                inspectorBlackboardView.UpdateSelection( blackboardProperty );
            }
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch ( obj )
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;    
                case PlayModeStateChange.ExitingEditMode:
                    break;  
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;  
                case PlayModeStateChange.ExitingPlayMode:
                    break;  
            }
        }

        private void OnSelectionChange()
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;
            if ( !tree )
            {
                if ( Selection.activeGameObject )
                {
                    BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                    if ( runner )
                    {
                        tree = runner.GetTree();
                    } 
                }
            }

            if ( Application.isPlaying )
            {
                if ( tree )
                {
                    treeView.PopulateView( tree );
                }
            }
            else
            {
                if ( tree && AssetDatabase.CanOpenForEdit( AssetDatabase.GetAssetPath( Selection.activeObject) ) )
                {
                    treeView.PopulateView( tree );
                }
            }

            if ( tree != null )
            {
                blackboardProperty = tree.blackboardRef;
            }
        }

        private void OnNodeSelectionChanged( NodeView node )
        {
            if ( node != null )
            {
                inspectorNodeView.UpdateSelection( (Object) node.node );
            }
        }

        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeState();
        }
    }
}

#endif