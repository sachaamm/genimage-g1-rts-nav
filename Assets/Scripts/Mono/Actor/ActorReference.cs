
    using System.Collections.Generic;
    using UnityEngine;

    public class ActorReference
    {
        public enum ElementAction
        {
            CreateWorker,
            CreateBuilding,
            None
        }
        
        public class ElementWithAction
        {
            public ActorReference.ElementAction ElementAction;
            public List<GameObject> ElementsForAction;
        }

        public static bool ElementWithActionListAlreadyContainsElementAction(List<ElementWithAction> elementWithActions, ElementAction elementAction)
        {
            foreach (ElementWithAction elementWithAction in elementWithActions)
            {
                if (elementWithAction.ElementAction == elementAction) return true;
            }
            
            return false;
        }

        public static ElementWithAction GetElementWithActionForElementActionInList(List<ElementWithAction> elementWithActions, ElementAction elementAction)
        {
            foreach (ElementWithAction elementWithAction in elementWithActions)
            {
                if (elementWithAction.ElementAction == elementAction) return elementWithAction;
            }

            Debug.LogError("L'element With action na pas été retrouvé dans la liste");
            
            return null;
        }
        
    }
