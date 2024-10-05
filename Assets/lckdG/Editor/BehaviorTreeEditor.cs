#if UNITY_EDITOR
using System.IO;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

namespace AI.Tree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        public static string editorPath { 
            get { 

                DirectoryInfo folder = new DirectoryInfo(Directory.GetCurrentDirectory());
                FileInfo[] files = folder.GetFiles("BehaviorTreeEditor.cs", SearchOption.AllDirectories);

                if (files.Length > 0)
                {
                    string absolutePath = files[0].DirectoryName;

                    int relativePathStart = absolutePath.IndexOf("\\Assets\\");
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
        private BehaviorTreeView treeView;
        private InspectorView inspectorBlackboardView;
        Blackboard blackboardProperty;

        [MenuItem("Window/AI/Behaviour Tree Editor")]
        public static void OpenWindow()
        {
            BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviorTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is BehaviorTree)
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
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(editorPath + "Visuals\\BehaviorTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(editorPath + "Visuals\\BehaviorTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviorTreeView>();
            inspectorBlackboardView = root.Q<InspectorView>("blackboard-inspector");

            OnSelectionChange();

            if (blackboardProperty != null)
            {
                inspectorBlackboardView.UpdateSelection(blackboardProperty);
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
            switch (obj)
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
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (tree == false)
            {
                if (Selection.activeGameObject)
                {
                    BehaviorTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();
                    if (runner)
                    {
                        tree = runner.GetTree();
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (tree != null && treeView != null)
                {
                    treeView.PopulateView(tree);
                }
            }
            else
            {
                if (tree && AssetDatabase.CanOpenForEdit(AssetDatabase.GetAssetPath(Selection.activeObject)))
                {
                    treeView.PopulateView(tree);
                }
            }

            if (tree != null)
            {
                string treePath = AssetDatabase.GetAssetPath(tree);
                if (Application.isPlaying && string.IsNullOrEmpty(treePath))
                {
                    blackboardProperty = tree.GetBlackboard();
                }
                else
                {
                    blackboardProperty = tree.blackboardRef;
                }
            }
        }

        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeState();
        }
    }
}

#endif