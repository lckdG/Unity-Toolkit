using UnityEngine;

namespace DevToolkit.AI
{
    public class SubTree : Decorator
    {
        [SerializeField] public BehaviorTree Tree
        {
            get { return _tree; }
            set {
                _tree = value;
                OnUpdateSubTree();
            }
        }

        private BehaviorTree _tree;

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (Tree == null) return State.FAILED;
            return Tree.Update();
        }

        private void OnUpdateSubTree()
        {
            if (_tree == null)
            {
                this.child = null;
                return;
            }
            
            this.child = _tree.root;
        }
    }
}

