using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeAI
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField] protected BehaviourTree cloneFrom;
        private BehaviourTree tree;

        void Awake()
        {
            if ( cloneFrom != null )
            {
                tree = cloneFrom.Clone();
            }
        }

        public void CloneFromTree( BehaviourTree tree )
        {
            this.tree = tree.Clone();
        }

        public void SetBlackboardTarget( MonoBehaviour target )
        {
            if ( tree )
            {
                tree.SetBlackboardTarget( target );
            }
        }

        public void SetBlackboardData( BlackboardObjectType type, string key, object data )
        {
            tree?.SetBlackBoardData( type, key, data );
        }

        public BlackboardKeyMapping GetBlackboardData( string key )
        {
            return tree.GetBlackboardData( key );
        }

        public void SetupTree()
        {
            if ( tree )
            {
                tree.Setup();
            }
        }

        public void UpdateTree()
        {
            if ( tree != null )
            {
                tree.Update();
            }
        }

        internal BehaviourTree GetTree()
        {
            return tree;
        }

        public void ResetTree()
        {
            tree.Reset();
        }
    }
}
