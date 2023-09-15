using UnityEngine;

namespace AI.Tree
{
    // TODO: less exposure on blackboards
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

        void Start()
        {
            if ( cloneFrom != null )
            {
                tree = cloneFrom.Clone();
                tree.Setup();
            }
        }

        public void CloneFromTree( BehaviourTree tree )
        {
            this.tree = tree.Clone();
            this.tree.Setup();
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

        public void UpdateTree()
        {
            if ( tree != null )
            {
                tree.Update();
            }
        }

        public void ResetTree()
        {
            tree.Reset();
        }

#if UNITY_EDITOR
        public BehaviourTree GetTree()
        {
            return tree;
        }
#endif

    }
}
