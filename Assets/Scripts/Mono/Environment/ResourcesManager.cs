using System;
using System.Collections.Generic;
using DefaultNamespace.Element;
using Mono.Entity;
using RotaryHeart.Lib.SerializableDictionary;
using Scriptable.Scripts;
using UnityEngine;

namespace Mono.Environment
{
    public class ResourcesManager : MonoBehaviour
    {
        [SerializeField] private ResourceDictionary _resourceDictionary;

        [System.Serializable]
        public class ResourceDictionary : SerializableDictionaryBase<ResourcesReference.Resource, ResourceScriptable>
        {
        }

        public static ResourcesManager Singleton;

        public List<GameObject> resourcesList = new List<GameObject>();
        
        
        private void Awake()
        {
            Singleton = this;
        }

        void Start()
        {
            CreateResources();
        }

        void CreateResources()
        {
            int nbResources = 10;

            for (int i = 0; i < nbResources; i++)
            {
                InstantiateResource(ResourcesReference.Resource.MineralField, EnemyManager.Singleton.RandomSpawnPos());
            }
            
        }

        public GameObject GetClosestResourceOfType()
        {
            GameObject closestResource = null;
            float minDistance = Mathf.Infinity;
            
            foreach (var r in Singleton.resourcesList)
            {
                float distance = Vector3.Distance(r.transform.position, transform.position);
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
}