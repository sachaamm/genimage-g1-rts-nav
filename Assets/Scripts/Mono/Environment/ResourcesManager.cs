using System;
using System.Collections.Generic;
using Mono.Entity;
using RotaryHeart.Lib.SerializableDictionary;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.UI;


    // Il gère les ressources, l'instantiation des ressources, et la liste resourcesList 
    // qui contient toutes les données relatives aux ressources
    public class ResourcesManager : MonoBehaviour
    {
        [SerializeField] private ResourceDictionary _resourceDictionary;

        [System.Serializable]
        public class ResourceDictionary : SerializableDictionaryBase<ResourcesReference.Resource, ResourceScriptable>
        {
        }

        public static ResourcesManager Singleton;

        public List<GameObject> resourcesList = new List<GameObject>();

        // private donc modifiables uniquement dans ce script
        int mineralAmount = 100;
        int gazAmount = 0;

        // acces public vers la valeur de mineralAmount et gazAmount
        public int MineralAmount => mineralAmount;
        public int GazAmount => gazAmount; 

        public Text mineralAmountText, gazAmountText;

        private void Awake()
        {
            Singleton = this;
        }

        void Start()
        {
            CreateResources();
        }

        public void AddMineral(int amount)
        {
            mineralAmount += amount;
        }

        public void AddGaz(int amount)
        {
            gazAmount += amount;
        }

        public void SpendMineral(int amount)
        {
            mineralAmount -= amount;
        }

        public void SpendGaz(int amount)
        {
            gazAmount -= amount;
        }

        private void Update()
        {
            mineralAmountText.text = mineralAmount.ToString();
            gazAmountText.text = gazAmount.ToString();
        }

        void CreateResources()
        {
            int nbResources = 10;

            for (int i = 0; i < nbResources; i++)
            {
                InstantiateResource(ResourcesReference.Resource.MineralField, EnemyManager.Singleton.RandomSpawnPos());
            }
            
        }
        public GameObject GetClosestResourceOfType(Vector3 pos)
        {
            GameObject closestResource = null;
            float minDistance = Mathf.Infinity;
            
            foreach (GameObject r in Singleton.resourcesList)
            {
                float distance = Vector3.Distance(r.transform.position, pos);
                if (distance < minDistance)
                {
                    closestResource = r;
                    minDistance = distance;                
                }
            }
            
            return closestResource;
        }

        void InstantiateResource(ResourcesReference.Resource resource, Vector3 pos)
        {
            var newResource = Instantiate(_resourceDictionary[resource].Prefab, pos, Quaternion.identity);
            newResource.AddComponent<EntityObject>().entityType = EntityReference.Entity.Resource;
            resourcesList.Add(newResource);
        }
        
        public ResourceScriptable GetResourceScriptable(ResourcesReference.Resource resource)
        {
            return Singleton._resourceDictionary[resource];
        }
    }
