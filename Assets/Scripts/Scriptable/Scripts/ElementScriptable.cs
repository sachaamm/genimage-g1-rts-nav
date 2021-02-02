using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Scripts
{
    // [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/Element", order = 0)]
    public class ElementScriptable : UnityEngine.ScriptableObject
    {
        public GameObject Prefab;
        public List<ActorReference.ElementAction> PossibleActions;
        public int MaxHealth = 100;
        public Sprite Icon;

    }
}