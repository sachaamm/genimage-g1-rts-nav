using System.Linq;
using ECS.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;

// using Random = Unity.Mathematics.Random;

namespace ECS.System
{
    public class SimpleTargetingSystem : SystemBase
    {
        EntityQuery query;
        private uint seed = 0;
        
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
            NativeArray<int> shuffledIndices = RandomInts(resourcesEntities.Length);
            Debug.Log("r e " + resourcesEntities.Length);
            
            var findRandomResourceJob = new FindRandomResourceJob
            {
                unitTypeHandle = GetComponentTypeHandle<Unit>(),
                unitTargetTypeHandle = GetComponentTypeHandle<UnitTarget>(),
                positions = resourcesEntities,
                shuffledIndices = shuffledIndices
                // BaseSeed = seed
            };
            
            this.Dependency =
                this.Dependency
                    = findRandomResourceJob.ScheduleParallel(query, 1, this.Dependency);
            
            this.Dependency.Complete();
            
            resourcesEntities.Dispose();
            shuffledIndices.Dispose();

        }

        public NativeArray<int> RandomInts(int size)
        {
            NativeArray<int> b = new NativeArray<int>(size, Allocator.TempJob);

            Random rnd=new Random();
            
            int[] a = IndexArray(size);
            int[] c = a.OrderBy(x => rnd.Next()).ToArray();

            for (int i = 0; i < size; i++)
            {
                b[i] = c[i];
            }
            
            return b;
        }

        public int[] IndexArray(int size)
        {
            int[] array = new int[size];
            
            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }

            return array;
        }
        
        public struct FindRandomResourceJob : IJobEntityBatchWithIndex
        {
            public ComponentTypeHandle<Unit> unitTypeHandle;
            public ComponentTypeHandle<UnitTarget> unitTargetTypeHandle;
            [NativeDisableParallelForRestriction]
            public NativeArray<Translation> positions;
            public NativeArray<int> shuffledIndices;
            
            // [BurstCompile]
            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery)
            {
                var units = batchInChunk.GetNativeArray(unitTypeHandle);
                var unitsTargets = batchInChunk.GetNativeArray(unitTargetTypeHandle);
                
                for (int i = 0; i < unitsTargets.Length; i++)
                {
                    unitsTargets[i] = new UnitTarget
                    {
                        TargetPoint = positions[shuffledIndices[i]].Value
                    };
                }
            }
        }

        
    }
}