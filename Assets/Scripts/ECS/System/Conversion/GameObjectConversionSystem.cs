using Unity.Entities;

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
class PrefabConverterDeclare : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((PrefabReference prefabReference) =>
        {
            DeclareReferencedPrefab(prefabReference.Prefab);
        });
    }
}