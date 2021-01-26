using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Singleton;

    public Transform gridLayoutParent;

    public GameObject groupElementPrefab;
    
    void Awake()
    {
        Singleton = this;

        ResetGroupLayout();
    }

    public void UpdateGroupLayout(List<GameObject> selection)
    {
        ResetGroupLayout();

        foreach (GameObject selected in selection)
        {
            Instantiate(groupElementPrefab, gridLayoutParent);
        }
        
    }

    void ResetGroupLayout()
    {
        foreach (Transform child in gridLayoutParent)
        {
            Destroy(child.gameObject);
        }
    }

    void Start()
    {
    }


    void Update()
    {
    }
}