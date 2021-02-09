using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Script appliqué à chaque bouton de "GroupElement".
///  GroupElement correspond à l'affichage de l'icone de l'élement
/// </summary>
public class GroupElementButton : MonoBehaviour
{

    public Selection.Selected Selected;

    public void Click()
    {
        List<Selection.Selected> selection = new List<Selection.Selected> {Selected};
        
        // J'envoie une liste qui contiendra un seul element qui correspond a l'element selectionné
        // je vais donc focus la selection sur l'element selectionné
        // ( selectionner un element dans la selection )
        Selection.Singleton.UpdateSelection(selection);
        UiManager.Singleton.UpdateGroupLayout(new List<Selection.Selected>{Selected});

        List<ActorReference.ElementWithAction> elementWithActions = RectSelector.ElementWithActionsFromSelection(selection);
        UiManager.Singleton.UpdateActionsLayout(elementWithActions);
    }
}
