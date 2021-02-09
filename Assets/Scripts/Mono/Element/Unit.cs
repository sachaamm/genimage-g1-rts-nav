using DefaultNamespace.Element;
using Mono.Util;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.UI;

// Script placé sur chaque Element "Unit"
public class Unit : MonoBehaviour
    {
        
        public ActorReference.ElementAction currentAction = ActorReference.ElementAction.None;

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
            // L'unité récupère ses stats dans l'ElementManager
            UnitScriptable unitScriptable = ElementManager.Singleton.GetElementScriptableForElement(_elementIdentity.Element) as UnitScriptable;
            
            if (currentAction == ActorReference.ElementAction.None)
            {
                Idle(unitScriptable);
            }
            
            // il se déplace vers l'enemy le plus proche
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

            // il se déplace vers le point que lui indique le joueur
            if (currentAction == ActorReference.ElementAction.MoveToPoint)
            {
                MoveToTargetPoint(unitScriptable);
                
                if (Vector3.Distance(TargetPoint, transform.position) <
                    0.5f)
                {
                    SetState(ActorReference.ElementAction.None);
                }
            }

            // l'ouvrier se déplace vers la ressource
            if (currentAction == ActorReference.ElementAction.MoveToResource)
            {
                MoveToTargetPoint(unitScriptable);
                
                // quand j'atteins la ressource
                if (Vector3.Distance(TargetPoint, transform.position) <
                    0.5f)
                {
                    // je définis comme target la maison la plus proche, pour revenir.
                    TargetPoint = ElementManager.Singleton.GetClosestElementOfType(ElementReference.Element.House).transform.position;
                    SetState(ActorReference.ElementAction.BringBackResource);
                }
            }

            // l'ouvrier ramène la ressource à la maison la plus proche
            if (currentAction == ActorReference.ElementAction.BringBackResource)
            {
                MoveToTargetPoint(unitScriptable);
                
                // Quand j'arrive à la maison
                if (Vector3.Distance(TargetPoint, transform.position) <
                    0.5f)
                {
                    // je me remets à chercher la ressource la plus proche
                    SetState(ActorReference.ElementAction.SeekClosestResource);
                    ResourcesManager.Singleton.AddMineral(8);

                }
            }

            // l'ouvrier chercher la resource la plus proche
            if (currentAction == ActorReference.ElementAction.SeekClosestResource)
            {
                TargetPoint = ResourcesManager.Singleton.GetClosestResourceOfType(transform.position).transform.position;
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
        
        // L'unité se déplace vers la cible "TargetPoint" ( Vector3 )
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
