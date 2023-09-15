namespace BehaviorTreeAI
{
    public interface IVisitee
    {
        public void Accept( INodeVisitor visitor );
    }
}
