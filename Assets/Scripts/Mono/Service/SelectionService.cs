using System;
using System.Collections.Generic;
using ECS.System;
using Mono.Actor;

namespace Mono.Service
{
    public class SelectionService
    {
        public static EventHandler<List<string>> OnSelectionChanged;
        public static EventHandler<ActorReference.ElementAction> OnElementAction;
        public static EventHandler<UnitsSystemBase.MoveSelectionGroup> OnSelectionMoveToPoint;
        
        public static void SelectionChangedMessage(List<string> message)
        {
            OnSelectionChanged?.Invoke(null, message);
        }
        
        public static void ElementActionMessage(ActorReference.ElementAction message)
        {
            OnElementAction?.Invoke(null, message);
        }

        public static void SelectionMoveToPointMessage(UnitsSystemBase.MoveSelectionGroup message)
        {
            OnSelectionMoveToPoint?.Invoke(null, message);
        }
    }
}