using UnityEngine;

namespace Scriptable.Scripts
{
    
    public class EnemyScriptable : ScriptableObject
    {
        public GameObject Prefab;
        
        public float RangeToAttack = 5;
        public float MoveSpeed = 5;

        public float DamageInterval = 1; // il inflige degats ttes les 1 secondes
        public int DamageAmount = 50;
        
        public int MaxHealth = 100;
    }
}