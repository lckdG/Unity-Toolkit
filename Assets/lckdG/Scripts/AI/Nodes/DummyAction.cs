namespace AI.Tree
{
    public class DummyAction : Action
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return State.SUCCESS;
        }
    }
}
