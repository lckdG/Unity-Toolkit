using UnityEngine;

namespace AI.Tree
{
    public class SimpleParallel : Composite
    {
        [Tooltip("Finish mode of this Parallel. Use IMMEDIATE to finish this node when main action is completed, use DELAYED to wait for any left-over actions.")]
        [SerializeField] private FinishMode finishMode;
       
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
            foreach (Node child in children)
            {
                child.Reset();
            }
        }

        protected override State OnUpdate()
        {
            Node mainAction = children[0];
            Node subAction = children[1];

            if (finishMode == FinishMode.IMMEDIATE)
            {
                state = ExecuteNode(mainAction);
                if (IsExecuting() && subAction.IsExecuting())
                {
                    ExecuteNode(subAction);
                }

                if (IsExecuting() == false)
                {
                    ResetStateVisitor resetStateVisitor = new ResetStateVisitor();
                    
                    IVisitee vistee = subAction as IVisitee;
                    vistee?.Accept(resetStateVisitor);
                }

                return state;
            }
            else
            {
                if (mainAction.IsExecuting())
                {
                    ExecuteNode(mainAction);
                }

                if (subAction.IsExecuting())
                {
                    ExecuteNode(subAction);
                }

                if (mainAction.IsExecuting() == false && subAction.IsExecuting() == false)
                {
                    state = State.SUCCESS;
                }
                else
                {
                    state = State.EXECUTING;
                }

                return state;
            }
        }

        private State ExecuteNode(Node node)
        {
            State state = node.Update();
            return state;
        }
    }
}
