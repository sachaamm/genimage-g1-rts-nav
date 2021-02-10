using System;
using System.Resources;
using DefaultNamespace.Element;
using Mono.Util;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Script placé sur chaque Element "Unit"
public class Unit : MonoBehaviour
    {
        
        ActorReference.ElementAction currentAction = ActorReference.ElementAction.None;

        private float attaqueCompteur = 0;

        private ElementIdentity _elementIdentity;


        Vector3 targetPoint;

        NavMeshAgent navMeshAgent;

        private bool start = false;

        public GameObject unitTarget;

        private int triggerCount = 0;
        private bool inTrigger = false;

        private Vector3 destinationPointBeforeStuck = new Vector3();

        private GameObject otherStuck;
        
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            _elementIdentity = GetComponent<ElementIdentity>();
            SetState(ActorReference.ElementAction.None);

        }

        bool IsTargetingAction(ActorReference.ElementAction elementAction)
        {
            // return elementAction == ActorReference.ElementAction.MoveToResource || elementAction == ActorReference.ElementAction.BringBackResource;
            return elementAction == ActorReference.ElementAction.BringBackResource;
        }

        // Quand j'arrete l'action avant d'avoir atteint ma cible
        public void Release()
        {
            if (currentAction == ActorReference.ElementAction.MoveToResource)
            {
                // On relache la ressource
                ResourcesManager.Singleton.ReleaseResource(unitTarget);
            }
        }
        

        public void SetTargetPoint(Vector3 p)
        {
            
            
            // soit le point p est sur le navmesh

            // soit le point p est sur le navmesh
            NavMeshHit hit;

            // Par rapport à l'endroit idéal ou on veut aller, on veux récupérer la position la plus proche contenue dans le navMesh
            
            // on utilise position qui est un décalage de la position p vers le joueur
            Vector3 position = p + NavMeshUtility.GetDiffNormalizedFromPosition(transform.position, p,1);
            

            if (NavMesh.SamplePosition(position, out hit, 10000.0f, NavMesh.AllAreas))
            {
                targetPoint = hit.position;
            }
            else
            {
                Debug.LogError("NavMeshHit failed");
            }
            
            
            if (IsTargetingAction(currentAction))
            {
                

            }
            else
            {
                // targetPoint = p;
            }
            
            
            if (IsMovingAction())
            {
                // si c'est une action de déplacement alors on va vers la cible
                navMeshAgent.destination = targetPoint;
            }
            else
            {
                // si ce n'est pas une action de déplacement, la cible est soi-même
                navMeshAgent.destination = transform.position;
            }
            
            DebugUtility.InstantiateDebugPoint(p, "p");
            DebugUtility.InstantiateDebugPoint(position, "position");
            DebugUtility.InstantiateDebugPoint(targetPoint, "TargetPoint");
            
        }

        bool IsMovingAction()
        {
            return currentAction != ActorReference.ElementAction.None;
        }

        private void Update()
        {
            // L'unité récupère ses stats dans l'ElementManager
            UnitScriptable unitScriptable = ElementManager.Singleton.GetElementScriptableForElement(_elementIdentity.Element) as UnitScriptable;

            // Si l'action en cours est une action de déplacement
            if (IsMovingAction())
            {
                // on vérifie qu'on est pas bloqué par d'autres agents
                if (inTrigger)
                {
                    triggerCount++;

                    if (triggerCount > 200)
                    {
                        Debug.Log("Stuck");
                        destinationPointBeforeStuck = targetPoint;
                        Vector3 diff = 
                            transform.position - otherStuck.transform.position;
                        SetTargetPoint(diff * 5);
                        otherStuck.GetComponent<Unit>().SetTargetPoint(-diff * 5);
                    }
                    
                    
                }
                else
                {
                    triggerCount = 0;
                }
                
            }
            
            
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
            
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance < 1f)
                {
                    Debug.Log("OnTargetReached " + currentAction);
                    OnTargetReachedNextState();
                }
            }
            
            // l'ouvrier chercher la resource la plus proche
            // if (currentAction == ActorReference.ElementAction.SeekClosestResource)
            // {
            //     SetTargetPoint(ResourcesManager.Singleton.GetClosestResourceOfType(transform.position).transform.position);             
            //     SetState(ActorReference.ElementAction.MoveToResource);
            // }
            
            
        }

        // Le prochain state quand on a atteint la cible
        void OnTargetReachedNextState()
        {
            Vector3 newTargetPos = new Vector3();
            GameObject newTarget = null;
            
            if (currentAction == ActorReference.ElementAction.None || currentAction == ActorReference.ElementAction.MoveToPoint) return;

            bool matchCase = true;
            
            switch (currentAction)
            {
                case ActorReference.ElementAction.MoveToResource:
                    SetState(ActorReference.ElementAction.BringBackResource);
                    GameObject resource = ResourcesManager.Singleton.GetClosestResourceOfType(transform.position); // on relache la ressource vers laquelle on est allés, qui est la ressource la plus proche
                    ResourcesManager.Singleton.ReleaseResource(resource);
                    
                    newTarget =
                        ElementManager.Singleton.GetClosestElementOfType(ElementReference.Element.House,
                            transform.position);
                    
                    newTargetPos = newTarget
                        .transform.position;
                    
                    
                    Debug.Log("Je retourne vers la maison");
                    
                    break;
                
                case ActorReference.ElementAction.BringBackResource:
                    SetState(ActorReference.ElementAction.MoveToResource);
                    newTarget = ResourcesManager.Singleton.GetClosestAvaibleResourceOfType(transform.position);
                    newTargetPos = newTarget.transform.position;
                    ResourcesManager.Singleton.AccaparateResource(newTarget);
                    ResourcesManager.Singleton.AddMineral(8);
                    
                    
                    Debug.Log("Je retourne chercher du minerai");
                    break;
                
                default:
                    matchCase = false; // on a pas trouvé de cas géré
                    break;
                    
            }

            if (matchCase)
            {
                unitTarget = newTarget;
                SetTargetPoint(newTargetPos); // si on a pu trouvé un cas géré, on redéfini le targetPoint
                navMeshAgent.SetDestination(targetPoint); // on met a jour la destination du NavMeshAgent
            }
            else
            {
                Debug.Log(currentAction + " triggered any next state.");
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
            navMeshAgent.SetDestination(targetPoint);
            // Vector3 diff = TargetPoint - transform.position;
            // transform.position += diff.normalized * unitScriptable.MoveSpeed;
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


        private void OnTriggerEnter(Collider other)
        {
            inTrigger = true;
            otherStuck = other.gameObject;

        }

        private void OnTriggerExit(Collider other)
        {
            inTrigger = false;
            triggerCount = 0;
        }
    }
