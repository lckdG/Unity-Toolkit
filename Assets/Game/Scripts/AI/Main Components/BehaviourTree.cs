using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace AI.Tree
{

    [CreateAssetMenu(menuName = "AI/Behaviour Tree")]
    public class BehaviourTree : ScriptableObject
    {
        public Node root;
        public State treeState = State.EXECUTING;
        public List<Node> nodes = new List<Node>();

        public Blackboard blackboardRef;
        private Blackboard blackboard;

        public void Setup()
        {
            blackboard.Setup();
            BindBlackboard();
        }

        public void BindBlackboard()
        {
            Traverse( root, ( n ) => {
                n.SetBlackboard ( blackboard );
            });
        }

        public void Traverse( Node node, System.Action<Node> visitor )
        {
            if ( node )
            {
                visitor.Invoke( node );
                var children = GetChildren( node );
                children.ForEach( child => Traverse( child, visitor ) );
            }
        }

        public void SetBlackboardTarget( MonoBehaviour target )
        {
            blackboard.SetTarget( target );
        }

        public void SetBlackBoardData( BlackboardObjectType type, string key, object data )
        {
            blackboard.SetOrAddData( type, key, data );
        }

        public BlackboardKeyMapping GetBlackboardData( string key )
        {
            return blackboard.GetData( key );
        }

        public State Update()
        {
            blackboard.Update();
            if ( root.state == State.EXECUTING )
            {
                treeState = root.Update();
            }

            return treeState;
        }

#if UNITY_EDITOR
        public Node CreateNode( Type type )
        {
            Node node = CreateInstance( type ) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject( this, "Behaviour Tree (CreateNode)");
            nodes.Add( node );

            if ( !Application.isPlaying )
            {
                AssetDatabase.AddObjectToAsset( node, this );
            }

            Undo.RegisterCreatedObjectUndo( node, "Behaviour Tree (CreateNode)");

            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode( Node node )
        {
            Undo.RecordObject( this, "Behaviour Tree (DeleteNode)");

            nodes.Remove( node );

            // AssetDatabase.RemoveObjectFromAsset( node );
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild( Node parent, Node child )
        {
            Root root = parent as Root;
            if ( root )
            {
                Undo.RecordObject( root, "Behaviour Tree (AddChild)");
                root.child = child;
                EditorUtility.SetDirty( root );
            }

            Decorator decorator = parent as Decorator;
            if ( decorator )
            {
                Undo.RecordObject( decorator, "Behaviour Tree (AddChild)");
                decorator.child = child;
                EditorUtility.SetDirty( decorator );
            }

            Composite composite = parent as Composite;
            if ( composite )
            {
                Undo.RecordObject( composite, "Behaviour Tree (AddChild)");
                composite.children.Add( child );
                EditorUtility.SetDirty( composite );
            }
        }

        public void RemoveChild( Node parent, Node child )
        {
            Root root = parent as Root;
            if ( root )
            {
                Undo.RecordObject( root, "Behaviour Tree (RemoveChild)");
                root.child = null;
                EditorUtility.SetDirty( root );
            }

            Decorator decorator = parent as Decorator;
            if ( decorator )
            {
                Undo.RecordObject( decorator, "Behaviour Tree (RemoveChild)");
                decorator.child = null;
                EditorUtility.SetDirty( decorator );
            }

            Composite composite = parent as Composite;
            if ( composite )
            {
                Undo.RecordObject( composite, "Behaviour Tree (RemoveChild)");
                composite.children.Remove( child );
                EditorUtility.SetDirty( composite );
            }
        }
#endif

        public List<Node> GetChildren( Node parent )
        {
            List<Node> children = new List<Node>();

            Root root = parent as Root;
            if ( root && root.child )
            {
                children.Add( root.child );
            }

            Decorator decorator = parent as Decorator;
            if ( decorator && decorator.child != null)
            {
                children.Add( decorator.child );
            }

            Composite composite = parent as Composite;
            if ( composite )
            {
                return composite.children;
            }

            return children;
        }

        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate( this );
            tree.root = this.root.Clone();
            tree.nodes = new List<Node>();

            if (blackboardRef != null)
            {
                tree.blackboard = blackboardRef.Clone();
            }

            Traverse( tree.root, n => tree.nodes.Add( n ) );

            return tree;
        }
    
        public bool InSameCondition( BlackboardKeyMapping mapping )
        {
            return blackboard.CompareKeyMapping( mapping );
        }

        public void Reset()
        {
            if ( root == null ) return;
            ResetStateVisitor resetVisitor = new ResetStateVisitor();
            (root as Root).Accept( resetVisitor );
        }
    }
}
