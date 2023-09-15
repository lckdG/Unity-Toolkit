using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Tree
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
