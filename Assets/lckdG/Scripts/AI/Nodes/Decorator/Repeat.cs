namespace AI.Tree
{
    // TODO: change this to repeat by time, or by secs instead
    public class Repeat : Decorator
    {
        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            child.Update();
            return State.EXECUTING;
        }
    }
}
