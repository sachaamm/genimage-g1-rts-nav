using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
