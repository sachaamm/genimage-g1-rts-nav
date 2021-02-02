using System.Collections.Generic;
using DefaultNamespace.Actor;
using UnityEngine;

namespace Scriptable.Scripts
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Element", order = 0)]
    public class ElementScriptable : UnityEngine.ScriptableObject
    {
        public GameObject Prefab;
        public List<ActorReference.ElementAction> PossibleActions;
        public int MaxHealth = 100;
        public bool IsMovable = false;
        

    }
}