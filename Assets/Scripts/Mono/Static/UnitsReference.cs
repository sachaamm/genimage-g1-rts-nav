using System.Collections.Generic;

namespace Mono.Static
{
    public class UnitsReference
    {
        public class UnitMetadata
        {
            public float RangeToAttack = 1; // pour qu'une unité fasse des dégats il faut qu'elle soit dans la range "RangeToAttack"
            public float MoveSpeed = 5;

            public float DamageInterval = 1; // il infligue degats ttes les 1 secondes
            public int DamageAmount = 50;

            public bool AutoAttackCloseEnemies = false;
            public int TriggerAutoAttackRange = 2; // pour qu'une unité focus un enemi, et aille en direction de l'attaquer, il faut qu'il soit dans la range "TriggerAutoAttackRange"

            public int UnitPopulationCost = 1;

            public UnitMetadata(
                float rangeToAttack, 
                float moveSpeed, 
                float damageInterval, 
                int damageAmount, 
                bool autoAttackCloseEnemies, 
                int triggerAutoAttackRange, 
                int unitPopulationCost)
            {
                RangeToAttack = rangeToAttack;
                MoveSpeed = moveSpeed;
                DamageInterval = damageInterval;
                DamageAmount = damageAmount;
                AutoAttackCloseEnemies = autoAttackCloseEnemies;
                TriggerAutoAttackRange = triggerAutoAttackRange;
                UnitPopulationCost = unitPopulationCost;
            }
        }
        
        public static Dictionary<ElementReference.Element,UnitMetadata> UnitMetadataMap = new Dictionary<ElementReference.Element,UnitMetadata>
        {
            {
               ElementReference.Element.Worker, 
               new UnitMetadata(1,0.01f,1,50,false,2,1)
            }
        };
    }
}