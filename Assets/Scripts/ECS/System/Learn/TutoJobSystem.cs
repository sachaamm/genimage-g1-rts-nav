// using ECS.Component;
// using JetBrains.Annotations;
// using Mono.Ecs;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Jobs;
// using Unity.Mathematics;
// using Unity.Transforms;
// using NotImplementedException = System.NotImplementedException;
//
// namespace ECS.System.Learn
// {
//     public class TutoJobSystem : JobComponentSystem
//     {
//
//         [BurstCompile]
//         struct FindTargetJob : IJobChunk
//         {
//             // Tableau des ressources disponibles
//             public NativeArray<Entity> Resources;
//             public NativeArray<Resource> ResourcesResources;
//             
//             public static int resourcesIndex = 0;
//             public ArchetypeChunkComponentType<UnitTarget> UnitTargetType;
//             public ArchetypeChunkComponentType<Unit> UnitType;
//             
//             public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
//             {
//                 NativeArray<UnitTarget> chunkUnitTargets = chunk.GetNativeArray(UnitTargetType);
//                 NativeArray<Unit> chunkUnits = chunk.GetNativeArray(UnitType);
//                 
//                 for (var i = 0; i < chunk.Count; i++)
//                 {
//                     // si la cible est nulle, on cible la plus proche. 
//                     // on cible uniquement quand on a changé d'action pour une unité
//
//                     // ResourcesResources[0];
//                     
//                     resourcesIndex++;
//                 }
//             }
//         }
//         
//         // private EndSimulationEntityCommandBufferSystem endSimulation;
//
//         protected override void OnCreate()
//         {
//             base.OnCreate();
//
//             // endSimulation = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//         }
//
//         [BurstCompile]
//         protected override JobHandle OnUpdate(JobHandle inputDeps)
//         {
//             // ecb forbidden in burst 
//             // EntityCommandBuffer.Concurrent entityCommandBuffer = endSimulation.CreateCommandBuffer().ToConcurrent();
//             // entityCommandBuffer.AddComponent();
//             
//             NativeArray<Entity> myEntityArray = new NativeArray<Entity>();
//
//             Entities.WithAll<Unit>().ForEach((ref Translation translation) =>
//                 {
//                     World.DefaultGameObjectInjectionWorld.EntityManager.Exists(myEntityArray[0]);
//                 }
//             ).Schedule(inputDeps);
//  
//             return inputDeps;
//         }
//     }
// }