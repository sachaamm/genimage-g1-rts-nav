using System;
using DefaultNamespace.Element;
using Scriptable.Scripts;
using UnityEngine;


    public class ElementPlacer : MonoBehaviour
    {
        public GameObject ghostPrefab;
        private GameObject ghost;
        public static ElementPlacer Singleton;
        
        private void Awake()
        {
            Singleton = this;
            ghost = GameObject.Instantiate(ghostPrefab);
            ghost.SetActive(false);
        }

        public void PrevisualizeBuildingGhost(Vector3 pos, ElementReference.Element element)
        {
            ElementScriptable elementScriptable = ElementManager.Singleton.GetElementScriptableForElement(element);
            ghost.transform.localScale = elementScriptable.Prefab.transform.localScale;
            ghost.SetActive(true);
            ghost.transform.position = pos;
        }

        public void StopPrevizualition()
        {
            ghost.SetActive(false);
        }
    }
