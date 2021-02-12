using System;
using DefaultNamespace.Element;
using Scriptable.Scripts;
using UnityEngine;
using System.Collections.Generic;
using Mono.Element;
using RotaryHeart.Lib.SerializableDictionary;

// Il gère la prévisualisation du placement des élements ( ex: placer un batiment avant de le construire )
public class ElementPlacer : MonoBehaviour
    {
        public GameObject ghostPrefab;
        private GameObject ghost;
        public static ElementPlacer Singleton;

        private MeshFilter ghostMeshFilter;
   

    // Le dictionnaire des stats des Element
    [SerializeField] public BuildingDictionary buildingDictionary;

    [System.Serializable]
    public class BuildingDictionary : SerializableDictionaryBase<ElementReference.Element, BuildingScriptable>
    {
    }


    // Le type d'élement du building que je suis en train de placer
    public ElementReference.Element elementTypeOfNewBuilding;

        
        
        private void Awake()
        {
            Singleton = this;
            ghost = GameObject.Instantiate(ghostPrefab);
            ghostMeshFilter = ghost.GetComponent<MeshFilter>();
            ghost.SetActive(false);
            
        }

        public void PrevisualizeBuildingGhost(Vector3 pos, ElementReference.Element element)
        {
            ElementScriptable elementScriptable = ElementManager.Singleton.GetElementScriptableForElement(element);
            BuildingScriptable buildingScriptable = elementScriptable as BuildingScriptable;
            ghost.transform.localScale = elementScriptable.Prefab.transform.localScale;
            ghost.SetActive(true);
            // ghost.transform.localScale = elementScriptable.Prefab.transform.localScale;
            ghost.transform.position = pos;
            
            ghostMeshFilter.mesh = buildingScriptable.ghostBuildingMesh;
        }

        public void StopPrevizualition()
        {
            ghost.SetActive(false);
        }
    }
