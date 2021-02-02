
    public class ElementReference
    {
        public enum Element
        {
            House,
            Worker,
            Soldier
        }

        public static bool IsBuildingElement(Element element)
        {
            if (element == Element.House) return true;
            
            return false;
        }


        public static bool IsUnitElement(Element element)
        {
            if (element == Element.Worker) return true;
            if (element == Element.Soldier) return true;
            
            return false;
        }
        
    }
