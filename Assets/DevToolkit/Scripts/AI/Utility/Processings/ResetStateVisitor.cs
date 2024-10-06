namespace DevToolkit.AI
{
    public class ResetStateVisitor : INodeVisitor
    {
        public void Visit(Root root)
        {
            root.Reset();
        }

        public void Visit(Decorator decorator)
        {
            decorator.Reset();
        }

        public void Visit(Composite composite)
        {
            composite.Reset();
        }

        public void Visit(Action action)
        {
            action.Reset();
        }
    }
}
