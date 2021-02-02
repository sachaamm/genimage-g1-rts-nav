using UnityEngine;

    public class ActionElementButton : MonoBehaviour
    {
        public ActorReference.ElementWithAction elementWithAction;
        public void Click()
        {
            Action.Singleton.InterpretAction(elementWithAction);
        }
    }
