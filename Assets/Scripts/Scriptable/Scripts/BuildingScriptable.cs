using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Scripts
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/BuildingScriptable", order = 0)]
    public class BuildingScriptable : ElementScriptable
    {
        public List<ElementReference.Element> producableUnits;
        // public Transform ghostBuildingTransform;
    }
}