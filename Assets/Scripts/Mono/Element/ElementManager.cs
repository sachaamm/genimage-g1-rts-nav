﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mono.Entity;
using Mono.Util;
using RotaryHeart.Lib.SerializableDictionary;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Element
{
    public class ElementManager : MonoBehaviour
    {
        public static ElementManager Singleton;

        public Transform ElementsParent;

        [SerializeField] private ElementDictionary _elementDictionary;

        [System.Serializable]
        public class ElementDictionary : SerializableDictionaryBase<ElementReference.Element, ElementScriptable>
        {
        }

        public GameObject prefabIndicator;

        public class ElementGameObject
        {
            public GameObject elementGameObject;
            private int elementHealth;
            private Image healthImg;
            private ElementReference.Element typeElement;

            public ElementGameObject(GameObject _elementGameObject, ElementReference.Element element, Image _healthImg)
            {
                elementGameObject = _elementGameObject;
                // Je récupère les stats de mon élément
                ElementScriptable elementScriptable = ElementManager.Singleton.GetElementScriptableForElement(element);
                elementHealth = elementScriptable.MaxHealth;
                healthImg = _healthImg;
                typeElement = element;
            }

            public int ElementHealth
            {
                get => elementHealth;
            }

            public void AddHealthAmount(int amount) // 
            {
                elementHealth += amount;

                ElementScriptable elementScriptable = ElementManager.Singleton.GetElementScriptableForElement(typeElement);
                float ratioLife = elementHealth / (float) elementScriptable.MaxHealth;
                healthImg.fillAmount = ratioLife;
                
                if (elementHealth <= 0)
                {
                    ElementManager.Singleton.DestroyElement(this);
                }
                
            }
            
        }

        public List<ElementGameObject> elementsNonEnemy = new List<ElementGameObject>();

        public GameObject debugActorCanvasPrefab;
        
        private void Awake()
        {
            Singleton = this;
        }

        public void InstantiateElement(ElementReference.Element element, Vector3 position)
        {
            ElementScriptable elementScriptable = _elementDictionary[element];
            
            GameObject newElement =
                Instantiate(elementScriptable.Prefab, position, Quaternion.identity, ElementsParent);
            
            GameObject indicatorNewElement = 
                Instantiate(prefabIndicator, newElement.transform.position + new Vector3(0,1,0), 
                    Quaternion.Euler(new Vector3(0,0,0)), 
                    newElement.transform);

            newElement.AddComponent<EntityObject>().entityType = EntityReference.Entity.Element;
            

            if (DebugUtility.DebugActors)
            {
                InstantiateLocalCanvasInActor(newElement);
            }
            
            
            if (elementScriptable.GetType() == typeof(UnitScriptable))
            {
                newElement.AddComponent<Unit>();
            }

            if (elementScriptable.GetType() == typeof(BuildingScriptable))
            {
                
            }

            ElementIdentity elementIdentity = newElement.AddComponent<ElementIdentity>();
            elementIdentity.Element = element;
            
            ElementGameObject elementGameObject = new ElementGameObject(newElement, element, GetHealthImg(newElement));

            elementsNonEnemy.Add(elementGameObject);
        }

        public static Image GetHealthImg(GameObject element)
        {
            // je récupére le local canvas et l'image associé la vie : "HealthBarFill"
            // return 
            return element.transform.Find("DebugActorCanvas").Find("HealthBarBackground").GetComponent<Image>();

        }

        public void InstantiateLocalCanvasInActor(GameObject actor)
        {
            // Debug canvas for actor
            GameObject debugCanvas = Instantiate(
                debugActorCanvasPrefab, 
                actor.transform.position + new Vector3(0,1,0), 
                Quaternion.Euler(new Vector3(90,0,0)), 
                actor.transform);
            debugCanvas.name = "DebugActorCanvas";
        }
        
        public void DestroyElement(ElementGameObject elementGameObject)
        {
            for (int i = elementsNonEnemy.Count - 1; i >= 0; i--)
            {
                if (elementsNonEnemy[i] == elementGameObject)
                {
                    Destroy(elementGameObject.elementGameObject);
                    elementsNonEnemy.Remove(elementGameObject);
                }
            }
        }

        public ElementScriptable GetElementScriptableForElement(ElementReference.Element element)
        {
            ElementScriptable elementScriptable = _elementDictionary[element];
            return elementScriptable;
        }

        public GameObject GetClosestElementOfType(ElementReference.Element element)
        {
            ElementManager.ElementGameObject closestNonEnemy = null;
            float minDistance = Mathf.Infinity;
            
            foreach (ElementManager.ElementGameObject nonEnemy in ElementManager.Singleton.elementsNonEnemy)
            {
                float distance = Vector3.Distance(nonEnemy.elementGameObject.transform.position, transform.position);
                if (distance < minDistance)
                {
                    if (nonEnemy.elementGameObject.GetComponent<ElementIdentity>().Element == element)
                    {
                        closestNonEnemy = nonEnemy;
                        minDistance = distance;
                    }
                    
                }
            }
            
            return closestNonEnemy.elementGameObject;
        }
    }
}