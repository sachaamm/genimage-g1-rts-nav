using System;
using System.Collections.Generic;
using Mono.Actor;

namespace Mono.Service
{
    public class SelectionService
    {
        public static EventHandler<List<int>> OnSelectionChanged;
        public static EventHandler<ActorReference.ElementAndAction> OnElementAction;
        // public static EventHandler<UnitsSystemBase.MoveSelectionGroup> OnSelectionMoveToPoint;
        
        public static void SelectionChangedMessage(List<int> message)
        {
            OnSelectionChanged?.Invoke(null, message);
        }
        
        public static void ElementActionMessage(ActorReference.ElementAndAction message)
        {
            OnElementAction?.Invoke(null, message);
        }

        // public static void SelectionMoveToPointMessage(UnitsSystemBase.MoveSelectionGroup message)
        // {
        //     OnSelectionMoveToPoint?.Invoke(null, message);
        // }
    }
}