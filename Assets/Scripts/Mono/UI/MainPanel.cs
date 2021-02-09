using UnityEngine;

/// <summary>
/// Script permettant de vérifier qu'on a pas le souris dans le PanelMenu
/// </summary>
public class MainPanel : MonoBehaviour
{
    public void onEnter()
    {
        Selection.Singleton.mouseOnGUI = true;
    }

    public void onExit()
    {
        Selection.Singleton.mouseOnGUI = false;
    }
}
