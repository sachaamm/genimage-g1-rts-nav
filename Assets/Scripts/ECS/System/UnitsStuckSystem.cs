// using ECS.Component;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;
// using NotImplementedException = System.NotImplementedException;
//
// namespace ECS.System
// {
//     public class UnitsStuckSystem : SystemBase
//     {
//         protected override void OnUpdate()
//         {
//
//             EntityQuery entityQuery = GetEntityQuery(typeof(Unit), typeof(Translation), typeof(Element));
//             var translations = entityQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
//             var elements = entityQuery.ToComponentDataArray<Element>(Allocator.TempJob);
//             
//             Entities
//                 .ForEach((ref Translation translation, ref Unit unit
//                     ) =>
//                 {
//                     // units
//
//                     bool stuck = false;
//                     int otherStuckUuid = -1;
//                     float3 otherStuckTranslation = new float3();
//
//                     for (int i = 0; i < translations.Length; i++)
//                     {
//                         float distance = math.distance(translations[i].Value, translation.Value);
//                         
//                         if (distance < 20 && distance > 0.001f)
//                         {
//                             stuck = true;
//                             otherStuckUuid = elements[i].uuid;
//                             otherStuckTranslation = translations[i].Value;
//                         }
//                     }
//
//                     unit.stuckInTrigger = stuck;
//                     if (stuck)
//                     {
//                         unit.stuckUuid = otherStuckUuid;
//                         unit.otherStuckTranslation = otherStuckTranslation;
//                     }
//                   
//                 })
//                 .WithoutBurst().Run();
//
//             translations.Dispose();
//             elements.Dispose();
//         }
//     }
// }