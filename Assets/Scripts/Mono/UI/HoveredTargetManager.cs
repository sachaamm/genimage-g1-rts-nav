using System;
using Mono.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Mono.UI
{
    
    /// <summary>
    /// Un script qui gère l'élement "hovered" ( l'élement sur lequel on passe la souris )
    /// HoveredTargetManager gère "target", qui est une cible potentielle.
    /// Target possède un targetType (Entity) qui sera égale à Element, Enemy ou Resource 
    /// </summary>
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

        /// <summary>
        /// Méthode permettant de recevoir la target, avec le type d'entité correspondant à cette target
        /// </summary>
        /// <param name="gameObject">La target</param>
        /// <param name="entityType">Le type d'entité de la target</param>
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