using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mono.Element
{
    public class ElementReferential : MonoBehaviour
    {
        public static ElementReferential Singleton;

        public Dictionary<int,ElementReference.Element> ElementReferentialMap = new Dictionary<int, ElementReference.Element>();
        
        private void Awake()
        {
            Singleton = this;
        }

        public void AddReferentialEntry(int uuid, ElementReference.Element element)
        {
            ElementReferentialMap.Add(uuid, element);
        }

        public static ElementReference.Element GetElementTypeForGameObject(GameObject go)
        {
            return Singleton.ElementReferentialMap[int.Parse(go.transform.name)];
        }
        
        
    }
}