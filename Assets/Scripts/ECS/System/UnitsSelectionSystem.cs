using System.Collections.Generic;
using DefaultNamespace;
using ECS.Component;
using Mono.Actor;
using Mono.Element;
using Mono.Service;
using Scriptable.Scripts;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.AI;
using NotImplementedException = System.NotImplementedException;

namespace ECS.System
{
    public class UnitsSelectionSystem : ComponentSystem 
    {
        public static List<int> selectionUuids = new List<int>();
        private bool OnSelectionChanged = false;
        // private NativeArray<NavMeshAgent> test;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            SelectionService.OnSelectionChanged += (object sender, List<int> uuidsSelections) =>
            {
                Entities.ForEach((ref Element element, NavMeshObstacle obstacle, NavMeshAgent agent) =>
                {
                    if (selectionUuids.Contains(element.uuid))
                    {
                        // agent.enabled = false;
                        // obstacle.enabled = true;
                    }
                });
                
                selectionUuids = uuidsSelections;
                
                Entities.ForEach((ref Element element, NavMeshObstacle obstacle, NavMeshAgent agent) =>
                {
                    if (selectionUuids.Contains(element.uuid))
                    {
                        agent.enabled = true;
                        obstacle.enabled = false;
                    }
                });
                
                OnSelectionChanged = true;
                
                
                
            };
            
            SelectionService.OnElementAction += (object sender, ActorReference.ElementAndAction ElementAction) =>
            {
                ApplyElementActionOnUnitSelection(ElementAction);
            };
            
        }

        // public static NativeArray<int> GetSelection()
        // {
        //     NativeArray<int> nativeSelection = new NativeArray<int>();
        // }

        void AdaptMaterialToSelection()
        {
            EntityQuery entityQuery = GetEntityQuery(typeof(Element));
            
            NativeArray<Element> elements = entityQuery.ToComponentDataArray<Element>(Allocator.TempJob);
            NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.TempJob);
            
            NativeArray<Element> elementsInSelection = new NativeArray<Element>(selectionUuids.Count, Allocator.TempJob);
            NativeArray<Entity> entitiesInSelection = new NativeArray<Entity>(selectionUuids.Count, Allocator.TempJob);
            
            NativeArray<Element> elementsOutOfSelection = new NativeArray<Element>(entities.Length - selectionUuids.Count, Allocator.TempJob);
            NativeArray<Entity> entitiesOutOfSelection = new NativeArray<Entity>(entities.Length - selectionUuids.Count, Allocator.TempJob);
            
            int indexSelection = 0;
            int indexOutOfSelection = 0;
            
            for (int i = 0; i < elements.Length; i++)
            {
                if (selectionUuids.Contains(elements[i].uuid))
                {
                    entitiesInSelection[indexSelection] = entities[i];
                    elementsInSelection[indexSelection] = elements[i];
                    indexSelection++;
                }
                else
                {
                    elementsOutOfSelection[indexOutOfSelection] = elements[i];
                    entitiesOutOfSelection[indexOutOfSelection] = entities[i];
                    indexOutOfSelection++;
                }
            }
            
            for (int i = 0; i < elementsInSelection.Length; i++)
            {
                ElementScriptable elementScriptable =
                    ElementManager.Singleton.GetElementScriptableForElement(elementsInSelection[i].element);
            
                PostUpdateCommands.SetSharedComponent(entitiesInSelection[i], new RenderMesh
                {
                    mesh = elementScriptable.ghostBuildingMesh,
                    material = MaterialManager.Singleton.SelectedMaterial
                });
            }
            
            for (int i = 0; i < elementsOutOfSelection.Length; i++)
            {
                ElementScriptable elementScriptable =
                    ElementManager.Singleton.GetElementScriptableForElement(elementsOutOfSelection[i].element);
            
                PostUpdateCommands.SetSharedComponent(entitiesOutOfSelection[i], new RenderMesh
                {
                    mesh = elementScriptable.ghostBuildingMesh,
                    material = MaterialManager.Singleton.DefaultMaterial
                });
            }
            
            
            entitiesInSelection.Dispose();
            elementsInSelection.Dispose();
            entitiesOutOfSelection.Dispose();
            elementsOutOfSelection.Dispose();
            elements.Dispose();
            entities.Dispose();
        }
        
        void ApplyElementActionOnUnitSelection(ActorReference.ElementAndAction elementAction)
        {
            Entities.ForEach((ref Element element, ref Unit unit) =>
            {
                if (selectionUuids.Contains(element.uuid))
                {
                    unit.ElementAction = elementAction.ElementAction;
                }
            });
        }

        protected override void OnUpdate()
        {
            if (OnSelectionChanged)
            {
                AdaptMaterialToSelection();
                OnSelectionChanged = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (!Input.GetKey(KeyCode.C))
                {
                    float3 destinationPoint = RaycastUtility.RaycastPosition();
                    Dictionary<int, float3> UnitsPositions = new Dictionary<int, float3>();

                    float3 groupCenter = new float3();

                   
                    
                    
                    Entities.ForEach((ref Translation Translation, ref Element element) =>
                    {
                        if (selectionUuids.Contains(element.uuid))
                        {
                            groupCenter += Translation.Value;
                        }
                    });

                    groupCenter /= selectionUuids.Count;

                    
                    int gridX = (int)Mathf.Sqrt(selectionUuids.Count);
                    float interval = 20;
                    int posX = 0;
                    int posZ = 0;
                    

                    Entities.ForEach((ref Translation Translation, ref Element element) =>
                    {
                        if (selectionUuids.Contains(element.uuid))
                        {
                            // MAINTAIN DISTANCES BETWEEN UNITS 
                            // UnitsPositions.Add(element.uuid, Translation.Value - groupCenter);
                            
                            posZ = (posX - (posX % gridX)) / gridX;

                            float3 destinationPointResult = destinationPoint
                                            +new float3((int) ((posX%gridX) * interval), 0, (int) (posZ * interval));
                            
                            UnitsPositions.Add(element.uuid, new int3(destinationPointResult));
                            posX++;
                            // posX %= gridX;
                            
                            // Debug.Log("PosX ");

                            // agent.SetDestination();
                        }
                    });
                    
                    int priority = 0;
                
                    Entities.ForEach((NavMeshAgent agent, ref Element element) =>
                    {
                        if (selectionUuids.Contains(element.uuid))
                        {
                            // MAINTAIN DISTANCES BETWEEN UNITS 
                            // agent.SetDestination(distanceToPoint + UnitsPositions[element.uuid]);
                            agent.SetDestination(UnitsPositions[element.uuid]);
                            // agent.avoidancePriority = priority;
                            // priority++;
                        }
                        
                        
                    });
                }
                else
                {
                    int priority = 0;
                    
                    Entities.ForEach((NavMeshAgent agent, ref Unit unit, ref Element element) =>
                        {
                            if (selectionUuids.Contains(element.uuid) && RaycastHoveredSystem.resourceHoveredUuid == -1)
                            {
                                unit.ElementAction = ActorReference.ElementAction.MoveToPoint;
                                agent.SetDestination(RaycastUtility.RaycastPosition());
                                agent.avoidancePriority = priority;
                                priority++;
                            }
                        });
                    
                    
                    
                }
            }
        }
        
    }
}