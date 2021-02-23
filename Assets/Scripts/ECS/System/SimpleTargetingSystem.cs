using ECS.Component;
using Unity.Entities;
using Unity.Jobs;

namespace ECS.System
{
    public class SimpleTargetingSystem : ComponentSystem
    {
        EntityQuery query;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            query = GetEntityQuery(typeof(Unit));
            query.SetChangedVersionFilter(new ComponentType(typeof(Unit)));
        }
        
        protected override void OnUpdate()
        {
            var findTargetJob = new FindTargetJob
            {
                unitTypeHandle = GetComponentTypeHandle<Unit>()
            };
            
            // JobHandle jobHandle = findTargetJob.Schedule(this, inputDeps);
        
        }
        
        public struct FindTargetJob : IJobEntityBatchWithIndex
        {
            public ComponentTypeHandle<Unit> unitTypeHandle;
            
            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery)
            {
                var units = batchInChunk.GetNativeArray(unitTypeHandle);
                
            }
        }

        
    }
}