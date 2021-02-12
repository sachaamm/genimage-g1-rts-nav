using System.Collections.Generic;
using Mono.Actor;
using UnityEngine;

namespace Scriptable.Scripts
{
    // [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/Element", order = 0)]
    public class ElementScriptable : UnityEngine.ScriptableObject
    {
        public GameObject Prefab;
        public Mesh ghostBuildingMesh;
        public Material elementMaterial;
        public int scaling = 50;
        
        
        public List<ActorReference.ElementAction> PossibleActions;
        public int MaxHealth = 100;
        public Sprite Icon;
        
        public int moneyCost = 5;
        public int gazCost = 5;

        
    }
}