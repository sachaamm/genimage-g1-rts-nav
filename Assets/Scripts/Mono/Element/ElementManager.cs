using System;
using RotaryHeart.Lib.SerializableDictionary;
using Scriptable.Scripts;
using UnityEngine;

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

        private void Awake()
        {
            Singleton = this;
        }

        public void InstantiateElement(ElementReference.Element element, Vector3 position)
        {
            ElementScriptable elementScriptable = _elementDictionary[element];
            GameObject newElement =
                Instantiate(elementScriptable.Prefab, position, Quaternion.identity, ElementsParent);

            ElementIdentity elementIdentity = newElement.AddComponent<ElementIdentity>();
            elementIdentity.Element = element;
        }

        public ElementScriptable GetElementScriptableForElement(ElementReference.Element element)
        {
            ElementScriptable elementScriptable = _elementDictionary[element];
            return elementScriptable;
        }
    }
}