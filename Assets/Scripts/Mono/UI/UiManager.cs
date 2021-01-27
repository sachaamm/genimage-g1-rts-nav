using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Actor;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Singleton;

    public Transform groupGridLayoutParent, actionsGridLayoutParent;

    public GameObject groupElementPrefab, actionElementPrefab;
    
    void Awake()
    {
        Singleton = this;

        ResetGroupLayout();
        ResetActionsLayout();
    }

    public void UpdateGroupLayout(List<GameObject> selection)
    {
        ResetGroupLayout();

        foreach (GameObject selected in selection)
        {
            Instantiate(groupElementPrefab, groupGridLayoutParent);
        }
        
    }

    void ResetGroupLayout()
    {
        foreach (Transform child in groupGridLayoutParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateActionsLayout(List<ActorReference.ElementAction> elementActions)
    {
        ResetActionsLayout();

        foreach (var action in elementActions)
        {
            GameObject go = Instantiate(actionElementPrefab, actionsGridLayoutParent);
            go.GetComponentInChildren<Text>().text = action.ToString();
        }
    }

    void ResetActionsLayout()
    {
        foreach (Transform child in actionsGridLayoutParent)
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