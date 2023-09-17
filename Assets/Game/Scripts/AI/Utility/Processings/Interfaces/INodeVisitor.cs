namespace AI.Tree
{
    public interface INodeVisitor
    {
        public void Visit( Root root );
        public void Visit( Decorator decorator );
        public void Visit( Composite composite );
        public void Visit( Action action );
    }
}
