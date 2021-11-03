﻿using ECS.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.System
{
    public class RaycastHoveredSystem : SystemBase
    {
        public static int resourceHoveredUuid = -1;
        public static float3 resourceHoveredPos = new float3();

        protected override void OnUpdate()
        {
            float3 raycastPos = RaycastUtility.RaycastPosition();

            bool match = false;
        
            Entities.ForEach((ref Resource resource, ref Translation translation) =>
            {
                if(math.distance(new float2(raycastPos.x, raycastPos.z), new float2(translation.Value.x, translation.Value.z)) < 10f)
                {
                    resourceHoveredUuid = resource.uuid;
                    resourceHoveredPos = translation.Value;
                    match = true;
                }
                
            }).WithoutBurst().Run();

            if (!match)
            {
                resourceHoveredUuid = -1;
            }
        }
    }
}