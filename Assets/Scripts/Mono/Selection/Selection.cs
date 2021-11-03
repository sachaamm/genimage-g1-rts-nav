using DefaultNamespace;
using System.Collections.Generic;
using System.Linq;
using Mono.Actor;
using Mono.Element;
using Mono.Service;
using Unity.Entities;
using UnityEngine;

// Sélectionner une unité ( avec un raycast ) quand on clique gauche
// et on affichera les informations de l'élément sur le GUI 


public class Selection : MonoBehaviour
{
    public bool mouseOnGUI = false;

    public static Selection Singleton;

    private List<Selected> _selection;
    

    // Classe mère d' élement selectionné
    public class Selected
    {
        public int SelectedUuid;
        // public GameObject SelectedGameObject;
        public ElementReference.Element SelectedElement;
    }
    
    private void Awake()
    {
        Singleton = this;
    }

    public static List<int> UuidSelection()
    {
        return Singleton.GetSelection().Select(e => e.SelectedUuid).ToList();
    }

    public void ReceiveSelection(List<Selected> newSelection)
    {
        _selection = newSelection;
        
        UiManager.Singleton.UpdateGroupLayout(newSelection);
        UiManager.Singleton.UpdateActionsLayout(RectSelector.ElementWithActionsFromSelection(newSelection),newSelection);
                
        SelectionService.SelectionChangedMessage(newSelection.Select(u => u.SelectedUuid).ToList());
    }

    public List<Selected> GetSelection()
    {
        return _selection;
    }

    public void SelectPartOfMultiGroup(ElementReference.Element element)
    {
        int nbItems = 12;
        int i = 0;
        
        List<Selected> newSelection = new List<Selected>();
        
        foreach (var selected in _selection)
        {
            if (selected.SelectedElement == element && i < nbItems)
            {
                newSelection.Add(selected);
                i++;
            }
        }
        
        
        
    }
    
}