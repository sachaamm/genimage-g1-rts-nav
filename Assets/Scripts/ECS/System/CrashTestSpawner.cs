using ECS.Component;
using ECS.System.Targeting;
using Mono.Actor;
using Mono.Element;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using NotImplementedException = System.NotImplementedException;

namespace ECS.System
{
    public class CrashTestSpawner : ComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            int spawnNb = 1;
            for (int i = 0; i < spawnNb; i++)
            {
                ElementManager.Singleton.InstantiateElement(ElementReference.Element.Worker, EnemyManager.RandomSpawnPos(20),0);
            }
        }

   
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                int spawnNb = 1;
                for (int i = 0; i < spawnNb; i++)
                {
                    ElementManager.Singleton.InstantiateElement(ElementReference.Element.Worker, EnemyManager.RandomSpawnPos(20),0);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                int spawnNb = 20;
                for (int i = 0; i < spawnNb; i++)
                {
                    ElementManager.Singleton.InstantiateElement(ElementReference.Element.Worker, EnemyManager.RandomSpawnPos(20),0);
                }
            }
            
            if (Input.GetKey(KeyCode.H))
            {
                ElementManager.Singleton.InstantiateElement(ElementReference.Element.House, EnemyManager.RandomSpawnPos(20),0);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                EntityQuery entityQuery = GetEntityQuery(typeof(Resource), typeof(Translation));
                var positions = entityQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

                Entities.ForEach((NavMeshAgent navMeshAgent, ref Unit unit, ref Translation translation) =>
                {
                    unit.ElementAction = ActorReference.ElementAction.MoveToResource;

                    // TargetingUtility.SetTargetPoint(
                    //     positions[(int)UnityEngine.Random.Range(0,positions.Length)].Value, ref unit, ref translation, navMeshAgent);

                });

                positions.Dispose();
            }
        }
    }
}