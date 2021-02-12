using System.Collections.Generic;
using ECS.Component;
using Mono.Actor;
using Mono.Element;
using Mono.Service;
using Mono.Static;
using Mono.Targeting;
using Mono.Util;
using Scriptable.Scripts;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.System
{
    public class UnitsSystemBase : SystemBase
    {
        public struct MoveSelectionGroup
        {
            public List<int> selection;
            public Vector3 destination;
        }
        
        private MoveSelectionGroup moveSelectionGroup;
        private bool MoveSelection = false;
        protected override void OnCreate()
        {
            base.OnCreate();

            SelectionService.OnSelectionMoveToPoint += (sender, ints) =>
            {
                moveSelectionGroup = ints;
                
                MoveSelection = true;
            };
        }

        protected override void OnUpdate()
        {
            // L'unité récupère ses stats dans l'ElementManager
            
            Entities.ForEach((NavMeshAgent navMeshAgent, ref Unit unit, ref Element element, ref Translation translation) =>
            {
                
                var unitScriptable = 
                    ElementManager.Singleton.GetElementScriptableForElement(element.element);
                
                // Si l'action en cours est une action de déplacement
                if (ActorReference.IsMovingAction(unit.ElementAction))
                {
                    // on vérifie qu'on est pas bloqué par d'autres agents
                    if (unit.stuckInTrigger)
                    {
                        unit.stuckInTriggerCount++;

                        if (unit.stuckInTriggerCount > 200)
                        {
                            Release(ref unit); 
                        
                            // Debug.Log("Stuck");
                            // TODO
                            Vector3 diff = 
                                (translation.Value - unit.OtherStuckTranslation.Value) * 2;
                        
                            Vector3 unitPos = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                        
                            SetTargetPoint(unitPos +  diff, ref unit, ref translation, navMeshAgent);
                            // otherStuck.GetComponent<UnitBehaviour>().SetTargetPoint(otherStuck.transform.position - diff); // TODO
                            // UnitManager.Singleton.SetTargetPointToOtherUnit(otherStuck, otherStuck.transform.position - diff);
                        }

                    }
                    else
                    {
                        unit.stuckInTriggerCount = 0;
                    }
                
                }

                #region Actions

                if (unit.ElementAction == ActorReference.ElementAction.None)
                {
                    Idle(unitScriptable as UnitScriptable, translation, ref unit);
                }
            
                // il se déplace vers l'enemy le plus proche
                if(unit.ElementAction == ActorReference.ElementAction.MoveToEnemy)
                {
                    var closest = TargetManager.GetClosestEnemy(translation.Value);
                    // MoveToEnemy(unitScriptable,closest); // TODO

                    // TODO
                    // if (Vector3.Distance(closest.enemyGameObject.transform.position, unit.transform.position) <
                    //     unitScriptable.RangeToAttack)
                    // {
                    //    CurrentAction = ActorReference.ElementAction.Attack;
                    // }
                }

                if (unit.ElementAction == ActorReference.ElementAction.Attack)
                {
                    var closest = TargetManager.GetClosestEnemy(translation.Value);
                
                    if (closest == null)
                    {
                        unit.ElementAction = ActorReference.ElementAction.None;
                        return;
                    }
                
                    if (Vector3.Distance(closest.enemyGameObject.transform.position, translation.Value) >
                        UnitsReference.UnitMetadataMap[element.element].RangeToAttack)
                    {
                        unit.ElementAction = ActorReference.ElementAction.None;
                    }
                
                    // Attack(unitScriptable, closest);
                }
            
                if (!navMeshAgent.pathPending)
                {
                    if (navMeshAgent.remainingDistance < 1f)
                    {
                        // Debug.Log("OnTargetReached " + currentAction);
                        OnTargetReachedNextState(ref unit, ref translation, navMeshAgent);
                    }
                }

                #endregion
            
            }).WithoutBurst().Run();

            if (Input.GetMouseButton(1))
            {
                

            }

            if (MoveSelection)
            {
                // TODO Pass selection in a job to Burst
                // Performances médiocres a cause de l'event
                Entities.ForEach((NavMeshAgent nav, ref Unit unit, ref Element element, ref Translation translation) =>
                {
                    if (moveSelectionGroup.selection.Contains(element.uuid))
                    {
                        Release(ref unit);
                        SetTargetPoint(moveSelectionGroup.destination,ref unit, ref translation, nav);
                    }

                    unit.ElementAction = ActorReference.ElementAction.MoveToPoint;
                }).WithoutBurst().Run();

                MoveSelection = false;
            }
        }
        
        
        void OnTargetReachedNextState(ref Unit unit, ref Translation translation, NavMeshAgent navMeshAgent)
        {
            Vector3 newTargetPos = new Vector3();
            GameObject newTarget = null;
            
            if (unit.ElementAction == ActorReference.ElementAction.None || unit.ElementAction == ActorReference.ElementAction.MoveToPoint) return;

            bool matchCase = true;
            
            switch (unit.ElementAction)
            {
                case ActorReference.ElementAction.MoveToResource:
                    unit.ElementAction = ActorReference.ElementAction.BringBackResource;
                    GameObject resource = ResourcesManager.Singleton.GetClosestResourceOfType(translation.Value); // on relache la ressource vers laquelle on est allés, qui est la ressource la plus proche
                    ResourcesManager.Singleton.ReleaseResource(resource);
                    
                    newTarget =
                        ElementManager.Singleton.GetClosestElementOfType(ElementReference.Element.House,
                            translation.Value);
                    
                    newTargetPos = newTarget
                        .transform.position;
                    
                    
                    Debug.Log("Je retourne vers la maison");
                    
                    break;
                
                case ActorReference.ElementAction.BringBackResource:
                    unit.ElementAction = ActorReference.ElementAction.MoveToResource;
                    newTarget = ResourcesManager.Singleton.GetClosestAvaibleResourceOfType(translation.Value);
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
                // unitTarget = newTarget; // TODO
                SetTargetPoint(newTargetPos, ref unit, ref translation, navMeshAgent); // si on a pu trouvé un cas géré, on redéfini le targetPoint
                navMeshAgent.SetDestination(unit.TargetPoint); // on met a jour la destination du NavMeshAgent
            }
            else
            {
                Debug.Log(unit.ElementAction + " triggered any next state.");
            }
            
        }
        
        public void SetTargetPoint(Vector3 p, ref Unit unit, ref Translation translation, NavMeshAgent navMeshAgent)
        {
            // soit le point p est sur le navmesh

            // soit le point p est sur le navmesh
            NavMeshHit hit;

            // Par rapport à l'endroit idéal ou on veut aller, on veux récupérer la position la plus proche contenue dans le navMesh
            
            // on utilise position qui est un décalage de la position p vers le joueur
            Vector3 position = p + NavMeshUtility.GetDiffNormalizedFromPosition(translation.Value, p,1);
            
            if (NavMesh.SamplePosition(position, out hit, 10000.0f, NavMesh.AllAreas))
            {
                unit.TargetPoint = hit.position;
            }
            else
            {
                Debug.LogError("NavMeshHit failed");
            }
            
            if (ActorReference.IsMovingAction(unit.ElementAction))
            {
                // si c'est une action de déplacement alors on va vers la cible
                navMeshAgent.destination = unit.TargetPoint;
            }
            else
            {
                // si ce n'est pas une action de déplacement, la cible est soi-même
                navMeshAgent.destination = translation.Value;
            }
            
            // DebugUtility.InstantiateDebugPoint(p, "p");
            // DebugUtility.InstantiateDebugPoint(position, "position");
            // DebugUtility.InstantiateDebugPoint(targetPoint, "TargetPoint");
            
        }
        
        #region Action
        
        void Idle(UnitScriptable unitScriptable, Translation translation, ref Unit unit)
        {
            // si autoAttackClosestEnemies
            if (unitScriptable.AutoAttackCloseEnemies)
            {
                // on récpère l'ennemi le plus proche
                var closestEnemy = TargetManager.GetClosestEnemy(translation.Value);

                if (closestEnemy == null)
                {
                    return;
                }

                if (Vector3.Distance(closestEnemy.enemyGameObject.transform.position, translation.Value) <
                    unitScriptable.TriggerAutoAttackRange)
                {
                    unit.ElementAction = ActorReference.ElementAction.MoveToEnemy;
                }
                
                // ClosestEnemy();

                // si il est a portée, on l'attaque
            }
            
        }

        
        void Release(ref Unit unit)
        {
            if (unit.ElementAction == ActorReference.ElementAction.MoveToResource)
            {
                // On relache la ressource
                // ResourcesManager.Singleton.ReleaseResource(unitTarget);
                // TODO
            }
        }
        
        void MoveToEnemy(UnitsReference.UnitMetadata unitScriptable, EnemyManager.EnemyWithHealth closest, Translation translation)
        {
            Vector3 diff = closest.enemyGameObject.transform.position - ConversionUtility.Float3ToVec3(translation.Value);
            // unit.transform.position += diff.normalized * unitScriptable.MoveSpeed; // TODO, Broken !!!!
        }
        
        #endregion

    }
}