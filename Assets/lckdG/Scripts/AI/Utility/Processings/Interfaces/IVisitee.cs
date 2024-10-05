namespace DevToolkit.AI
{
    public interface IVisitee
    {
        public void Accept(INodeVisitor visitor);
    }
}
