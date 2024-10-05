using UnityEngine;

namespace DevToolkit.AI
{
    public class Root : Node, IVisitee
    {
        [HideInInspector] public Node child;
        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            child.Update();
            return State.EXECUTING;
        }

        public override Node Clone()
        {
            Root node = Instantiate(this);
            node.child = child.Clone();

            return node;
        }

        public void Accept(INodeVisitor visitor)
        {
            IVisitee visitee = child as IVisitee;
            visitee.Accept( visitor );
        }
    }
}
