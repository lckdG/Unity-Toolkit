namespace DevToolkit.AI
{
    public abstract class Action : Node, IVisitee
    {
        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

}
