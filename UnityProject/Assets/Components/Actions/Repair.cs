using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.Core;
using TL.Components;

namespace TL.Components.Actions
{
    [CreateAssetMenu(fileName = "Repair", menuName = "Components/Actions/Repair")]
    public class Repair : Action
    {
        public override void    Execute(NPCController npc, NPCAction action)
        {
            npc.DoRepair(3);
        }
    
        public override void SetRequiredDestinationPlace(NPCController npc, string locationName)
        {
            // float   distance = Mathf.Infinity;
            int     size = 0;
            Vector3 nearestPlace = Vector3.zero;

            List<GameObject> places = npc.context.Destinations[DestinationType.place];
            foreach (GameObject place in places)
            {
                // Debug.Log("place = " + place.transform.name + " locationName = " + locationName);
                if (place.name == locationName)
                {
                    size++;
                    // Debug.Log("locationName in action = " + locationName);
                    // float distanceFromPlace = Vector3.Distance(place.transform.position, npc.transform.position);
                    // if (distanceFromPlace < distance)
                    // {
                    //     nearestPlace = place.transform;
                    //     distance = distanceFromPlace;
                    // }
                    nearestPlace += place.transform.position;
                }
            }
            GameObject newObject = new GameObject("TransformObject");
            newObject.transform.position = nearestPlace / size;

            // Access the transform component of the newly created GameObject
            npc.RequiredDestination = newObject.transform;
            npc.mover.destination = npc.RequiredDestination;
        }
        
        public override void SetRequiredDestinationResource(NPCController npc, NPCAction action)
        {
            float distance = Mathf.Infinity;
            Transform nearestPlace = null;

            List<GameObject> resources = npc.context.Destinations[DestinationType.resource];
            foreach (GameObject resource in resources)
            {
                // Debug.Log("resource = " + resource.transform.name + " objectName = " + objectName);
                if (resource.name.Contains(action.object_name))
                {
                    if (action.location_name.Length == 0)
                    {
                        // Debug.Log("objectName in action = " + action.object_name + " && resource.name = " + resource.name);
                        float distanceFromPlace = Vector3.Distance(resource.transform.position, npc.transform.position);
                        if (distanceFromPlace <= 10f)
                        {
                            npc.RequiredDestination = resource.transform;
                            npc.mover.destination = npc.RequiredDestination; 
                            return ; 
                        }
                        if (distanceFromPlace < distance)
                        {
                            nearestPlace = resource.transform;
                            distance = distanceFromPlace;
                        }
                    }
                    else
                    {
                        List<GameObject> places = npc.context.Destinations[DestinationType.place];
                        foreach (GameObject place in places)
                        {
                            if (place.name == action.location_name)
                            {
                                // Debug.Log("locationName in action = " + action.object_name +  " && place.name = " + place.name);
                                float distanceFromPlace = Vector3.Distance(resource.transform.position, place.transform.position);
                                if (distanceFromPlace < distance)
                                {
                                    nearestPlace = resource.transform;
                                    distance = distanceFromPlace;
                                }
                            }
                        }
                    }
                }
            }

            if (!nearestPlace)
            {
                npc.RequiredDestination = npc.mover.agent.transform;
            }
            else
            {
                npc.RequiredDestination = nearestPlace;
            }
            npc.mover.destination = npc.RequiredDestination;
        }

        public override void    SetRequiredDestinationTarget(NPCController npc, string targetName)
        {
            float distance = Mathf.Infinity;
            Transform nearestPlace = null;

            List<GameObject> others = npc.context.Destinations[DestinationType.npc];
            foreach (GameObject other in others)
            {
                // Debug.Log("other = " + other.transform.name + " targetName = " + targetName);
                if (other.name.Contains(targetName))
                {
                    // Debug.Log("targetName in action = " + targetName + " && other.name = " + other.name);
                    float distanceFromPlace = Vector3.Distance(other.transform.position, npc.transform.position);
                    if (distanceFromPlace <= 10f)
                    {
                        npc.RequiredDestination = other.transform;
                        npc.mover.destination = npc.RequiredDestination; 
                        return ; 
                    }
                    if (distanceFromPlace < distance)
                    {
                        nearestPlace = other.transform;
                        distance = distanceFromPlace;
                    }
                }
            }

            if (!nearestPlace)
            {
                npc.RequiredDestination = npc.mover.agent.transform;
            }
            else
            {
                npc.RequiredDestination = nearestPlace;
            }
            npc.mover.destination = npc.RequiredDestination;
        }
    }
}
