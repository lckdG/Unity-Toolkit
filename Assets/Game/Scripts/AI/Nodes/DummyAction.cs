using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeAI
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
