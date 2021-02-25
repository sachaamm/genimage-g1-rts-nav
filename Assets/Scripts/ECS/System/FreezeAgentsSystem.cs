using ECS.Component;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.System
{
    public class FreezeAgentsSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Entities.ForEach((NavMeshAgent agent, ref Element element) =>
                {
                    if (UnitsSelectionSystem.selectionUuids.Contains(element.uuid))
                    {
                        agent.enabled = false;
                    }
                });
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                Entities.ForEach((NavMeshAgent agent, ref Element element) =>
                {
                    if (UnitsSelectionSystem.selectionUuids.Contains(element.uuid))
                    {
                        agent.enabled = true;
                    }
                });
            }
        }
    }
}