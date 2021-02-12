using Mono.Actor;
using Mono.Element;
using Mono.Targeting;
using Mono.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Scriptable.Scripts.Sets
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/RuntimeSets/UnitRuntimeSet", order = 0)]
    public class UnitRuntimeSet : RuntimeSet<Unit>
    {
        
    }

    public class Unit
    {
        // Identity
        public GameObject unit;
        public ElementReference.Element Element;
        // Action 
        ActorReference.ElementAction currentAction = ActorReference.ElementAction.None;

        public ActorReference.ElementAction CurrentAction
        {
            get => currentAction;
            set => currentAction = value;
        }

        // Nav
        public NavMeshAgent navMeshAgent;
        public GameObject unitTarget;
        public Vector3 targetPoint;
        
        public int stuckTriggerCount = 0;
        public bool stuckInTrigger = false;
        
        public GameObject otherStuck;
        
        private float attaqueCompteur = 0;


        public Unit(GameObject unit, ElementReference.Element element)
        {
            this.unit = unit;
            Element = element;

            navMeshAgent = unit.GetComponent<NavMeshAgent>();
        }
        
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
            Vector3 position = p + NavMeshUtility.GetDiffNormalizedFromPosition(unit.transform.position, p,1);
            

            if (NavMesh.SamplePosition(position, out hit, 10000.0f, NavMesh.AllAreas))
            {
                targetPoint = hit.position;
            }
            else
            {
                Debug.LogError("NavMeshHit failed");
            }
            
            if (ActorReference.IsMovingAction(currentAction))
            {
                // si c'est une action de déplacement alors on va vers la cible
                navMeshAgent.destination = targetPoint;
            }
            else
            {
                // si ce n'est pas une action de déplacement, la cible est soi-même
                navMeshAgent.destination = unit.transform.position;
            }
            
            // DebugUtility.InstantiateDebugPoint(p, "p");
            // DebugUtility.InstantiateDebugPoint(position, "position");
            // DebugUtility.InstantiateDebugPoint(targetPoint, "TargetPoint");
            
        }
        
        public void Update()
        {
            // L'unité récupère ses stats dans l'ElementManager
            UnitScriptable unitScriptable = ElementManager.Singleton.GetElementScriptableForElement(Element) as UnitScriptable;

            // Si l'action en cours est une action de déplacement
            if (ActorReference.IsMovingAction(currentAction))
            {
                // on vérifie qu'on est pas bloqué par d'autres agents
                if (stuckInTrigger)
                {
                    stuckTriggerCount++;

                    if (stuckTriggerCount > 200)
                    {
                        Release();
                        
                        // Debug.Log("Stuck");
                        // destinationPointBeforeStuck = targetPoint;
                        Vector3 diff = 
                            (unit.transform.position - otherStuck.transform.position) * 2;
                        SetTargetPoint(unit.transform.position + diff);
                        // otherStuck.GetComponent<UnitBehaviour>().SetTargetPoint(otherStuck.transform.position - diff);
                        UnitManager.Singleton.SetTargetPointToOtherUnit(otherStuck, otherStuck.transform.position - diff);
                    }

                }
                else
                {
                    stuckTriggerCount = 0;
                }
                
            }

            #region Actions

            if (currentAction == ActorReference.ElementAction.None)
            {
                Idle(unitScriptable);
            }
            
            // il se déplace vers l'enemy le plus proche
            if(currentAction == ActorReference.ElementAction.MoveToEnemy)
            {
                var closest = TargetManager.GetClosestEnemy(unit.transform.position);
                MoveToEnemy(unitScriptable,closest);

                if (Vector3.Distance(closest.enemyGameObject.transform.position, unit.transform.position) <
                    unitScriptable.RangeToAttack)
                {
                   CurrentAction = ActorReference.ElementAction.Attack;
                }
            }

            if (currentAction == ActorReference.ElementAction.Attack)
            {
                var closest = TargetManager.GetClosestEnemy(unit.transform.position);
                
                if (closest == null)
                {
                    CurrentAction = ActorReference.ElementAction.None;
                    return;
                }
                
                if (Vector3.Distance(closest.enemyGameObject.transform.position, unit.transform.position) >
                    unitScriptable.RangeToAttack)
                {
                    CurrentAction = ActorReference.ElementAction.None;
                }
                
                Attack(unitScriptable, closest);
            }
            
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance < 1f)
                {
                    // Debug.Log("OnTargetReached " + currentAction);
                    OnTargetReachedNextState();
                }
            }

            #endregion
            
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
                    currentAction = ActorReference.ElementAction.BringBackResource;
                    GameObject resource = ResourcesManager.Singleton.GetClosestResourceOfType(unit.transform.position); // on relache la ressource vers laquelle on est allés, qui est la ressource la plus proche
                    ResourcesManager.Singleton.ReleaseResource(resource);
                    
                    newTarget =
                        ElementManager.Singleton.GetClosestElementOfType(ElementReference.Element.House,
                            unit.transform.position);
                    
                    newTargetPos = newTarget
                        .transform.position;
                    
                    
                    Debug.Log("Je retourne vers la maison");
                    
                    break;
                
                case ActorReference.ElementAction.BringBackResource:
                    CurrentAction = ActorReference.ElementAction.MoveToResource;
                    newTarget = ResourcesManager.Singleton.GetClosestAvaibleResourceOfType(unit.transform.position);
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
        
        void Idle(UnitScriptable unitScriptable)
        {
            // si autoAttackClosestEnemies
            if (unitScriptable.AutoAttackCloseEnemies)
            {
                // on récpère l'ennemi le plus proche
                var closestEnemy = TargetManager.GetClosestEnemy(unit.transform.position);

                if (closestEnemy == null)
                {
                    return;
                }

                if (Vector3.Distance(closestEnemy.enemyGameObject.transform.position, unit.transform.position) <
                    unitScriptable.TriggerAutoAttackRange)
                {
                    CurrentAction = ActorReference.ElementAction.MoveToEnemy;
                }
                
                // ClosestEnemy();

                // si il est a portée, on l'attaque
            }
            
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
        
        void MoveToEnemy(UnitScriptable unitScriptable, EnemyManager.EnemyWithHealth closest)
        {
            Vector3 diff = closest.enemyGameObject.transform.position - unit.transform.position;
            unit.transform.position += diff.normalized * unitScriptable.MoveSpeed;
        }
        
        public void OnTriggerEnter(GameObject other)
        {
            stuckInTrigger = true;
            otherStuck = other.gameObject; 
        }

        public void OnTriggerExit()
        {
            stuckInTrigger = false;
            stuckTriggerCount = 0;
        }
    }
}