using Unity.Entities;

namespace ECS.Component.Authoring
{
    [GenerateAuthoringComponent]
    public struct PrefabEntityComponent : IComponentData
    {
        public Entity prefabEntity;
    }
}