using Mono.Actor;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Component
{
    public struct Unit : IComponentData
    {
        public ActorReference.ElementAction ElementAction;
        public bool stuckInTrigger;
        public int stuckInTriggerCount;

        public float3 TargetPoint;
        
        // OTHER STUCK POS AND 
        public Translation OtherStuckTranslation;
    }
}