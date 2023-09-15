#if UNITY_EDITOR

using System;

using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BehaviorTreeAI
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {

        public Action<NodeView> OnNodeSelected;
        public BehaviorTreeAI.Node node;

        public Port input;
        public Port output;
        public Port subOutput;

        public NodeView( BehaviorTreeAI.Node node ) : base(BehaviourTreeEditor.editorPath + "NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            SetUpClass();

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void SetUpClass()
        {
            if ( node is BehaviorTreeAI.Action )
            {
                AddToClassList("action");
            }
            else if ( node is Composite )
            {
                AddToClassList("composite");
            }
            else if ( node is Decorator )
            {
                AddToClassList("decorator");
            }
            else if ( node is Root )
            {
                AddToClassList("root");
            }

        }

        private void CreateInputPorts()
        {
            if ( node is BehaviorTreeAI.Action )
            {
                input = InstantiatePort( Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool) );
            }
            else if ( node is Composite )
            {
                input = InstantiatePort( Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool) );
            }
            else if ( node is Decorator )
            {
                input = InstantiatePort( Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool) );
            }
            else if ( node is Root )
            { }

            if ( input != null )
            {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add( input );
            }
        }

        private void CreateOutputPorts()
        {
            if ( node is BehaviorTreeAI.Action ) { }
            else if ( node is SimpleParallel )
            {
                output = InstantiatePort( Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool) );
                subOutput = InstantiatePort( Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool) );
            }
            else if ( node is Composite )
            {
                output = InstantiatePort( Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool) );
            }
            else if ( node is Decorator || node is Root )
            {
                output = InstantiatePort( Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool) );
            }

            if ( output != null )
            {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add( output );
            }

            if ( subOutput != null )
            {
                subOutput.portName = "";
                subOutput.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add( subOutput );
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject( node, "Behaviour Tree (Set Position)");

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;

            EditorUtility.SetDirty( node );
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if ( OnNodeSelected != null )
            {
                OnNodeSelected.Invoke( this );
            }
        }

        public void SortChildren()
        {
            Composite composite = node as Composite;
            if ( composite )
            {
                composite.children.Sort( SortByHorizontalPosition );
            }
        }

        private int SortByHorizontalPosition( BehaviorTreeAI.Node left, BehaviorTreeAI.Node right )
        {
            return left.position.x < right.position.x ? -1 : 1;
        }

        public void UpdateState()
        {
            RemoveFromClassList( "executing" );
            RemoveFromClassList( "failed" );
            RemoveFromClassList( "success" );

            if ( !Application.isPlaying ) return;
        
            switch( node.state )
            {
                case State.EXECUTING:
                    if ( node.started )
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