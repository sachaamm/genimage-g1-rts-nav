using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Selection
{
    public class Selection : MonoBehaviour
    {
        // Sélectionner une unité ( avec un raycast ) quand on clique gauche
        
        // et on affichera les informations de l'élément sur le GUI 

        

        public static Selection Singleton;

        public List<GameObject> selection;

        private void Awake()
        {
            Singleton = this;
        }

        public void ReceiveSelection(List<GameObject> s)
        {
            
            foreach (var go in selection)
            {
                Unselect(go);
            }
            
            selection = s;

            foreach (var go in selection)
            {
                Select(go);
            }
        }

        void Unselect(GameObject s)
        {
            s.GetComponent<MeshRenderer>().material = SelectorManager.Singleton.DefaultMaterial;
        }
        
        
        void Select(GameObject s)
        {
            s.GetComponent<MeshRenderer>().material = SelectorManager.Singleton.SelectedMaterial;
        }

        
    }
}