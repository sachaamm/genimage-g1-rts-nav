using UnityEngine;

    public class ActionElementButton : MonoBehaviour
    {

        // public ActorReference.ElementAction elementAction;
        //
        // // Les games objects qui sont propriétaires de l'action
        // public List<GameObject> ActionOwners;
        //

        public ActorReference.ElementWithAction elementWithAction;
        public void Click()
        {
            Action.Singleton.InterpretAction(elementWithAction);
        }
    }
