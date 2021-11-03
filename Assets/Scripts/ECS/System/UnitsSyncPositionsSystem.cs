
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.AI;


namespace ECS.System
{
    
    partial class UnitsSyncPositionsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((NavMeshAgent navMeshAgent, ref Translation translation) =>
            {
                translation.Value = navMeshAgent.transform.position;
            }).WithoutBurst().Run();
        }
    
        // public UnitsSyncPositionsSystem(object @object, IntPtr method) : base(@object, method)
        // {
        //     
        // }
    }
}

