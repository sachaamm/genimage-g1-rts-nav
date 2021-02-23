using Unity.Entities;

namespace ECS.Component.Learning
{
    public struct PrefabEntityReference : IComponentData
    {
        public Entity Prefab;
    }
}