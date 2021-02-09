using System;
using UnityEngine;

namespace DefaultNamespace
{
    // Il gère les materiaux , comme les matériaux de selection
    public class MaterialManager : MonoBehaviour
    {
        public static MaterialManager Singleton; // Un Singleton est un objet qui est instantié une seule fois !

        public Material SelectedMaterial;
        public Material DefaultMaterial;
        
        private void Awake()
        {
            Singleton = this; // this est le script en question : SelectorManager
        }
        
    }
}