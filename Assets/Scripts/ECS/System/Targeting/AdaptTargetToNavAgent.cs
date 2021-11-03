using ECS.Component;
using Unity.Entities;
using UnityEngine.AI;
using NotImplementedException = System.NotImplementedException;

namespace ECS.System.Targeting
{
    public class AdaptTargetToNavAgent : SystemBase
    {
        
        EntityQuery query;
        
        
        protected override void OnCreate()
        {
            base.OnCreate();
            query = GetEntityQuery(typeof(Unit), typeof(UnitTarget));
            query.SetChangedVersionFilter(new ComponentType(typeof(UnitTarget)));
        }
        
        protected override void OnUpdate()
        {
            // Entities.ForEach((NavMeshAgent agent, ref UnitTarget unitTarget) =>
            // {
            //     agent.SetDestination(unitTarget.TargetPoint);
            // }).WithoutBurst().Run();
        }
    }
}