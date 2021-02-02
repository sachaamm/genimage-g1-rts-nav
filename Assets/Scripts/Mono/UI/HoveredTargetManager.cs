using System;
using Mono.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Mono.UI
{
    public class HoveredTargetManager : MonoBehaviour
    {
        public static HoveredTargetManager Singleton;

        public GameObject target;
        public EntityReference.Entity targetType;

        public Text hoveredText;
        
        private void Awake()
        {
            Singleton = this;
        }

        public void ReceiveTarget(GameObject gameObject, EntityReference.Entity entityType)
        {
            target = gameObject;
            targetType = entityType;
            UpdateText(gameObject, entityType);
        }

        public void ReleaseTarget()
        {
            target = null;
            hoveredText.text = "NoTarget";
        }

        void UpdateText(GameObject gameObject, EntityReference.Entity entityType)
        {
            hoveredText.text = "Hovered : " + gameObject.name + " ( " + entityType.ToString() + " )";
        }

       
        
    }
}