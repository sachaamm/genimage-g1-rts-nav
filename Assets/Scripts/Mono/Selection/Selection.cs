using DefaultNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Selection : MonoBehaviour
{
    // Sélectionner une unité ( avec un raycast ) quand on clique gauche

    // et on affichera les informations de l'élément sur le GUI 

    

    public bool mouseOnGUI = false;

    public static Selection Singleton;

    public List<Selected> selection = new List<Selected>();

    public GameObject houseGhostPrefab;
    private GameObject houseGhost;

    public class Selected
    {
        public GameObject SelectedGameObject;
        public ElementReference.Element SelectedElement;
    }

    public class UnitSelected : Selected
    {
        public Unit Unit;
    }

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
        if (mouseOnGUI)
        {
            return;
        }

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

    void Unselect(Selected s)
    {
        s.SelectedGameObject.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.DefaultMaterial;
    }
    
    void Select(Selected s)
    {
        s.SelectedGameObject.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.SelectedMaterial;
    }

    public void MoveSelection(Vector3 destination)
    {
        foreach (var selected in selection)
        {
            if (selected.GetType() == typeof(UnitSelected))
            {
                var unitSelected = selected as UnitSelected;
                unitSelected.Unit.TargetPoint = destination;
                unitSelected.Unit.SetState(ActorReference.ElementAction.MoveToPoint);
                
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
