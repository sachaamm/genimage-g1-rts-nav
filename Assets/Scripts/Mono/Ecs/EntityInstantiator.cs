using ECS.Component;
using Mono.Actor;
using Mono.Element;
using Scriptable.Scripts;
using Unity.Entities;
// using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using BoxCollider = UnityEngine.BoxCollider;

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

        public static EntityArchetype UnitEntityArchetype()
        {
            return EntityManager().CreateArchetype(
                typeof(Translation),
                typeof(Rotation),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(ECS.Component.Element),
                typeof(NavMeshAgent),
                typeof(NavMeshObstacle),
                typeof(Unit),
                typeof(UnitTarget)
                // , typeof(PhysicsCollider)
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

        static EntityArchetype ResourceEntityArchetype()
        {
            return EntityManager().CreateArchetype(
                typeof(Translation),
                typeof(Rotation),
                typeof(Scale),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(Resource)
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

            // BoxCollider boxCollider = newElement.GetComponent<BoxCollider>();
            

            // EntityManager().AddComponentData(unit, new PhysicsCollider
            // {
            //     Value = boxCollider
            // });

            EntityManager().AddComponentObject(unit, newElement.GetComponent<NavMeshAgent>());
            EntityManager().AddComponentObject(unit, newElement.GetComponent<NavMeshObstacle>());
        }

        public static void InstantiateBuildingEntity(ElementReference.Element element, GameObject newElement)
        {
            ElementScriptable elementScriptable =
                ElementManager.Singleton.GetElementScriptableForElement(element);
            
            Unity.Entities.Entity unit = EntityManager().CreateEntity(BuildingEntityArchetype());
            
            EntityManager().AddComponentData(unit, new Translation
            {
                Value = newElement.transform.position
            });
            
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

        private static int resourceUuid = 0;

        public static void InstantiateResourceEntity(ResourcesReference.Resource resource, Vector3 pos)
        {
            ResourceScriptable resourceScriptable = ResourcesManager.Singleton.GetResourceScriptable(resource);
            
            Unity.Entities.Entity resourceEntityArchetype = EntityManager().CreateEntity(ResourceEntityArchetype());
            resourceUuid++;
            
            EntityManager().AddSharedComponentData(resourceEntityArchetype, new RenderMesh
            {
                mesh = resourceScriptable.mesh,
                material = resourceScriptable.material
            });

            EntityManager().AddComponentData(resourceEntityArchetype, new Resource
            {
                IsMineral = resource == ResourcesReference.Resource.MineralField,
                Available = true,
                uuid = resourceUuid
            });

            EntityManager().AddComponentData(resourceEntityArchetype, new Translation
            {
                Value = pos
            });
            
            EntityManager().AddComponentData(resourceEntityArchetype, new Scale
            {
                Value = resourceScriptable.Scale
            });



            // EntityManager().AddComponentData()
        }
    }
}