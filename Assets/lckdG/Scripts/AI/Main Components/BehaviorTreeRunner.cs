using UnityEngine;

namespace DevToolkit.AI
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        [SerializeField] protected BehaviorTree cloneFrom;
        private BehaviorTree tree;

#region Tree Setups
        void Awake()
        {
            if (cloneFrom != null)
            {
                tree = cloneFrom.Clone();
            }
        }

        void Start()
        {
            if (cloneFrom != null)
            {
                tree = cloneFrom.Clone();
                tree.Setup();
            }
        }

        public void CloneFromTree(BehaviorTree tree)
        {
            this.tree = tree.Clone();
            this.tree.Setup();
        }

        public void SetBlackboardTarget(MonoBehaviour target)
        {
            tree?.SetBlackboardTarget(target);
        }
#endregion

#region Tree Manipulations
        public void UpdateTree()
        {
            tree?.Update();
        }

        public void SetBlackboardData(BlackboardObjectType type, string key, object data)
        {
            tree?.SetBlackBoardData(type, key, data);
        }

        public void ResetTree()
        {
            tree.ResetState();
        }
#endregion

#if UNITY_EDITOR
        public BehaviorTree GetTree()
        {
            return tree;
        }
#endif
    }
}
