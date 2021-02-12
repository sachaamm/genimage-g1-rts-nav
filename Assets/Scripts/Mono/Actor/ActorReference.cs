
    using System.Collections.Generic;
    using UnityEngine;

    public class ActorReference
    {
        // Enumeration décrivant les différentes actions possibles d'un élément
        public enum ElementAction
        {
            // CreateWorker, // par exemple une maison peut produire un ouvrier
            CreateBuilding,
            None,
            // CreateSoldier,
            Attack,
            MoveToEnemy,
            MoveToPoint,
            MoveToResource,
            BringBackResource,
            SeekClosestResource,
            CreateUnit,
        }
        
        
        // Un objet contentant une ElementAction appliquée à une liste de GameObject
        public class ElementWithAction
        {
            public ActorReference.ElementAction ElementAction;
            public List<GameObject> ElementsForAction;
        }

        public class ElementWithActionCreateUnit : ElementWithAction
        {
            public ElementReference.Element UnitToCreate;
        }

        
        // Elle renvoie un booléen qui correspond à la présence d'une ElementAction dans une liste de ElementWithAction
        public static bool ElementWithActionListAlreadyContainsElementAction(
            List<ElementWithAction> elementWithActions, 
            ElementAction elementAction)
        {
            foreach (ElementWithAction elementWithAction in elementWithActions)
            {
                if (elementWithAction.ElementAction == elementAction) return true;
            }
            
            return false;
        }


        // Elle renvoie un ElementWithAction pour une ElementAction donnée dans une liste de ElementWithAction
        public static ElementWithAction GetElementWithActionForElementActionInList(
            List<ElementWithAction> elementWithActions, 
            ElementAction elementAction)
        {
            foreach (ElementWithAction elementWithAction in elementWithActions)
            {
                if (elementWithAction.ElementAction == elementAction) return elementWithAction;
            }

            Debug.LogError("L'element With action na pas été retrouvé dans la liste");
            
            return null;
        }
        
    }
