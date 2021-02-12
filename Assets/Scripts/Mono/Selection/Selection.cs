using DefaultNamespace;
using System.Collections.Generic;
using System.Linq;
using ECS.System;
using Mono.Actor;
using Mono.Element;
using Mono.Service;
using Unity.Entities;
using UnityEngine;

// Sélectionner une unité ( avec un raycast ) quand on clique gauche
// et on affichera les informations de l'élément sur le GUI 


public class Selection : MonoBehaviour
{
    public bool mouseOnGUI = false;

    public static Selection Singleton;

    public List<Selected> selection = new List<Selected>();

    public GameObject houseGhostPrefab;
    private GameObject houseGhost;

    // Classe mère d' élement selectionné
    public class Selected
    {
        public GameObject SelectedGameObject;
        public Entity SelectedEntity;
        public ElementReference.Element SelectedElement;
    }

    // Element selectionné de type unité
    public class UnitSelected : Selected // Plus utile
    {
        public UnitBehaviour UnitBehaviour; // Plus utile
    }

    // Element selectionné de type building
    public class BuildingSelected : Selected
    {
        // public 
    }
    
    private void Awake()
    {
        Singleton = this;
    }
    
    public void ReceiveSelectionOnMouseUp(List<Selected> s)
    {
        if (mouseOnGUI) return;
        UpdateSelection(s);
    }

    public void UpdateSelection(List<Selected> s)
    {
        foreach (var go in selection)
        {
            Unselect(go);
        }

        selection = s;

        foreach (var go in selection)
        {
            Select(go);
        }
    }

    // Je réapplique le matériau de base à l'unité déselectionnée
    void Unselect(Selected s)
    {
        // s.SelectedGameObject.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.DefaultMaterial;
    }
    
    // J'applique le matériau de selection à l'unité selectionnée
    void Select(Selected s)
    {
        // s.SelectedGameObject.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.SelectedMaterial;
    }

    // Je déplace la sélection quand je clique droit sur l'écran 
    public void MoveSelection(Vector3 destination)
    {
        List<int> uuidSelection = selection.Select(s => s.SelectedGameObject).Select(s => int.Parse(s.name)).ToList();
        
        SelectionService.SelectionMoveToPointMessage(
            new UnitsSystemBase.MoveSelectionGroup
            {
                selection = uuidSelection,
                destination = destination
            }
            );
        
        foreach (var selected in selection)
        {
            if (selected.GetType() == typeof(UnitSelected))
            {
                
                var unitSelected = selected as UnitSelected;

                
                
                // TODO BROKEN
                // UnitManager.GetUnitForGameObject(unitSelected.SelectedGameObject).Release();
                // UnitManager.GetUnitForGameObject(unitSelected.SelectedGameObject).CurrentAction = ActorReference.ElementAction.MoveToPoint;
                // UnitManager.GetUnitForGameObject(unitSelected.SelectedGameObject).SetTargetPoint(destination);
                
                
                
                // unitSelected.UnitBehaviour.Release();
                // unitSelected.UnitBehaviour.SetState(ActorReference.ElementAction.MoveToPoint); 
                // unitSelected.UnitBehaviour.SetTargetPoint(destination);

            }
            
            // TODO
            // NavMeshAgent navMeshAgent = selected.SelectedGameObject.GetComponent<NavMeshAgent>();
            // if (navMeshAgent != null)
            // {
            //     navMeshAgent.destination = destination;
            // }
        }
    }
    
}
