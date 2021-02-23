using ECS.Component;
using Mono.Actor;
using Mono.Util;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.System.Targeting
{
    class TargetingUtility
    {
        public static void SetTargetPoint(Vector3 p, ref Unit unit, ref Translation translation, NavMeshAgent navMeshAgent)
        {
            // soit le point p est sur le navmesh

            // soit le point p est sur le navmesh
            NavMeshHit hit;

            // Par rapport à l'endroit idéal ou on veut aller, on veux récupérer la position la plus proche contenue dans le navMesh
            
            // on utilise position qui est un décalage de la position p vers le joueur
            Vector3 position = p + NavMeshUtility.GetDiffNormalizedFromPosition(translation.Value, p,1);
            
            if (NavMesh.SamplePosition(position, out hit, 10000.0f, NavMesh.AllAreas))
            {
                // unit.TargetPoint = hit.position; // BROKEN
            }
            else
            {
                Debug.LogError("NavMeshHit failed");
            }
            
            if (ActorReference.IsMovingAction(unit.ElementAction))
            {
                // si c'est une action de déplacement alors on va vers la cible
                
                // navMeshAgent.destination = unit.TargetPoint; // BROKEN
            }
            else
            {
                // si ce n'est pas une action de déplacement, la cible est soi-même
                navMeshAgent.destination = translation.Value;
            }
            
            // DebugUtility.InstantiateDebugPoint(p, "p");
            // DebugUtility.InstantiateDebugPoint(position, "position");
            // DebugUtility.InstantiateDebugPoint(targetPoint, "TargetPoint");
            
        }
    }
}