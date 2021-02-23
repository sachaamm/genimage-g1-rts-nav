using ECS.Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using NotImplementedException = System.NotImplementedException;

namespace ECS.System.Targeting
{
    public class RandomTarget : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var entityQuery = GetEntityQuery(typeof(Resource), typeof(Translation));

               // var translations =  entityQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

                Entities.ForEach((NavMeshAgent agent, ref UnitTarget unitTarget) =>
                {
                    
                    agent.SetDestination(unitTarget.TargetPoint);
                }).WithoutBurst().Run();

               //  translations.Dispose();
            }
        }
    }
}