using System;
using System.Collections.Generic;
using Mono.Actor;

namespace Mono.Service
{
    public class SelectionService
    {
        public static EventHandler<List<string>> OnSelectionChanged;
        public static EventHandler<ActorReference.ElementAction> OnElementAction;
        
        public static void SelectionChangedMessage(List<string> message)
        {
            OnSelectionChanged?.Invoke(null, message);
        }
        
        public static void ElementActionMessage(ActorReference.ElementAction message)
        {
            OnElementAction?.Invoke(null, message);
        }
    }
}