
using DefaultNamespace.Element;
using Mono.Entity;
using Mono.UI;
using Scriptable.Scripts;
using UnityEngine;

    public class Action : MonoBehaviour
    {
        public static Action Singleton;

        public ActorReference.ElementAction currentAction = ActorReference.ElementAction.None;
        
        private void Awake()
        {
            Singleton = this;
        }
        
        private void Update()
        {
            if (currentAction == ActorReference.ElementAction.None)
            {
                MoveUnits();
            }

            if (currentAction == ActorReference.ElementAction.CreateBuilding)
            {
                PrepareToCreateBuilding();
            }
        
        }

        public void InterpretAction(ActorReference.ElementWithAction elementWithAction)
        {
            bool preparingAction = false; // preparingAction = une action en 2 temps ( ex: CreateBuilding ) 

            if (IsPreparingAction(elementWithAction.ElementAction))
            {
                // Soit on appelle une action de "préparation" : Préparer la construction d'un batiment avec la prévisualisation
                currentAction = elementWithAction.ElementAction;
            }
            else
            {
                // Soit on appelle une action "directe" : ex: Déplacer une unité, construire un ouvrier
                
                // J'effectue l'action instantanément
                InstantAction(elementWithAction);
            }
            
        }

        // Une action qui s'effectue instantanément
        void InstantAction(ActorReference.ElementWithAction elementWithAction)
        {
            ActorReference.ElementAction action = elementWithAction.ElementAction;
            
            if (action == ActorReference.ElementAction.CreateWorker)
            {
                CreateUnitInBuilding(elementWithAction, ElementReference.Element.Worker);
            }
            
            if (action == ActorReference.ElementAction.CreateSoldier)
            {
                CreateUnitInBuilding(elementWithAction, ElementReference.Element.Soldier);
            }
            
            
        }

        void CreateUnitInBuilding(ActorReference.ElementWithAction elementWithAction, ElementReference.Element element)
        {
            ElementScriptable elementScriptable =
                ElementManager.Singleton.GetElementScriptableForElement(element);

            foreach (GameObject go in elementWithAction.ElementsForAction)
            {
                Vector3 spawnPos = go.transform.position;
                
                ElementManager.Singleton.InstantiateElement(element, spawnPos);
            }
        }

        bool IsPreparingAction(ActorReference.ElementAction action)
        {
            return action == ActorReference.ElementAction.CreateBuilding;
        }

        void MoveUnits()
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {

                        if (HoveredTargetManager.Singleton.target == null)
                        {
                            Selection.Singleton.MoveSelection(hit.point);
                        }
                        else
                        {
                            EntityReference.Entity targetType = HoveredTargetManager.Singleton.targetType;

                            DefineActionForElements();

                        }
                        // Select(hit.collider.gameObject);
                        

                    }
                }
                else
                {

                }
            }

        }

        void DefineActionForElements()
        {
            foreach (Selection.Selected selected in Selection.Singleton.selection)
            {
                if (selected.GetType() == typeof(Selection.UnitSelected))
                {
                    Selection.UnitSelected unitSelected = selected as Selection.UnitSelected;
                    ElementReference.Element unit = unitSelected.SelectedElement;

                    if (unit == ElementReference.Element.Soldier)
                    {
                        
                    }

                    if (unit == ElementReference.Element.Worker)
                    {
                        if (HoveredTargetManager.Singleton.targetType == EntityReference.Entity.Resource)
                        {
                            // 
                            unitSelected.Unit.TargetPoint = HoveredTargetManager.Singleton.target.transform.position;
                            unitSelected.Unit.SetState(ActorReference.ElementAction.MoveToResource);
                            
                        }
                    }

                }

                if (selected.GetType() == typeof(Selection.BuildingSelected))
                {
                    
                }
            }
        }
        
        void PrepareToCreateBuilding()
        {
            Vector3 mousePos = RaycastUtility.RaycastPosition();
            ElementPlacer.Singleton.PrevisualizeBuildingGhost(mousePos, ElementReference.Element.House);

            if (Input.GetMouseButtonDown(0))
            {
                // Je cree le batiment en question a cet endroit

                ElementManager.Singleton.InstantiateElement(ElementReference.Element.House, mousePos);
                ElementPlacer.Singleton.StopPrevizualition();
                
                ResetCurrentAction();
            }
        }

        void ResetCurrentAction()
        {
            currentAction = ActorReference.ElementAction.None;
        }
        
        
    }
