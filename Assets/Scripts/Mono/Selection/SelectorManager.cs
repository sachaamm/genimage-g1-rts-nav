using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SelectorManager : MonoBehaviour
    {
        public static SelectorManager Singleton; // Un Singleton est un objet qui est instantié une seule fois !

        public Material SelectedMaterial;
        public Material DefaultMaterial;
        
        private void Awake()
        {
            Singleton = this; // this est le script en question : SelectorManager
        }
        
    }
}