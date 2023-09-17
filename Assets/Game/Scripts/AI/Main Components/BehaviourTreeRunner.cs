using UnityEngine;

namespace AI.Tree
{
    // TODO: less exposure on blackboards
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField] protected BehaviourTree cloneFrom;
        private BehaviourTree tree;

#region Tree Setups
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
#endregion

#region  Tree Manipulations
        public void UpdateTree()
        {
            if ( tree != null )
            {
                tree.Update();
            }
        }

        public void SetBlackboardData( BlackboardObjectType type, string key, object data )
        {
            tree?.SetBlackBoardData( type, key, data );
        }

        public void ResetTree()
        {
            tree.ResetState();
        }
#endregion

#if UNITY_EDITOR
        public BehaviourTree GetTree()
        {
            return tree;
        }
#endif

    }
}
