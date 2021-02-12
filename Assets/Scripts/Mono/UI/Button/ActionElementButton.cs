using Mono.Actor;
using UnityEngine;
    
// Quand on clique bouton d'action ( ex: CreateWorker )
    public class ActionElementButton : MonoBehaviour
    {
        public ActorReference.ElementWithAction elementWithAction;
        public void Click()
        {
            Action.Singleton.InterpretAction(elementWithAction);
        }
    }
