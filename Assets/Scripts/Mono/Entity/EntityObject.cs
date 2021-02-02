using System;
using Mono.UI;
using UnityEngine;

namespace Mono.Entity
{
    public class EntityObject : MonoBehaviour
    {
        public EntityReference.Entity entityType;


        private void OnMouseEnter()
        {
            HoveredTargetManager.Singleton.ReceiveTarget(gameObject, entityType);
        }

        private void OnMouseExit()
        {
            HoveredTargetManager.Singleton.ReleaseTarget();
        }
    }
}