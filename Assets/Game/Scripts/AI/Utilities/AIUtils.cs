using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeAI
{
    public static class AIUtils
    {
        public static Vector3 GetNavMeshPosition( Transform transform , Vector3 worldPosition, float maxDistance, int area )
        {
            NavMeshHit hit;
            if ( NavMesh.SamplePosition( worldPosition, out hit, maxDistance, area ) == false )
            {
                return transform.position;
            }
            else
            {
                return hit.position;
            }
        }
    }
}

