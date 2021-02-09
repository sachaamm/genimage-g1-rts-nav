using System.Collections.Generic;
using DefaultNamespace.Element;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Il gère l'UI. Il met à jour le GroupLayout et l'ActionsLayout.
/// Le GroupLayout correspond aux boutons d'icones des élements de notre selection.
/// L'ActionsLayout correspond aux boutons d'actions des élements de notre selection.
/// </summary>
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

    /// <summary>
    /// Elle met à jour le GroupLayout en fonction de la sélection
    /// </summary>
    /// <param name="selection"></param>
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

    /// <summary>
    /// Elle reset le GroupLayout en détruisant les enfants du GroupLayout
    /// </summary>
    void ResetGroupLayout()
    {
        foreach (Transform child in groupGridLayoutParent)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Elle met à jour l' ActionsLayout en fonction de la sélection
    /// </summary>
    /// <param name="elementActions"></param>
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

    /// <summary>
    /// Elle reset l'ActionsLayout en détruisant les enfants de l'ActionsLayout
    /// </summary>
    void ResetActionsLayout()
    {
        foreach (Transform child in actionsGridLayoutParent)
        {
            Destroy(child.gameObject);
        }
    }

}