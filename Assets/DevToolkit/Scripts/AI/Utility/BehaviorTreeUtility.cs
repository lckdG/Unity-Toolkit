using UnityEngine;
using UnityEngine.AI;

namespace DevToolkit.AI
{
    public static class BehaviorTreeUtility
    {
        public static Vector3 GetNavMeshPosition(Transform transform , Vector3 worldPosition, float maxDistance, int area)
        {
            NavMeshHit hit;
            if ( NavMesh.SamplePosition(worldPosition, out hit, maxDistance, area ) == false)
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
