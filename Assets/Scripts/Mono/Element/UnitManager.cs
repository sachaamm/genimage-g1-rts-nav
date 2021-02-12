using System;
using Scriptable.Scripts.Sets;
using UnityEngine;
using Unit = Scriptable.Scripts.Sets.Unit;

namespace Mono.Element
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Singleton;

        public UnitRuntimeSet UnitRuntimeSet;
        
        private void Awake()
        {
            Singleton = this;
        }

        public void AddUnitInRuntimeSet(GameObject unit, ElementReference.Element element)
        {
            // UnitRuntimeSet.Add( new Unit(unit, element));
        }

        public void RemoveUnitInRuntimeSet()
        {
            // TODO
        }

        void Update()
        {
            if (!Input.GetKey(KeyCode.U))
            {
                foreach (var unit in UnitRuntimeSet.Items)
                {
                    // unit.Update();
                }
            }
            
        }

        public static Unit GetUnitForGameObject(GameObject unit)
        {
            foreach (var a in Singleton.UnitRuntimeSet.Items)
            {
                if (a.unit == unit)
                {
                    return a;
                }
            }

            return null;
        }

        public void SetTargetPointToOtherUnit(GameObject otherUnit, Vector3 t)
        {
            GetUnitForGameObject(otherUnit).SetTargetPoint(t);
        }

        public void EnterNavStuck(GameObject unit, GameObject other)
        {
            GetUnitForGameObject(unit).OnTriggerEnter(other);
        }

        public void ExitNavStuck(GameObject unit)
        {
            GetUnitForGameObject(unit).OnTriggerExit();
        }
        
    }
}