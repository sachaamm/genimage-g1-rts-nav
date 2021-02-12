using UnityEngine;

namespace Scriptable.Scripts
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "RtsTuto/Resource", order = 0)]
    public class ResourceScriptable : ScriptableObject
    {
        public GameObject Prefab;
        public Mesh mesh;
        public Material material;
        public int Scale = 30;
    }
}