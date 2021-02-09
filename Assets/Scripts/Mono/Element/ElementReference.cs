
    public class ElementReference
    {
        // Les différents Element
        public enum Element
        {
            House,
            Worker,
            Soldier,
            Caserne
        }

        // Définir si un élement est constructible 
        public static bool IsBuildingElement(Element element)
        {
            if (element == Element.House) return true;
            if (element == Element.Caserne) return true;

            return false;
        }

        // Définir si un élement est une unité
        public static bool IsUnitElement(Element element)
        {
            if (element == Element.Worker) return true;
            if (element == Element.Soldier) return true;
            
            return false;
        }
        
    }
