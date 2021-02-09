using System;
using DefaultNamespace.Element;
using Scriptable.Scripts;
using UnityEngine;

namespace Mono.Actor
{
    // Un script appliqué sur chaque Enemy et qui va gérer le comportement de l'Enemy
    public class Enemy : MonoBehaviour
    {
        private float attaqueCompteur = 0;

        public EnemyReference.EnemyType EnemyType;
        
        private void Update()
        {
            // je récupère l'élement le plus proche
            ElementManager.ElementGameObject closestElement = GetClosestNonEnemyElement();

            // si il n'y a pas d'élement, on s'arrête ici (return)
            if (closestElement == null) return;
            
            // l'ennemi regarde dans la direction de l'élement le plus proche de lui
            transform.LookAt(closestElement.elementGameObject.transform);
            
            float distanceToClosestElement = 
                Vector3.Distance(closestElement.elementGameObject.transform.position, transform.position);

            // à partir de la variable EnemyType, l'ennemy récupère ses stats ( montant dégats, vitesse déplacement, etc )
            EnemyScriptable enemyScriptable = EnemyManager.GetEnemyScriptable(EnemyType);
            
            // si il est à portée d'attaque
            if (distanceToClosestElement < enemyScriptable.RangeToAttack)
            {
                // J'attaque l'element le plus proches
                Attack(closestElement, enemyScriptable);
            }
            else
            {
                attaqueCompteur = 0;
                // Je me déplace dans la direction de l'element le plus proche
                MoveToClosestTarget(closestElement, enemyScriptable);
            }
            
            
            // Je récupère la liste des elements

            
        }

        // je me déplace vers la cible la plus proche
        void MoveToClosestTarget(ElementManager.ElementGameObject closestElement, EnemyScriptable enemyScriptable)
        {
            Vector3 diff = closestElement.elementGameObject.transform.position - transform.position;
            transform.position += diff.normalized * enemyScriptable.MoveSpeed;
        }

        void Attack(ElementManager.ElementGameObject closest, EnemyScriptable enemyScriptable)
        {
            
            attaqueCompteur += Time.deltaTime;

            if (attaqueCompteur >= enemyScriptable.DamageInterval)
            {
                attaqueCompteur = 0;
                Debug.Log("Attack");
                
                closest.AddHealthAmount(-enemyScriptable.DamageAmount);
                
            }
            
        }

        // On récupère l'element le plus proche
        ElementManager.ElementGameObject GetClosestNonEnemyElement()
        {
            ElementManager.ElementGameObject closestNonEnemy = null;
            float minDistance = Mathf.Infinity;
            
            foreach (ElementManager.ElementGameObject nonEnemy in ElementManager.Singleton.elementsNonEnemy)
            {
                float distance = Vector3.Distance(nonEnemy.elementGameObject.transform.position, transform.position);
                if (distance < minDistance)
                {
                    closestNonEnemy = nonEnemy;
                    minDistance = distance;
                }
            }
            
            return closestNonEnemy;
        }
        
    }
}