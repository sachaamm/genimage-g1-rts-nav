using DefaultNamespace;
using System.Collections.Generic;
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
        public ElementReference.Element SelectedElement;
    }

    // Element selectionné de type unité
    public class UnitSelected : Selected
    {
        public Unit Unit;
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
        s.SelectedGameObject.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.DefaultMaterial;
    }
    
    // J'applique le matériau de selection à l'unité selectionnée
    void Select(Selected s)
    {
        s.SelectedGameObject.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.SelectedMaterial;
    }

    // Je déplace la sélection quand je clique droit sur l'écran 
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
