using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.Core;

namespace TL.Components
{
    public abstract class Action : ScriptableObject
    {
        // Name of the action.
        public string       Name;

        public abstract void    Execute(NPCController npc, NPCAction action);
        
        // Function to manage the movement to the location_name provided by the request
        // I've used an algorithm to find the nearest component corresponding to the location but it was not efficient in some cases.
        // So for repair and move, I finally decided to get the center of the place by adding every position and divide by the number of elements.
        //  - A way of improvement is to find another way to calculate the center to avoid flooding the Tranform component creation.
        public abstract void    SetRequiredDestinationPlace(NPCController npc, string locationName);
        // Function to manage the movement to the object_name provided by the request.
        // There, I used the nearest object but in a restricted area if the location_name is set.
        public abstract void    SetRequiredDestinationResource(NPCController npc, NPCAction action);
        // Function to manage the movement to the target_name provided by the request.
        // There, I used the nearest target.
        public abstract void    SetRequiredDestinationTarget(NPCController npc, string targetName);
    }
}
