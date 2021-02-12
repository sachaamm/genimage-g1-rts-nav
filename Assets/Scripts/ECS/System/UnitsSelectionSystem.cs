using System.Collections.Generic;
using System.ComponentModel;
using DefaultNamespace;
using ECS.Component;
using Mono.Actor;
using Mono.Element;
using Mono.Service;
using Scriptable.Scripts;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace ECS.System
{
    public class UnitsSelectionSystem : ComponentSystem 
    {
        EntityQuery m_Group;
        private List<int> selectionUuids = new List<int>();
        private bool OnSelectionChanged = false;
        
        
        public EntityCommandBuffer.Concurrent CommandBuffer;
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        
        
        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            
            base.OnCreate();
            
            m_Group = GetEntityQuery(typeof(ECS.Component.Element), typeof(Unit));
            
            SelectionService.OnSelectionChanged += (object sender, List<string> uuidsSelections) =>
            {
                Debug.Log("selection changed");
                selectionUuids = new List<int>();

                foreach (var a in uuidsSelections)
                {
                    selectionUuids.Add(int.Parse(a));
                }
                
                OnSelectionChanged = true;
                
                _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
                // AdaptMaterialToSelection();
            };
            
            SelectionService.OnElementAction += (object sender, ActorReference.ElementAction ElementAction) =>
            {
                ApplyElementActionOnUnitSelection(ElementAction);
            };
        }

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
            
            
            
            elements.Dispose();
            entities.Dispose();
            entitiesInSelection.Dispose();
            elementsInSelection.Dispose();
            entitiesOutOfSelection.Dispose();
            elementsOutOfSelection.Dispose();
        }
        
        void ApplyElementActionOnUnitSelection(ActorReference.ElementAction elementAction)
        {
            Entities.ForEach((ref Element element, ref Unit unit) =>
            {
                if (selectionUuids.Contains(element.uuid))
                {
                    unit.ElementAction = elementAction;
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
            // AdaptMaterialToSelection();
        }
    }
}