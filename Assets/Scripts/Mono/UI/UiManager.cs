using System.Collections.Generic;
using System.Linq;
using Mono.Actor;
using Mono.Element;
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

    public GameObject groupElementPrefab, actionElementPrefab, createBuildingPrefab;

    enum ActionMenu
    {
        Main, // c'est le menu ou il y a toutes les actions possibles pour les élements sélectionnés
        CreateBuilding // c'est le menu qui me propose les différents batiments à construire
    }


    ActionMenu actionMenu = ActionMenu.Main;

    List<BuildingCreateButtonOption> buildingCreateButtons = new List<BuildingCreateButtonOption>();

    class BuildingCreateButtonOption
    {
        public Button buildingCreateButton;
        public BuildingScriptable buildingScriptable;
    }

    class UnitCreateButtonOption
    {
        public Button unitCreateButton;
        public UnitScriptable UnitScriptable;
    }
    
    void Awake()
    {
        Singleton = this;

        ResetGroupLayout();
        ResetActionsLayout();
    }
    
    
    

    public void EnterInCreateBuildingSubmenu()
    {

        actionMenu = ActionMenu.CreateBuilding;

        // Je vais supprimer les boutons d'actions
        ResetActionsLayout();
        buildingCreateButtons = new List<BuildingCreateButtonOption>();

        // Je vais créer un bouton pour chaque élement présent dans ElementPlacer.

        foreach (KeyValuePair<ElementReference.Element, BuildingScriptable> kvp in ElementPlacer.Singleton.buildingDictionary)
        {
            // je crée un bouton
            var createBuildingButton = Instantiate(createBuildingPrefab, actionsGridLayoutParent);
            CreateBuildingButton buildingButton = createBuildingButton.GetComponent<CreateBuildingButton>();
            buildingButton.buildingElement = kvp.Key;

            createBuildingButton.GetComponentInChildren<Text>().text = kvp.Key.ToString();

            BuildingCreateButtonOption buildingCreateButtonOption = new BuildingCreateButtonOption();
            buildingCreateButtonOption.buildingCreateButton = createBuildingButton.transform.Find("Button").GetComponent<Button>();
            buildingCreateButtonOption.buildingScriptable = kvp.Value;

            buildingCreateButtons.Add(buildingCreateButtonOption);

        }
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
    public void UpdateActionsLayout(List<ActorReference.ElementWithAction> elementActions, List<Selection.Selected> selection)
    {
        ResetActionsLayout();

        foreach (ActorReference.ElementWithAction elementWithAction in elementActions)
        {
            CreateActionButton(elementWithAction.ElementAction.ToString(), elementWithAction);
        }
        
        
        List<ActorReference.ElementWithActionCreateUnit> elementWithActionCreateUnitList = new List<ActorReference.ElementWithActionCreateUnit>();

        foreach (Selection.Selected selected in selection)
        {
            if (selected.GetType() == typeof(Selection.BuildingSelected))
            {
                BuildingScriptable buildingScriptable = ElementManager.Singleton.GetElementScriptableForElement(selected.SelectedElement) as BuildingScriptable;
                foreach (var unit in buildingScriptable.producableUnits)
                {
                    if (!elementWithActionCreateUnitList.Select(e => e.UnitToCreate).Contains(unit))
                    {
                        ActorReference.ElementWithActionCreateUnit elementWithActionCreateUnit = new ActorReference.ElementWithActionCreateUnit();
                        elementWithActionCreateUnit.UnitToCreate = unit;
                        elementWithActionCreateUnit.ElementAction = ActorReference.ElementAction.CreateUnit;
                        elementWithActionCreateUnit.ElementsForAction = new List<GameObject>{selected.SelectedGameObject};
                    
                        CreateActionButton("Create " + unit.ToString(), elementWithActionCreateUnit);
                    }
                    else
                    {
                        elementWithActionCreateUnitList
                            .First(e => e.UnitToCreate == unit).ElementsForAction.Add(selected.SelectedGameObject);
                    }
                    
                }
            }
        }
    }

    void CreateActionButton(string text, ActorReference.ElementWithAction elementWithAction)
    {
        GameObject go = Instantiate(actionElementPrefab, actionsGridLayoutParent);
        go.GetComponentInChildren<Text>().text = text;
        go.GetComponent<ActionElementButton>().elementWithAction = elementWithAction;
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

    private void Update()
    {
        if (actionMenu == ActionMenu.Main)
        {
            UpdateUnitCreationAvailability();
        }
        
        if(actionMenu == ActionMenu.CreateBuilding)
        {
            UpdateCreateBuildingAvailability();
        }
    }

    void UpdateUnitCreationAvailability()
    {
        
    }
    

    // si le batiment coute trop cher pour nos ressources, on le desactive, sinon il reste interactable
    void UpdateCreateBuildingAvailability()
    {
        
        foreach(BuildingCreateButtonOption option in buildingCreateButtons)
        {

            var mineral = ResourcesManager.Singleton.MineralAmount;
            var gaz = ResourcesManager.Singleton.GazAmount;
            // si le cout du batiment est supérieur en minerai disponible ou/et en gaz disponible
            if (option.buildingScriptable.moneyCost > mineral || option.buildingScriptable.gazCost > gaz)
            {
                option.buildingCreateButton.interactable = false;
            }
            else
            {
                option.buildingCreateButton.interactable = true;
            }
        }
    }



}