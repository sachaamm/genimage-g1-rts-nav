using System.Collections.Generic;
using Mono.Ecs;
// using Mono.Ecs;
using Mono.Entity;
using Mono.Util;
using RotaryHeart.Lib.SerializableDictionary;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Mono.Element
{
    // Il va instantier les élements et il contient la liste des éléments, regroupant toutes les données relatives aux élements
    public class ElementManager : MonoBehaviour
    {
        public static ElementManager Singleton;

        public Transform ElementsParent;

        // Le dictionnaire des stats des Element
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

        // la liste de tous les élements qui peuvent sous notre controle (qui ne sont pas ennemis.)
        public List<ElementGameObject> elementsNonEnemy = new List<ElementGameObject>();

        public GameObject debugActorCanvasPrefab;

        private int populationAmount = 0;
        private int populationAmountMax = 8;

        public Text populationAmountText;

        private int uuid = 0;
        
        private void Awake()
        {
            Singleton = this;
        }

        void Update()
        {
            populationAmountText.text = populationAmount + "/" + populationAmountMax;
        }

        public GameObject InstantiateElement(ElementReference.Element element, Vector3 position)
        {
            ElementScriptable elementScriptable = _elementDictionary[element];
            
            GameObject newElement =
                Instantiate(elementScriptable.Prefab, position, Quaternion.identity, ElementsParent);
            
            
            newElement.transform.name = uuid.ToString();
            uuid++;
            
            GameObject indicatorNewElement = 
                Instantiate(prefabIndicator, newElement.transform.position + new Vector3(0,1,0), 
                    Quaternion.Euler(new Vector3(0,0,0)), 
                    newElement.transform);
            
            newElement.AddComponent<EntityObject>().entityType = EntityReference.Entity.Element;

            // ElementReferential.Singleton.AddReferentialEntry(int.Parse(newElement.transform.name), element);

            if (DebugUtility.DebugActors)
            {
                InstantiateLocalCanvasInActor(newElement);
            }
            
            if (elementScriptable.GetType() == typeof(UnitScriptable))
            {
                var agent = newElement.AddComponent<NavMeshAgent>();
                agent.angularSpeed = 500000;
                agent.speed *= 50;
                agent.acceleration *= 50;
                // agent.radius = 0.1f;
                // agent.autoBraking = false;
                
                UnitManager.Singleton.AddUnitInRuntimeSet(newElement, element);
                // newElement.AddComponent<UnitBehaviour>();
                newElement.transform.tag = "Unit";

                var unitScriptable = elementScriptable as UnitScriptable;
                populationAmount += unitScriptable.UnitPopulationCost;

                EntityInstantiator.InstantiateUnitEntity(element, newElement);
            }

            if (elementScriptable.GetType() == typeof(BuildingScriptable))
            {
                EntityInstantiator.InstantiateBuildingEntity(element, newElement);
            }

            ElementIdentity elementIdentity = newElement.AddComponent<ElementIdentity>();
            elementIdentity.Element = element;

            Image healthImg = null;

            if (DebugUtility.DebugActors)
            {
                healthImg = GetHealthImg(newElement);
            }
            
            ElementGameObject elementGameObject = new ElementGameObject(newElement, element, healthImg);

            elementsNonEnemy.Add(elementGameObject);

            return newElement;
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

        public GameObject GetClosestElementOfType(ElementReference.Element element, Vector3 pos)
        {
            ElementManager.ElementGameObject closestNonEnemy = null;
            float minDistance = Mathf.Infinity;
            
            foreach (ElementManager.ElementGameObject nonEnemy in ElementManager.Singleton.elementsNonEnemy)
            {
                float distance = Vector3.Distance(nonEnemy.elementGameObject.transform.position, pos);
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