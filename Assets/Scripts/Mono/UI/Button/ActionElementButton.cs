using Mono.Actor;
using Mono.Service;
using UnityEngine;
    
// Quand on clique bouton d'action ( ex: CreateWorker )
public class ActionElementButton : MonoBehaviour
{
    public ActorReference.ElementWithAction elementWithAction;
    public void Click()
    {
        // Action.Singleton.InterpretAction(elementWithAction);
        ActorReference.ElementAndAction elementAndAction = new ActorReference.ElementAndAction
        {
            Element = elementWithAction.Element,
            ElementAction = elementWithAction.ElementAction
        };
            
        SelectionService.ElementActionMessage(elementAndAction);
    }
}