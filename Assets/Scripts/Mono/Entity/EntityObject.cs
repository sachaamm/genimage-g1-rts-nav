using System;
using Mono.UI;
using UnityEngine;

namespace Mono.Entity
{
    // Script associé à chaque Entité ( Element, Enemy, Resource ) 
    public class EntityObject : MonoBehaviour
    {
        public EntityReference.Entity entityType;

        // si je place la souris sur l'entité
        private void OnMouseEnter()
        {
            // tu m'as mis la souris dessus, c'est donc moi ta cible
            HoveredTargetManager.Singleton.ReceiveTarget(gameObject, entityType);
        }
        
        // si je sours la souris de l'entité
        private void OnMouseExit()
        {
            // tu as enlevé la souris de moi, je ne suis plus ta cible
            HoveredTargetManager.Singleton.ReleaseTarget();
        }
    }
}