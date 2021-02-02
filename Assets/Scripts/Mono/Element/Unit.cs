
using System;
using System.Resources;
using DefaultNamespace.Element;
using Mono.Environment;
using Mono.Util;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.UI;


public class Unit : MonoBehaviour
    {
        // public ElementReference.Element Element;

        public ActorReference.ElementAction currentAction = ActorReference.ElementAction.None;

        // private int idleCompteur = 0;

        private float attaqueCompteur = 0;

        private ElementIdentity _elementIdentity;

        public Vector3 TargetPoint;
        
        
        
        private void Start()
        {
            _elementIdentity = GetComponent<ElementIdentity>();
            SetState(ActorReference.ElementAction.None);
        }

        private void Update()
        {
            UnitScriptable unitScriptable = ElementManager.Singleton.GetElementScriptableForElement(_elementIdentity.Element) as UnitScriptable;
            
            if (currentAction == ActorReference.ElementAction.None)
            {
                Idle(unitScriptable);
            }
            
            if(currentAction == ActorReference.ElementAction.MoveToEnemy)
            {
                var closest = ClosestEnemy();
                MoveToEnemy(unitScriptable,closest);

                if (Vector3.Distance(closest.enemyGameObject.transform.position, transform.position) <
                    unitScriptable.RangeToAttack)
                {
                    SetState(ActorReference.ElementAction.Attack);
                }
            }

            if (currentAction == ActorReference.ElementAction.Attack)
            {
                var closest = ClosestEnemy();
                
                if (closest == null)
                {
                    SetState(ActorReference.ElementAction.None);
                    return;
                }
                
                if (Vector3.Distance(closest.enemyGameObject.transform.position, transform.position) >
                    unitScriptable.RangeToAttack)
                {
                    SetState(ActorReference.ElementAction.None);
                }
                
                Attack(unitScriptable, closest);
            }

            if (currentAction == ActorReference.ElementAction.MoveToPoint)
            {
                MoveToTargetPoint(unitScriptable);
                
                if (Vector3.Distance(TargetPoint, transform.position) <
                    0.5f)
                {
                    SetState(ActorReference.ElementAction.None);
                }
            }

            if (currentAction == ActorReference.ElementAction.MoveToResource)
            {
                MoveToTargetPoint(unitScriptable);
                
                if (Vector3.Distance(TargetPoint, transform.position) <
                    0.5f)
                {
                    TargetPoint = ElementManager.Singleton.GetClosestElementOfType(ElementReference.Element.House).transform.position;
                    SetState(ActorReference.ElementAction.BringBackResource);
                }
            }

            if (currentAction == ActorReference.ElementAction.BringBackResource)
            {
                MoveToTargetPoint(unitScriptable);
                
                if (Vector3.Distance(TargetPoint, transform.position) <
                    0.5f)
                {
                    SetState(ActorReference.ElementAction.SeekClosestResource);
                }
            }

            if (currentAction == ActorReference.ElementAction.SeekClosestResource)
            {
                TargetPoint = ResourcesManager.Singleton.GetClosestResourceOfType().transform.position;
                SetState(ActorReference.ElementAction.MoveToResource);
            }
            
            
        }
        
        

        public void SetState(ActorReference.ElementAction action)
        {
            if (DebugUtility.DebugActors)
            {
                GetComponentInChildren<Text>().text = action.ToString();
            }
            
            currentAction = action;
        }

        void Idle(UnitScriptable unitScriptable)
        {
            // si autoAttackClosestEnemies
            if (unitScriptable.AutoAttackCloseEnemies)
            {
                // on récpère l'ennemi le plus proche
                var closestEnemy = ClosestEnemy();

                if (closestEnemy == null)
                {
                    return;
                    
                }

                if (Vector3.Distance(closestEnemy.enemyGameObject.transform.position, transform.position) <
                    unitScriptable.TriggerAutoAttackRange)
                {
                    SetState(ActorReference.ElementAction.MoveToEnemy);
                }
                
                // ClosestEnemy();

                // si il est a portée, on l'attaque
            }
            
        }

        void MoveToEnemy(UnitScriptable unitScriptable, EnemyManager.EnemyWithHealth closest)
        {
            Vector3 diff = closest.enemyGameObject.transform.position - transform.position;
            transform.position += diff.normalized * unitScriptable.MoveSpeed;
        }

        void MoveToTargetPoint(UnitScriptable unitScriptable)
        {
            Vector3 diff = TargetPoint - transform.position;
            transform.position += diff.normalized * unitScriptable.MoveSpeed;
        }

        void Attack(UnitScriptable unitScriptable, EnemyManager.EnemyWithHealth closest)
        {
            
            
            attaqueCompteur += Time.deltaTime;
            
            if (attaqueCompteur >= unitScriptable.DamageInterval)
            {
                attaqueCompteur = 0;
                
                closest.AddHealthAmount(-unitScriptable.DamageAmount);
            }
        }
        
        EnemyManager.EnemyWithHealth ClosestEnemy()
        {
            EnemyManager.EnemyWithHealth closestEnemy = null;
            float minDistance = Mathf.Infinity;
            
            foreach (var enemy in EnemyManager.Singleton._enemies)
            {
                float distance = Vector3.Distance(enemy.enemyGameObject.transform.position, transform.position);
                if (distance < minDistance)
                {
                    closestEnemy = enemy;
                    minDistance = distance;
                }
            }
            
            return closestEnemy;
        }
        

        
        
        
    }
