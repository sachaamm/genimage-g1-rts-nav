using System;
using System.Collections.Generic;
using DefaultNamespace;
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

        public List<ResourceGameObject> resourcesList = new List<ResourceGameObject>();

        public class ResourceGameObject
        {
            public GameObject gameObject;
            public int workers;
        }

        // private donc modifiables uniquement dans ce script
        int mineralAmount = 6000;
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
            int nbResources = 100;

            for (int i = 0; i < nbResources; i++)
            {
                var r = InstantiateResource(ResourcesReference.Resource.MineralField, EnemyManager.Singleton.RandomSpawnPos());
                r.name = "Minerai " + i;
            }
            
        }
        
        
        public GameObject GetClosestAvaibleResourceOfType(Vector3 pos)
        {
            GameObject closestResource = null;
            float minDistance = Mathf.Infinity;
            
            foreach (var r in Singleton.resourcesList)
            {
                float distance = Vector3.Distance(r.gameObject.transform.position, pos);
                if (distance < minDistance && r.workers == 0)
                {
                    closestResource = r.gameObject;
                    minDistance = distance;                
                }
            }
            
            return closestResource;
        }
        
        public GameObject GetClosestResourceOfType(Vector3 pos)
        {
            GameObject closestResource = null;
            float minDistance = Mathf.Infinity;
            
            foreach (var r in Singleton.resourcesList)
            {
                float distance = Vector3.Distance(r.gameObject.transform.position, pos);
                if (distance < minDistance)
                {
                    closestResource = r.gameObject;
                    minDistance = distance;                
                }
            }
            
            return closestResource;
        }

        public void AccaparateResource(GameObject resource)
        {
            foreach (var r in Singleton.resourcesList)
            {
                if (resource == r.gameObject)
                {
                    r.workers++; // si il s'agit de la ressource ciblée, je la bloque pour un ouvrier
                    UpdateMineraiMaterialAvailability(r);
                }
            }
        }

        public void ReleaseResource(GameObject resource)
        {
            foreach (var r in Singleton.resourcesList)
            {
                if (resource == r.gameObject)
                {
                    r.workers--; // si il s'agit de la ressource ciblée, je la debloque pour les autres ouvriers
                    UpdateMineraiMaterialAvailability(r);
                }
            }
        }

        void UpdateMineraiMaterialAvailability(ResourceGameObject r)
        {
            if (r.workers == 0)
            {
                r.gameObject.GetComponent<MeshRenderer>().material =
                    MaterialManager.Singleton.MineraiDefaultMaterial;
            }

            if (r.workers < 0)
            {
                r.gameObject.GetComponent<MeshRenderer>().material =
                    MaterialManager.Singleton.MineraiNegativeWorkersMaterial;
            }
                    
            if (r.workers > 0)
            {
                r.gameObject.GetComponent<MeshRenderer>().material =
                    MaterialManager.Singleton.MineraiUnavailableMaterial;
            }
        }

        GameObject InstantiateResource(ResourcesReference.Resource resource, Vector3 pos)
        {
            var newResource = Instantiate(_resourceDictionary[resource].Prefab, pos, Quaternion.identity);
            newResource.AddComponent<EntityObject>().entityType = EntityReference.Entity.Resource;

            ResourceGameObject resourceGameObject = new ResourceGameObject();
            resourceGameObject.gameObject = newResource;
            resourceGameObject.workers = 0;
            
            resourcesList.Add(resourceGameObject);

            return newResource;
        }
        
        public ResourceScriptable GetResourceScriptable(ResourcesReference.Resource resource)
        {
            return Singleton._resourceDictionary[resource];
        }
    }
