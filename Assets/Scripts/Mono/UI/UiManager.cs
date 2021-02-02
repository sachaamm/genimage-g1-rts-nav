using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Element;
using Scriptable.Scripts;
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

    public void UpdateGroupLayout(List<Selection.Selected> selection)
    {
        ResetGroupLayout();

        foreach (var selected in selection)
        {
            var groupElement = Instantiate(groupElementPrefab, groupGridLayoutParent);
            ElementScriptable elementScriptable =
                ElementManager.Singleton.GetElementScriptableForElement(selected.SelectedElement);
            
            Transform button = groupElement.transform.Find("Button");
            GroupElementButton groupElementButton = button.GetComponent<GroupElementButton>();
            groupElementButton.Selected = selected;
            
            button.Find("Image").GetComponentInChildren<Image>().sprite = elementScriptable.Icon;
        }
        
    }

    void ResetGroupLayout()
    {
        foreach (Transform child in groupGridLayoutParent)
        {
            Destroy(child.gameObject);
        }
    }



    public void UpdateActionsLayout(List<ActorReference.ElementWithAction> elementActions)
    {
        ResetActionsLayout();

        foreach (ActorReference.ElementWithAction elementWithAction in elementActions)
        {
            GameObject go = Instantiate(actionElementPrefab, actionsGridLayoutParent);
            go.GetComponentInChildren<Text>().text = elementWithAction.ElementAction.ToString();
            go.GetComponent<ActionElementButton>().elementWithAction = elementWithAction;
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