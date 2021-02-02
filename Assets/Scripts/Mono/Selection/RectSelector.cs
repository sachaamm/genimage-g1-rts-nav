using System;
using System.Collections.Generic;
using DefaultNamespace.Element;
using Scriptable.Scripts;
using UnityEngine;


    public class RectSelector : MonoBehaviour
    {
        public GameObject rectSelector;

        public RectTransform rectSelectorTransform;
        
        private Vector3 startMousePos; // La position de la souris au moment ou on a commencé à cliquer
        private Vector3 startMouseWorldPos;

        public Transform elementsParent;

        public GameObject cube;
        
        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                startMousePos = Input.mousePosition;
                
                startMouseWorldPos = RaycastUtility.RaycastPosition();
                
            }
            
            if (Input.GetMouseButton(0))
            {
                rectSelector.SetActive(true);
                DrawRect();
            }
            else
            {
                rectSelector.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                List<GameObject> unitsInRect = UnitsInRect();
                
                
                // List<ActorReference.ElementAction> actionsPossibles = new List<ActorReference.ElementAction>();
                List<ActorReference.ElementWithAction> elementActions = new List<ActorReference.ElementWithAction>();

                foreach (GameObject selected in unitsInRect)
                {
                    ElementIdentity elementIdentity = selected.GetComponent<ElementIdentity>();

                    ElementScriptable elementScriptable =
                        ElementManager.Singleton.GetElementScriptableForElement(elementIdentity.Element);

                    List<GameObject> selectedForActions = new List<GameObject>();
                    selectedForActions.Add(selected);
                    
                    foreach (ActorReference.ElementAction actionPossiblePourCetElement in elementScriptable.PossibleActions)
                    {
                        ActorReference.ElementWithAction elementWithAction = new ActorReference.ElementWithAction();
                        elementWithAction.ElementAction = actionPossiblePourCetElement;
                        elementWithAction.ElementsForAction = selectedForActions;
                        
                        // si cette action de type ElementAction n'est pas deja présente dans la liste "elementsAction",
                        
                        if (!ActorReference.ElementWithActionListAlreadyContainsElementAction(elementActions, actionPossiblePourCetElement))
                        {
                            // -> je l'ajoute
                            elementActions.Add(elementWithAction);
                        }
                        else // si cette action de type ElementAction est présente dans la liste "elementsAction",
                        {
                            // je récupére l'objet correspondant dans la liste

                            ActorReference.ElementWithAction existingElementWithAction =
                                ActorReference.GetElementWithActionForElementActionInList(elementActions,
                                    actionPossiblePourCetElement);

                            // -> j'ajoute le gameObject a la liste de elementWithAction.ElementsForAction correspondant 
                            existingElementWithAction.ElementsForAction.Add(selected);

                        }
       
                    }
                }
                
                Debug.Log(unitsInRect.Count);
                
                Selection.Singleton.ReceiveSelection(unitsInRect);
                UiManager.Singleton.UpdateGroupLayout(unitsInRect);
                UiManager.Singleton.UpdateActionsLayout(elementActions);
                
            }
            
        }

        
        

        void DrawRect()
        {
            
            Vector2 delta = startMousePos - Input.mousePosition;

            float width = delta.x;
            float absWidth = Mathf.Abs(delta.x);
            float height = delta.y;
            float absHeight = Mathf.Abs(delta.y);
            
            // On va définir la taille du rectangle selon ou on a cliqué
            rectSelectorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, absHeight);
            rectSelectorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, absWidth);


            rectSelectorTransform.anchoredPosition = startMousePos -
                                                     new Vector3(Screen.width / 2, Screen.height / 2, 0)
                                                     -
                                                     new Vector3(width/2, height/2, 0);


        }


        List<GameObject> UnitsInRect()
        {
            List<GameObject> unitsInRect = new List<GameObject>();
            
            Vector3 screenPosToWorldPoint = RaycastUtility.RaycastPosition();

            // le point minimum entre screenPosToWorldPoint et startMouseWorldPos
            // le point maximum entre screenPosToWorldPoint et startMouseWorldPos

            Vector3 min = Vector3.Min(startMouseWorldPos, screenPosToWorldPoint);
            Vector3 max = Vector3.Max(startMouseWorldPos, screenPosToWorldPoint);
            
            foreach (Transform child in elementsParent)
            {
                if (InBounds(min, max, child.position))
                {
                    unitsInRect.Add(child.gameObject);
                }
            }
            
            return unitsInRect;
        }


        bool InBounds(Vector3 min, Vector3 max, Vector3 pos)
        {
            if (pos.x > min.x && pos.x < max.x && pos.z > min.z && pos.z < max.z) return true;
            return false;
        }

        
    }
