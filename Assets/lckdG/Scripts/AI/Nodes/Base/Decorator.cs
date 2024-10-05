using UnityEngine;

namespace AI.Tree
{
    public abstract class Decorator : Node, IVisitee
    {
        [HideInInspector] public Node child;

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);

            IVisitee visitee = child as IVisitee;
            visitee?.Accept(visitor);
        }

        public override Node Clone()
        {
            Decorator node = Instantiate(this);
            node.child = child.Clone();

            return node;
        }
    }
}
