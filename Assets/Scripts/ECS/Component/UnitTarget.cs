using Unity.Entities;

namespace ECS.Component
{
    public struct UnitTarget : IComponentData
    {
        public Entity Target;
        public void Execute()
        {
            
        }
    }
}