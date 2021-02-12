using System;
using ECS.Component;
using Mono.Actor;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Mono.Ecs
{
    public class EntityQueryManager : MonoBehaviour
    {
        public static EntityQueryManager Singleton;

        private void Awake()
        {
            Singleton = this;
        }

        public Unity.Entities.Entity GetEntityForGameObject(GameObject gameObject)
        {
            int uuid = int.Parse(gameObject.transform.name);
            
            // Unity.Entities.Entity entity = null;
            EntityQuery m_Group = EntityInstantiator.EntityManager().CreateEntityQuery(typeof(ECS.Component.Element));
            var entities = m_Group.ToEntityArray(Allocator.Temp);
            NativeArray<ECS.Component.Element> elementsEntities = m_Group.ToComponentDataArray<ECS.Component.Element>(Allocator.Temp);
            
            for (int i = 0; i < elementsEntities.Length; i++)
            {
                if (elementsEntities[i].uuid == uuid)
                {
                    return entities[i];
                }
            }
            
            Debug.LogError("entity Not found for uuid " + uuid + ".");

            return new Unity.Entities.Entity();
            // return null;
        }

        public void SetActionToUnit(GameObject gameObject, ActorReference.ElementAction elementAction)
        {
            
        }
    }
}