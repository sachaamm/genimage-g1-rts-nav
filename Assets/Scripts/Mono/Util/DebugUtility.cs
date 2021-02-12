using System;
using UnityEditor;
using UnityEngine;

namespace Mono.Util
{
    /// <summary>
    /// Un utilitaire de Debug qui permet de débugger les acteurs en affichant les actions en cours des acteurs ainsi que
    /// leur vie
    /// </summary>
    public class DebugUtility: MonoBehaviour
    {
        public static bool DebugActors = false;

        [SerializeField]
        private GameObject DebugPoint;

        public static DebugUtility Singleton;
        private void Awake()
        {
            Singleton = this;
        }

        // Instantier un point de debug.
        public static void InstantiateDebugPoint(Vector3 p, string name)
        {
            GameObject findDebugPoint = GameObject.Find(name);
            if (findDebugPoint)
            {
                // Il est pas null donc je me contente de modifier sa position
                findDebugPoint.transform.position = p;
            }
            else
            {
                // Il est null donc il existe pas donc je le crée
                var point = Instantiate(Singleton.DebugPoint, p, Quaternion.identity);
                point.name = name;
            }
            
            
        }
    }
}