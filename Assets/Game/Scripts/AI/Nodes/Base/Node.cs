using System.Collections.Generic;
using UnityEngine;

namespace AI.Tree
{
    public abstract class Node : ScriptableObject
    {
        public State state = State.EXECUTING;
        [HideInInspector] public Node parent = null;
        [HideInInspector] public Vector2 position;
        [HideInInspector] public string guid;
        protected Blackboard blackboard;

        public bool started = false;

        public State Update()
        {
            if ( !started )
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if ( state == State.FAILED || state == State.SUCCESS )
            {
                OnStop();
                started = false;
            }

            return state;
        }

        public void SetBlackboard( Blackboard board )
        {
            blackboard = board;
        }

        public virtual Node Clone()
        {
            return Instantiate( this );
        }

        public bool IsSucceeded() => state == State.SUCCESS;
        public bool IsFailed() => state == State.FAILED;
        public bool IsExecuting() => state == State.EXECUTING;
        
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        public virtual void Reset()
        {
            state = State.EXECUTING;
            started = false;
        }
    }
}
