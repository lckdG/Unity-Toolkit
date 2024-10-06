using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevToolkit.AI
{
    public class SubTree : Decorator
    {
        [SerializeField] private BehaviorTree subTree;

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            return subTree.Update();
        }
    }
}

