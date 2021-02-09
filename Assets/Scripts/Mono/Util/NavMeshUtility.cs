using UnityEngine;
using UnityEngine.AI;

namespace Mono.Util
{
    public class NavMeshUtility : MonoBehaviour
    {
        public static Vector3 GetDiffNormalizedFromPosition(Vector3 start, Vector3 goal, int amnt)
        {
            // NavMeshHit hit;
            Vector3 diff = start - goal;
            return diff.normalized;
            // Vector3 result = new Vector3();
            //
            // if (NavMesh.SamplePosition(p, out hit, 10000.0f, NavMesh.AllAreas))
            // {
            //     result = hit.position;
            // }
            // else
            // {
            //     Debug.LogError("GetClosestPointInNavMeshFromPosition NavMeshHit failed");
            // }
            //
            // return result;
        } 
    }
}