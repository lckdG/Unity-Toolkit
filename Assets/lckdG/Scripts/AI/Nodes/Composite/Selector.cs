namespace DevToolkit.AI
{
    public class Selector : Composite
    {
        Node lastChild = null;
        protected override void OnStart() { }

        protected override void OnStop()
        {
            lastChild = null;
        }

        protected override State OnUpdate()
        {
            foreach(Node _ in children)
            {
                state = _.Update();
                if (IsFailed() == false)
                {
                    if (lastChild != _)
                    {
                        ResetStateVisitor resetStateVisitor = new ResetStateVisitor();
                        IVisitee visitee = lastChild as IVisitee;
                        visitee?.Accept(resetStateVisitor);

                        lastChild = _;
                    }

                    return state;
                }
            }

            state = State.FAILED;
            return state;
        }
    }
}
