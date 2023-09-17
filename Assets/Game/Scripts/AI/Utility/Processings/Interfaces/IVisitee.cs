namespace AI.Tree
{
    public interface IVisitee
    {
        public void Accept( INodeVisitor visitor );
    }
}
