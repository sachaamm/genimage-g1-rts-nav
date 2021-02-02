using UnityEngine;

namespace Scriptable.Scripts
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/BuildingScriptable", order = 0)]
    public class BuildingScriptable : ElementScriptable
    {
        public int moneyCost = 5;
        public int gazCost = 5;
    }
}