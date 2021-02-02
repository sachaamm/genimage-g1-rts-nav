
using DefaultNamespace.Element;
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
                ElementScriptable elementScriptable =
                    ElementManager.Singleton.GetElementScriptableForElement(ElementReference.Element.Worker);

                foreach (GameObject element in elementWithAction.ElementsForAction)
                {
                    Vector3 spawnPos = element.transform.position;
                
                    ElementManager.Singleton.InstantiateElement(ElementReference.Element.Worker, spawnPos);
                }
                
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
                        // Select(hit.collider.gameObject);
                        Selection.Singleton.MoveSelection(hit.point);

                    }
                }
                else
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
