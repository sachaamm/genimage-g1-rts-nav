using System;
using DefaultNamespace.Element;
using Scriptable.Scripts;
using UnityEngine;

namespace Mono.Actor
{
    public class Enemy : MonoBehaviour
    {
        private float attaqueCompteur = 0;

        public EnemyReference.EnemyType EnemyType;
        
        private void Update()
        {
            ElementManager.ElementGameObject closestElement = GetClosestNonEnemyElement();

            if (closestElement == null) return;
            
            transform.LookAt(closestElement.elementGameObject.transform);
            
            float distanceToClosestElement = 
                Vector3.Distance(closestElement.elementGameObject.transform.position, transform.position);

            EnemyScriptable enemyScriptable = EnemyManager.GetEnemyScriptable(EnemyType);
            
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