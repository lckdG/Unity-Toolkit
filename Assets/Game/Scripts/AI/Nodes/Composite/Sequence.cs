using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Tree
{
    public class Sequence : Composite
    {
        [HideInInspector] private int current;

        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Node child = children[ current ];

            switch( child.Update() )
            {
                case State.EXECUTING:
                    return State.EXECUTING;
                case State.FAILED:
                    return State.FAILED;
                case State.SUCCESS:
                    current++;
                    break;
            }

            return current == children.Count ? State.SUCCESS : State.EXECUTING;
        }
    }
}
