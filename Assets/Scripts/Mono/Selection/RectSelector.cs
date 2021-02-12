using System.Collections.Generic;
using DefaultNamespace.Element;
using Mono.Element;
using Scriptable.Scripts;
using UnityEngine;

// Script permettant de réaliser une selection à partir d'un rectangle 
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

            // Je commence la sélection quand j'enfonce le clique gauche
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

            // Je valide la sélection quand je relache le clique gauche
            if (Input.GetMouseButtonUp(0) && !Selection.Singleton.mouseOnGUI)
            {
                List<Selection.Selected> unitsInRect = ElementsInRect();

                List<ActorReference.ElementWithAction> elementActions = ElementWithActionsFromSelection(unitsInRect);
                
                Debug.Log(unitsInRect.Count);
                
                Selection.Singleton.ReceiveSelectionOnMouseUp(unitsInRect);
                UiManager.Singleton.UpdateGroupLayout(unitsInRect);
                UiManager.Singleton.UpdateActionsLayout(elementActions, unitsInRect);             
            }
            
        }
        public static List<ActorReference.ElementWithAction> ElementWithActionsFromSelection(List<Selection.Selected> selection)
        {
            List<ActorReference.ElementWithAction> elementActions = new List<ActorReference.ElementWithAction>();

                foreach (Selection.Selected selected in selection)
                {
                    ElementIdentity elementIdentity = selected.SelectedGameObject.GetComponent<ElementIdentity>();

                    ElementScriptable elementScriptable =
                        ElementManager.Singleton.GetElementScriptableForElement(elementIdentity.Element);

                    List<GameObject> selectedForActions = new List<GameObject>();
                    selectedForActions.Add(selected.SelectedGameObject);
                    
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
                            existingElementWithAction.ElementsForAction.Add(selected.SelectedGameObject);

                        }
       
                    }
                }

                return elementActions;
        }
        
        // Dessiner le rectangle de selection à l'ecran
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

        // La liste des élements selectionnés par le rectangle de sélection
        List<Selection.Selected> ElementsInRect()
        {
            List<Selection.Selected> unitsInRect = new List<Selection.Selected>();
            
            Vector3 screenPosToWorldPoint = RaycastUtility.RaycastPosition();

            // le point minimum entre screenPosToWorldPoint et startMouseWorldPos
            // le point maximum entre screenPosToWorldPoint et startMouseWorldPos

            Vector3 min = Vector3.Min(startMouseWorldPos, screenPosToWorldPoint);
            Vector3 max = Vector3.Max(startMouseWorldPos, screenPosToWorldPoint);
            
            foreach (Transform child in elementsParent)
            {
                if (InRectBounds(min, max, child.position))
                {
                    AddElementInSelection(child.gameObject, unitsInRect);
                }
            }
            
            return unitsInRect;
        }

        // Pour chaque élement contenu dans le rectangle de sélection, on l'ajoute à la selection
        public List<Selection.Selected> AddElementInSelection(GameObject element, List<Selection.Selected> selection)
        {
            ElementIdentity elementIdentity = element.GetComponent<ElementIdentity>();
            
            if (ElementReference.IsBuildingElement(elementIdentity.Element))
            {
                Selection.BuildingSelected buildingSelected = new Selection.BuildingSelected();
                buildingSelected.SelectedGameObject = element.gameObject;
                buildingSelected.SelectedElement = elementIdentity.Element;
                selection.Add(buildingSelected);
            }
                    
            if (ElementReference.IsUnitElement(elementIdentity.Element))
            {
                Selection.UnitSelected unitSelected = new Selection.UnitSelected();
                unitSelected.SelectedGameObject = element.gameObject;
                unitSelected.Unit = element.gameObject.GetComponent<Unit>();
                unitSelected.SelectedElement = elementIdentity.Element;
                selection.Add(unitSelected);
            }

            return selection;
        }
        
        // Position Contenue dans le rectangle de sélection
        bool InRectBounds(Vector3 min, Vector3 max, Vector3 pos)
        {
            if (pos.x > min.x && pos.x < max.x && pos.z > min.z && pos.z < max.z) return true;
            return false;
        }

        
    }
