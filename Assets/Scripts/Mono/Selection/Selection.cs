using DefaultNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static DefaultNamespace.Actor.ActorReference;


public class Selection : MonoBehaviour
{
    // Sélectionner une unité ( avec un raycast ) quand on clique gauche

    // et on affichera les informations de l'élément sur le GUI 

    public ElementAction currentAction = ElementAction.None;

    public bool mouseOnGUI = false;

    public static Selection Singleton;

    public List<GameObject> selection;

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

    private void MoveSelection(Vector3 destination)
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
    private void Update()
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
                    MoveSelection(hit.point);

                }
            }
            else
            {
                // if(selected) Unselect(selected);
            }
        }

    }
}
