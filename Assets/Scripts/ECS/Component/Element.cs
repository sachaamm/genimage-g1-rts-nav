using Unity.Entities;

namespace ECS.Component
{
    public struct Element: IComponentData
    {
        public int uuid;
        public byte TeamIndex;
        
        public ElementReference.Element element;
        public void Execute()
        {
            
        }
    }

    
}