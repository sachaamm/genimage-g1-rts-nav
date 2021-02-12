using Unity.Mathematics;
using UnityEngine;

namespace Mono.Util
{
    public class ConversionUtility
    {
        public static Vector3 Float3ToVec3(float3 float3)
        {
            return new Vector3(float3.x, float3.y, float3.z);
        }
    }
}