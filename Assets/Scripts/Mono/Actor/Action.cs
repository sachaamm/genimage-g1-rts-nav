
using DefaultNamespace.Element;
using Mono.Entity;
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

        // Quand je clique sur un bouton d'ElementAction, j'appelle la fonction InterpretAction qui me permet de définir l'action à effectuer
        public void InterpretAction(ActorReference.ElementWithAction elementWithAction)
        {
            bool preparingAction = false; // preparingAction = une action en 2 temps ( ex: CreateBuilding ) 

            // si 
            if(elementWithAction.ElementAction == ActorReference.ElementAction.CreateBuilding)
            {
                // j'appelle UI manager pour le mettre en contexte de menu "CreateBuilding"
                UiManager.Singleton.EnterInCreateBuildingSubmenu();
            }


            if (IsPreparingAction(elementWithAction.ElementAction))
            {
                // Soit on appelle une action de "préparation" : Préparer la construction d'un batiment avec la prévisualisation
                // currentAction = elementWithAction.ElementAction;
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

        // Permet à un batiment de créer des unités
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

        // Si l'ElementAction est une action de préparation ( ex: avant de Creer un batiment, il y a une phase ou on le positionne sur la map )
        bool IsPreparingAction(ActorReference.ElementAction action)
        {
            return action == ActorReference.ElementAction.CreateBuilding;
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
                            Selection.Singleton.MoveSelection(hit.point);
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
            // sur la liste des elements selectionnés (Selected)
            foreach (Selection.Selected selected in Selection.Singleton.selection)
            {
                // si l'élément selectionné est de type Unité (UnitSelected)
                if (selected.GetType() == typeof(Selection.UnitSelected))
                {
                    Selection.UnitSelected unitSelected = selected as Selection.UnitSelected;
                    ElementReference.Element unit = unitSelected.SelectedElement;

                    if (unit == ElementReference.Element.Soldier)
                    {
                        
                    }

                    if (unit == ElementReference.Element.Worker)
                    {
                        // les éléments Worker peuvent cibler les ressources
                        if (HoveredTargetManager.Singleton.targetType == EntityReference.Entity.Resource)
                        {
                            // on définit l'action de l'ouvrier à "MoveToRessource"
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
        
        // Preview pour la création d'un batiment
        void PrepareToCreateBuilding()
        {
            Vector3 mousePos = RaycastUtility.RaycastPosition();
            ElementPlacer.Singleton.PrevisualizeBuildingGhost(mousePos, ElementReference.Element.House);

            if (Input.GetMouseButtonDown(0))
            {
                ElementScriptable elementScriptableForBuilding = 
                ElementManager.Singleton.GetElementScriptableForElement(ElementPlacer.Singleton.elementTypeOfNewBuilding);

                BuildingScriptable buildingScriptable = elementScriptableForBuilding as BuildingScriptable;
                ResourcesManager.Singleton.SpendMineral(buildingScriptable.moneyCost);
                ResourcesManager.Singleton.SpendGaz(buildingScriptable.gazCost);

                // Je crée le batiment en question a cet endroit
                ElementManager.Singleton.InstantiateElement(ElementReference.Element.House, mousePos);
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
