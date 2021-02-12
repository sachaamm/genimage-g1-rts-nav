using ECS.Component;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.AI;
using NotImplementedException = System.NotImplementedException;

namespace ECS.System
{
    public class UnitsSyncPositionsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((NavMeshAgent navMeshAgent, ref Translation translation) =>
            {
                translation.Value = navMeshAgent.transform.position;
            }).WithoutBurst().Run();
        }
    }
}