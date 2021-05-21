using ECS.Component;
using Mono.Actor;
using Mono.Element;
using Mono.Service;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace ECS.System
{
    public class UnitsReceiveActionSystem : JobComponentSystem
    {
        private bool receiveAction = false;
        private ActorReference.ElementAndAction _elementAndAction;
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            SelectionService.OnElementAction += (sender, action) =>
            {
                _elementAndAction = action;
                receiveAction = true;
            };
            
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // Update action from selection ( from an ActionButton )
            if (receiveAction)
            {
                EntityQuery entityQuery = GetEntityQuery(typeof(Element), typeof(Translation));

                var entities = entityQuery.ToEntityArray(Allocator.TempJob);
                var elements = entityQuery.ToComponentDataArray<Element>(Allocator.TempJob);
                var positions = entityQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

                if (_elementAndAction.ElementAction == ActorReference.ElementAction.CreateWorker)
                {
                    for (int i = 0; i < entities.Length; i++)
                    {
                        if (Selection.UuidSelection().Contains(elements[i].uuid) &&
                            elements[i].element == _elementAndAction.Element)
                        {
                            // GameObject newUnit =
                            //     ElementManager.Singleton.InstantiateElement(ElementReference.Element.Worker,
                            //         positions[i].Value);
                        }
                    }
                }

                if (_elementAndAction.ElementAction == ActorReference.ElementAction.CreateBuilding)
                {
                    UiManager.Singleton.EnterInCreateBuildingSubmenu();
                }

                entities.Dispose();
                elements.Dispose();
                positions.Dispose();
                
                receiveAction = false;
            }
            
            return new JobHandle();
        }
        
    }
}