using Mono.Actor;
using Mono.Element;
using Mono.Entity;
using Mono.Service;
using Mono.UI;
using Scriptable.Scripts;
using UnityEngine;

/// <summary>
/// Script executant les actions disponibles de la selection active( exemple : Construire un batiment / une unité )
/// </summary>
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

        public void InterpretCreateBuildingAction(ElementReference.Element element)
        {
            currentAction = ActorReference.ElementAction.CreateBuilding;
            ElementPlacer.Singleton.elementTypeOfNewBuilding = element;
        }

        

        // je déplace les unités de la selection avec le clic droit 
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
                            // Selection.Singleton.MoveSelection(hit.point);
                        }
                        else
                        {
                            // comme j'ai right click sur un élément, je ne me déplace pas mais je définis l'action à effectuer en fonction de l'Element
                            EntityReference.Entity targetType = HoveredTargetManager.Singleton.targetType;
                            DefineActionForElements();
                        }
                    }
                }
                else
                {

                }
            }

        }

        // Définir l'action possible pour l'Element, selon si c'est une Unité ou pas, quel type d'unité etc
        void DefineActionForElements()
        {
            // // sur la liste des elements selectionnés (Selected)
            // foreach (Selection.Selected selected in Selection.Singleton.selection)
            // {
            //     ElementReference.Element element = selected.SelectedElement;
            //     
            //     if (ElementReference.IsUnitElement(selected.SelectedElement))
            //     {
            //         if (element == ElementReference.Element.Soldier)
            //         {
            //             
            //         }
            //
            //         if (element == ElementReference.Element.Worker)
            //         {
            //             // les éléments Worker peuvent cibler les ressources
            //             if (HoveredTargetManager.Singleton.targetType == EntityReference.Entity.Resource)
            //             {
            //
            //                 // UnitManager.GetUnitForGameObject(selected.SelectedGameObject).unitTarget = HoveredTargetManager.Singleton.target;
            //                 // UnitManager.GetUnitForGameObject(selected.SelectedGameObject).CurrentAction = ActorReference.ElementAction.MoveToResource;
            //                 // UnitManager.GetUnitForGameObject(selected.SelectedGameObject).SetTargetPoint(HoveredTargetManager.Singleton.target.transform.position);
            //                 
            //                 // on définit l'action de l'ouvrier à "MoveToRessource"
            //                 // unitSelected.UnitBehaviour.unitTarget = 
            //                 // unitSelected.UnitBehaviour.SetState();
            //                 // unitSelected.UnitBehaviour.SetTargetPoint();
            //
            //                 ResourcesManager.Singleton.AccaparateResource(HoveredTargetManager.Singleton.target);
            //
            //                 SelectionService.ElementActionMessage(ActorReference.ElementAction.MoveToResource);
            //
            //             }
            //         }
            //     }
            //     
            // }
        }
        
        // Preview pour la création d'un batiment
        void PrepareToCreateBuilding()
        {
            Vector3 mousePos = RaycastUtility.RaycastPosition();
            ElementPlacer.Singleton.PrevisualizeBuildingGhost(mousePos,
                ElementPlacer.Singleton.elementTypeOfNewBuilding);

            if (Input.GetMouseButtonDown(0))
            {
                ElementScriptable elementScriptableForBuilding = 
                ElementManager.Singleton.GetElementScriptableForElement(ElementPlacer.Singleton.elementTypeOfNewBuilding);

                BuildingScriptable buildingScriptable = elementScriptableForBuilding as BuildingScriptable;
                ResourcesManager.Singleton.SpendMineral(buildingScriptable.moneyCost);
                ResourcesManager.Singleton.SpendGaz(buildingScriptable.gazCost);

                // Je crée le batiment en question a cet endroit
                ElementManager.Singleton.InstantiateElement(ElementPlacer.Singleton.elementTypeOfNewBuilding, mousePos,0);
                ElementPlacer.Singleton.StopPrevizualition();
                // je Reset l'action pour arrêter la prévisualisation du batiment à construire
                ResetCurrentAction();
            }
        }

        
        void ResetCurrentAction()
        {
            currentAction = ActorReference.ElementAction.None;
        }
        
        
    }
