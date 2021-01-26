using DefaultNamespace;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private Material initMaterial;
    private MeshRenderer _meshRenderer;
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        initMaterial = _meshRenderer.material;
    }
 
 

    // private void OnMouseEnter()
    // {
    //     _meshRenderer.material = SelectorManager.Singleton.SelectedMaterial;
    // }
    //
    // private void OnMouseExit()
    // {
    //     _meshRenderer.material = initMaterial;
    // }
    //
    

    private void OnMouseDown()
    {
        Debug.Log("Vous avez cliqué sur le game object " + gameObject.name);
    }
}
