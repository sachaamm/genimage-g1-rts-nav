using ECS.Component;
using Mono.Actor;
using Mono.Element;
using Scriptable.Scripts;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace Mono.Ecs
{
    public class EntityInstantiator : MonoBehaviour
    {
        public GameObject point;

        public static EntityManager EntityManager()
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager;
        }
        
        #region EntityArchetype

        static EntityArchetype UnitEntityArchetype()
        {
            return EntityManager().CreateArchetype(
                typeof(Translation),
                typeof(Rotation),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(ECS.Component.Element),
                typeof(NavMeshAgent),
                typeof(Unit)
            );
        }
        
        static EntityArchetype BuildingEntityArchetype()
        {
            return EntityManager().CreateArchetype(
                typeof(Translation),
                typeof(Rotation),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(ECS.Component.Element),
                typeof(Building)
            );
        }

        static EntityArchetype MineralEntityArchetype()
        {
            return EntityManager().CreateArchetype(
                typeof(Translation),
                typeof(Rotation),
                typeof(Scale),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld)
                
            );
        }
        
        #endregion

        public static void InstantiateUnitEntity(ElementReference.Element element, GameObject newElement)
        {
            ElementScriptable elementScriptable =
                ElementManager.Singleton.GetElementScriptableForElement(element);
            
            Unity.Entities.Entity unit = EntityManager().CreateEntity(UnitEntityArchetype());
            
            EntityManager().AddComponentData(unit, new Scale
            {
                Value = elementScriptable.scaling
            });
            
            EntityManager().AddComponentData(unit, new ECS.Component.Element
            {
                element = element,
                uuid = int.Parse(newElement.transform.name)
            });
            
            EntityManager().AddComponentData(unit, new Unit
            {
                ElementAction = ActorReference.ElementAction.None
            });
            
            EntityManager().AddSharedComponentData(unit, new RenderMesh
            {
                mesh = elementScriptable.ghostBuildingMesh,
                material = elementScriptable.elementMaterial
            });

            EntityManager().AddComponentObject(unit, newElement.GetComponent<NavMeshAgent>());
        }

        public static void InstantiateBuildingEntity(ElementReference.Element element, GameObject newElement)
        {
            ElementScriptable elementScriptable =
                ElementManager.Singleton.GetElementScriptableForElement(element);
            
            Unity.Entities.Entity unit = EntityManager().CreateEntity(BuildingEntityArchetype());
            
            EntityManager().AddComponentData(unit, new Scale
            {
                Value = elementScriptable.scaling
            });
            
            EntityManager().AddComponentData(unit, new ECS.Component.Element
            {
                element = element,
                uuid = int.Parse(newElement.transform.name)
            });
            
            EntityManager().AddSharedComponentData(unit, new RenderMesh
            {
                mesh = elementScriptable.ghostBuildingMesh,
                material = elementScriptable.elementMaterial
            });
        }
    }
}