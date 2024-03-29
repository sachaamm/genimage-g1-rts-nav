﻿using System.Collections;
using System.Collections.Generic;
using Mono.Actor;
using Mono.Service;
using UnityEngine;

/// <summary>
///  Script appliqué à chaque bouton de "GroupElement".
///  GroupElement correspond à l'affichage de l'icone de l'élement
/// </summary>
public class GroupElementButton : MonoBehaviour
{
    public bool multiMode = false;
    public Selection.Selected Selected;

    public void Click()
    {
        if (multiMode == false)
        {
            List<Selection.Selected> selection = new List<Selection.Selected> {Selected};
            
            Selection.Singleton.ReceiveSelection(selection);
        
            // J'envoie une liste qui contiendra un seul element qui correspond a l'element selectionné
            // je vais donc focus la selection sur l'element selectionné
            // ( selectionner un element dans la selection )
        
            // UiManager.Singleton.UpdateGroupLayout(new List<Selection.Selected>{Selected});

            // SelectionService.SelectionChangedMessage(new List<int>{Selected.SelectedUuid});

            List<ActorReference.ElementWithAction> elementWithActions = RectSelector.ElementWithActionsFromSelection(selection);
            
        }
        else
        {
            
        }
        
        
    }
}