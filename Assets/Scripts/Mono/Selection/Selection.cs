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

    public List<GameObject> selection;

    public GameObject houseGhostPrefab;
    private GameObject houseGhost;
    
    private void Awake()
    {
        Singleton = this;
    }

    public void ReceiveSelection(List<GameObject> s)
    {
        if (mouseOnGUI)
        {
            return;
        }
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

    void Unselect(GameObject s)
    {
        s.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.DefaultMaterial;
    }
    
    void Select(GameObject s)
    {
        s.GetComponent<MeshRenderer>().material = MaterialManager.Singleton.SelectedMaterial;
    }

    public void MoveSelection(Vector3 destination)
    {
        foreach (var go in selection)
        {
            NavMeshAgent navMeshAgent = go.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.destination = destination;
            }
        }
    }
    
}
