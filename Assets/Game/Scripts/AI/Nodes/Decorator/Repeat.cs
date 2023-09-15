using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeAI
{
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
