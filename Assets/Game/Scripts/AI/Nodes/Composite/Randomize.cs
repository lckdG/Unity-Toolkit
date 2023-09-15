using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

namespace BehaviorTreeAI
{
    public class Randomize : Composite
    {
        private int executingChild = -1;
        protected override void OnStart()
        {
            executingChild = Random.Range( 0, children.Count );
        }

        protected override void OnStop()
        {
            executingChild = -1;
        }

        protected override State OnUpdate()
        {
            if ( executingChild == -1 )
            {
                state = State.FAILED;
                return state;
            }

            Node chosenChild = children[ executingChild ];
            state = chosenChild.Update();

            return state;
        }
    }
}
