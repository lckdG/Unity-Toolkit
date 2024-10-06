#if UNITY_EDITOR

using System;

using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DevToolkit.AI.Editor
{
    internal class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public static Vector2 SubTreeOffset = 125f * Vector2.up;
        public Action<NodeView> OnNodeSelected;
        public Action<NodeView, Vector2> OnNodeMove;
        public Node node;

        public Port input;
        public Port output;
        public Port subOutput;

        public NodeView(Node node, Vector2 offset, bool subTree) : base(BehaviorTreeEditor.editorPath + "Visuals\\NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x + offset.x;
            style.top = node.position.y + offset.y;

            SetUpClass(subTree);

            CreateInputPorts(subTree);
            CreateOutputPorts(subTree);
        }

        private void SetUpClass(bool subTree)
        {
            if (node is Action)
            {
                AddToClassList("action");
            }
            else if (node is Composite)
            {
                AddToClassList("composite");
            }
            else if (node is Decorator)
            {
                AddToClassList("decorator");
            }
            else if (node is Root)
            {
                AddToClassList("root");
                capabilities ^= Capabilities.Movable ^ Capabilities.Deletable;
            }

            if (subTree)
            {
                capabilities = 0;
            }
        }

        private void CreateInputPorts(bool subTree)
        {
            if ((node is Root && subTree) || node is Action || node is Composite || node is Decorator)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            if (input != null)
            {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);

                input.SetEnabled(subTree == false);
            }
        }

        private void CreateOutputPorts(bool subTree)
        {
            if (node is Action) { }
            else if (node is SimpleParallel)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                subOutput = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Composite)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (node is Decorator || node is Root)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }

            if (output != null)
            {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);

                output.SetEnabled(subTree == false);
            }

            if (subOutput != null)
            {
                subOutput.portName = "";
                subOutput.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(subOutput);

                subOutput.SetEnabled(subTree == false);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(node, "Behaviour Tree (Set Position)");

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;

            OnNodeMove?.Invoke(this, node.position);

            EditorUtility.SetDirty(node);
        }

        public void SetOffsetPosition(Vector2 offset)
        {
            style.left = node.position.x + offset.x;
            style.top = node.position.y + offset.y;
        }

        public override void OnSelected()
        {
            base.OnSelected();

            Selection.activeObject = node;
            OnNodeSelected?.Invoke(this);
        }

        public void SortChildren()
        {
            Composite composite = node as Composite;
            if (composite)
            {
                composite.children.Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(Node left, Node right)
        {
            return left.position.x < right.position.x ? -1 : 1;
        }

        public void UpdateState()
        {
            RemoveFromClassList("executing");
            RemoveFromClassList("failed");
            RemoveFromClassList("success");

            if (Application.isPlaying == false) return;
        
            switch(node.state)
            {
                case State.EXECUTING:
                    if (node.started)
                    {
                        AddToClassList("executing");
                    }

                    break;

                case State.FAILED:
                    AddToClassList("failed");
                    break;

                case State.SUCCESS:
                    AddToClassList("success");
                    break;
            }
        }
    }
}

#endif