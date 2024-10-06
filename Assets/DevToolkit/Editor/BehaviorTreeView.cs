#if UNITY_EDITOR

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DevToolkit.AI.Editor
{
    internal class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }

        public Action<BehaviorTree, Vector2> OnDroppedTree;

        private BehaviorTree tree;
        public BehaviorTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new TreeDragAndDrop(this));

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(BehaviorTreeEditor.editorPath + "Visuals\\BehaviorTreeEditor.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;

            BehaviorTreeEditorUtility.RegisterAssetPostprocessCallback(OnAssetPostProcessed);
        }

        ~BehaviorTreeView()
        {
            BehaviorTreeEditorUtility.UnregisterAssetPostprocessCallback(OnAssetPostProcessed);
        }

        private void OnAssetPostProcessed()
        {
            if (this.tree == null)
            {
                DeleteOldTree();
            }
        }

        private void DeleteOldTree()
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());

            if (this.tree == null) return;
            graphViewChanged += OnGraphViewChanged;
        }

        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        internal void PopulateView(BehaviorTree tree)
        {
            this.tree = tree;
            DeleteOldTree();

            Vector2 offsetZero = Vector2.zero;

            tree.nodes.ForEach(n => CreateNodeView(n, offsetZero));
            tree.nodes.ForEach(n => {
                NodeView parentView = FindNodeView(n);

                var children = tree.GetChildren(n);

                if (parentView.subOutput != null) // Simple parallel node only
                {
                    Node mainAction = children[0];
                    Node subAction = children[1]; 

                    NodeView mainActionView = FindNodeView(mainAction);
                    Edge mainEdge = parentView.output.ConnectTo(mainActionView.input);
                    AddElement(mainEdge);

                    NodeView subActionView = FindNodeView(subAction);
                    Edge subEdge = parentView.subOutput.ConnectTo(subActionView.input);
                    AddElement(subEdge);
                }
                else
                {
                    children.ForEach(c => {
                        NodeView childView = FindNodeView(c);

                        Edge edge = parentView.output.ConnectTo(childView.input);
                        AddElement(edge);
                    });
                }
            });
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove?.ForEach(element => {
                NodeView nodeView = element as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = element as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    tree.RemoveChild(parentView.node, childView.node);
                }
            });

            graphViewChange.edgesToCreate?.ForEach(edge => {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;

                tree.AddChild(parentView.node, childView.node);
            });

            if (graphViewChange.movedElements != null)
            {
                nodes.ForEach(n => {
                    NodeView view = n as NodeView;
                    view.SortChildren();
                });
            }

            return graphViewChange;
        }

        private void CreateNodeView(Node node, Vector2 offset, bool subTree = false)
        {
            NodeView nodeView = new NodeView(node, offset, subTree);
            AddElement(nodeView);

            SubTree subTreeNode = node as SubTree;
            if (subTreeNode)
            {
                nodeView.OnNodeMove = OnMoveSubtree;
                PopulateSubTree(subTreeNode);
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 createPosition = evt.mousePosition;

            {
                var types = TypeCache.GetTypesDerivedFrom<Composite>();
                foreach(var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] - {type.Name}", (_) => CreateNode(type, createPosition));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<Decorator>();
                foreach(var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] - {type.Name}", (_) => CreateNode(type, createPosition));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<Action>();
                foreach(var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] - {type.Name}", (_) => CreateNode(type, createPosition));
                }
            }
        }

        private void CreateNode(System.Type type, Vector2 nodePosition)
        {
            Node node = tree.CreateNode(type, nodePosition);
            CreateNodeView(node, Vector2.zero);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        private void OnUndoRedo()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }

        public void UpdateNodeState()
        {
            nodes.ForEach(n => {
                NodeView view = n as NodeView;
                view.UpdateState();
            });
        }

        internal BehaviorTree GetCurrentTree() => tree;

#region Sub-Tree Manipulation
        internal void DropTree(BehaviorTree tree, Vector2 mousePosition)
        {
            OnDroppedTree?.Invoke(tree, mousePosition);
        }

        public void AppendSubTree(BehaviorTree subTree, Vector2 mousePosition)
        {
            tree.AppendSubTree(subTree, mousePosition);
        }

        private void PopulateSubTree(SubTree subTreeNode)
        {
            BehaviorTree tree = subTreeNode.Tree;
            if (tree == null) return;

            Vector2 offsetPosition = subTreeNode.position + NodeView.SubTreeOffset;
            foreach (var node in tree.nodes)
            {
                CreateNodeView(node, offsetPosition, subTree: true);
            }

            tree.nodes.ForEach(n => {
                NodeView parentView = FindNodeView(n);
                var children = tree.GetChildren(n);

                children.ForEach(c => {
                    NodeView childView = FindNodeView(c);

                    Edge edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                });
            });
        }

        private void OnMoveSubtree(NodeView nodeView, Vector2 newPosition)
        {
            SubTree subTreeNode = nodeView.node as SubTree;
            if (subTreeNode == null || subTreeNode.Tree == null) return;

            subTreeNode.Tree.nodes.ForEach(n => {
                NodeView nodeView = FindNodeView(n);
                nodeView.SetOffsetPosition(newPosition + NodeView.SubTreeOffset);
            });
        }
#endregion
    }
}
#endif
