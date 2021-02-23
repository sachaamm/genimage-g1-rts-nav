// Conversion system, running in the conversion world

using Mono.Authoring;

class FooConversion : GameObjectConversionSystem
{
    protected override void OnUpdate()
    { 
        // Iterate over all authoring components of type FooAuthoring
        Entities.ForEach((FooAuthoring input) =>
        {
            // Get the destination world entity associated with the authoring GameObject
            var entity = GetPrimaryEntity(input);

            // Do the conversion and add the ECS component
            DstEntityManager.AddComponentData(entity, new Foo
            {
                SquaredValue = input.Value * input.Value
            });
        });
    }
}