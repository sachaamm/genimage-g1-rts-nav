using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Component
{
    public struct UnitTarget : IComponentData
    {
        // public Entity Target;
        public float3 TargetPoint;
    }
}