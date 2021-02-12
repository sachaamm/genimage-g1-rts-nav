using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mono.Targeting
{
    public class TargetManager : MonoBehaviour
    {
        static TargetManager Singleton;

        private void Awake()
        {
            Singleton = this;
        }

        public struct ClosestResourceTargeting
        {
            
        }
        
        public static EnemyManager.EnemyWithHealth GetClosestEnemy(Vector3 position)
        {
            EnemyManager.EnemyWithHealth closest = null;
            
            float minDistance = Mathf.Infinity;
            
            foreach (var go in  EnemyManager.Singleton._enemies)
            {
                float distance = Vector3.Distance(go.enemyGameObject.transform.position, position);
                if (distance < minDistance)
                {
                    closest = go;
                    minDistance = distance;
                }
            }
            
            return closest;
        }
        

        public static GameObject GetClosestGameObject(List<GameObject> list, Vector3 position)
        {
            GameObject closest = null;
            
            float minDistance = Mathf.Infinity;
            
            foreach (var go in list)
            {
                float distance = Vector3.Distance(go.transform.position, position);
                if (distance < minDistance)
                {
                    closest = go;
                    minDistance = distance;
                }
            }
            
            return closest;
        }
    }
}