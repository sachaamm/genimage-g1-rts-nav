using UnityEngine;

namespace Scriptable.Scripts
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/Unit", order = 0)]
    public class UnitScriptable : ElementScriptable
    {
        public float RangeToAttack = 1; // pour qu'une unité fasse des dégats il faut qu'elle soit dans la range "RangeToAttack"
        public float MoveSpeed = 5;

        public float DamageInterval = 1; // il infligue degats ttes les 1 secondes
        public int DamageAmount = 50;

        public bool AutoAttackCloseEnemies = false;
        public int TriggerAutoAttackRange = 2; // pour qu'une unité focus un enemi, et aille en direction de l'attaquer, il faut qu'il soit dans la range "TriggerAutoAttackRange"

        public int UnitPopulationCost = 1;

    }
}