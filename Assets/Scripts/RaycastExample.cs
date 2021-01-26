using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class RaycastExample : MonoBehaviour
{
    private GameObject selected;
    private Material selectedInitMaterial;

    public NavMeshAgent NavMeshAgent;
    
    void Start()
    {
        
    }
    
    void Update()
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
                    SetPlayerDestination(hit.point);

                }
            }
            else
            {
                // if(selected) Unselect(selected);
            }
        }
        
            
            
            
        
    }

    void SetPlayerDestination(Vector3 point)
    {
        NavMeshAgent.destination = point;
    }

    void Select(GameObject s)
    {
        if (selected != null)  // si j'ai déja selectionné un gameobject auparavant
        {
            Unselect(selected); // je le déselectionne
        }
        
        // j'assigne selected avec le game object
        selected = s;
        // on enregistre le matériau initial du gameobject selectionné pour l'utiliser plus tard
        selectedInitMaterial = s.GetComponent<MeshRenderer>().material;
        // je change le materiau de son mesh renderer
        s.GetComponent<MeshRenderer>().material = SelectorManager.Singleton.SelectedMaterial;

    }

    void Unselect(GameObject s)
    {
        s.GetComponent<MeshRenderer>().material = selectedInitMaterial;
    }
    
    
}
