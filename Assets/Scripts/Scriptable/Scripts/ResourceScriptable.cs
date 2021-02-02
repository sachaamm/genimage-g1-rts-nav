using UnityEngine;

namespace Scriptable.Scripts
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/Resource", order = 0)]
    public class ResourceScriptable : ScriptableObject
    {
        public GameObject Prefab;
    }
}