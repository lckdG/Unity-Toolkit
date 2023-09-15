using UnityEngine;

namespace AI.Tree
{
    public class Root : Node, IVisitee
    {
        [HideInInspector] public Node child;
        protected override void OnStart() { }

        protected override void OnStop()
        { }

        protected override State OnUpdate()
        {
            return child.Update();
        }

        public override Node Clone()
        {
            Root node = Instantiate( this );
            node.child = child.Clone();

            return node;
        }

        public void Accept(INodeVisitor visitor)
        {
            IVisitee visitee = (child as IVisitee);
            visitee.Accept( visitor );
        }
    }
}
