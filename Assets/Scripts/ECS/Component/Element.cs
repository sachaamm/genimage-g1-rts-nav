using Unity.Entities;

namespace ECS.Component
{
    public struct Element: IComponentData
    {
        public int uuid;
        public ElementReference.Element element;
    }

    
}