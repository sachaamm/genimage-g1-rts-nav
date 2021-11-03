using Unity.Entities;

namespace ECS.Component
{
    public struct Resource : IComponentData
    {
        public bool Available;
        public bool IsMineral;
        public int uuid;
        public void Execute()
        {
            
        }
    }
}