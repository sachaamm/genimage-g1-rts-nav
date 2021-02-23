using ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.System
{
    public class SimpleTargetingSystem : SystemBase
    {
        EntityQuery query;
        
        
        protected override void OnCreate()
        {
            base.OnCreate();
            query = GetEntityQuery(typeof(Unit), typeof(UnitTarget));
            query.SetChangedVersionFilter(new ComponentType(typeof(Unit)));
        }
        
        protected override void OnUpdate()
        {
            EntityQuery resources = GetEntityQuery(typeof(Resource), typeof(Translation));
            NativeArray<Translation> resourcesEntities = resources.ToComponentDataArray<Translation>(Allocator.TempJob);
            
            var findRandomResourceJob = new FindRandomResourceJob
            {
                unitTypeHandle = GetComponentTypeHandle<Unit>(),
                unitTargetTypeHandle = GetComponentTypeHandle<UnitTarget>(),
                positions = resourcesEntities
            };
            
            this.Dependency =
                this.Dependency
                    = findRandomResourceJob.ScheduleParallel(query, 1, this.Dependency);
            
            this.Dependency.Complete();
            
            resourcesEntities.Dispose();
            
        }
        
        public struct FindRandomResourceJob : IJobEntityBatchWithIndex
        {
            public ComponentTypeHandle<Unit> unitTypeHandle;
            public ComponentTypeHandle<UnitTarget> unitTargetTypeHandle;
            [NativeDisableParallelForRestriction]
            public NativeArray<Translation> positions;
            // [ReadOnly] public uint BaseSeed;
            
            // [BurstCompile]
            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery)
            {
                var units = batchInChunk.GetNativeArray(unitTypeHandle);
                var unitsTargets = batchInChunk.GetNativeArray(unitTargetTypeHandle);
                
                for (int i = 0; i < unitsTargets.Length; i++)
                {
                    // var seed = (uint)(i+1);
                    // Random rnd = new Unity.Mathematics.Random(seed);

                    int a = (int)(UnityEngine.Random.value * unitsTargets.Length);

                    unitsTargets[i] = new UnitTarget
                    {
                        TargetPoint = positions[a].Value
                    };
                    // units[i] = 
                }
            }
        }

        
    }
}