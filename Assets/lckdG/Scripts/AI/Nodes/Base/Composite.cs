using System.Collections.Generic;
using UnityEngine;

namespace AI.Tree
{
    public abstract class Composite : Node, IVisitee
    {
        [HideInInspector] public List<Node> children = new List<Node>();

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);

            foreach(Node child in children)
            {
                IVisitee visitee = child as IVisitee;
                visitee?.Accept(visitor);
            }
        }

        public override Node Clone()
        {
            Composite node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());

            return node;
        }
    }
}
