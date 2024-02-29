using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Getting components by tag to use them in the code.
namespace TL.Core
{
    public class Context : MonoBehaviour
    {       
        public string resourceTag = "Resource";
        public string placeTag = "Place";
        public string npcTag = "Npc";
        public float MinDistance = 5f;
        public Dictionary<DestinationType, List<GameObject>> Destinations { get; private set; }

        void Start()
        {
            List<GameObject> resourceDestinations = GetAllResources();
            List<GameObject> placeDestinations = GetAllPlaces();
            List<GameObject> npcDestinations = GetAllNpcs();

            Destinations = new Dictionary<DestinationType, List<GameObject>>()
            {
                { DestinationType.resource, resourceDestinations },
                { DestinationType.place, placeDestinations },
                { DestinationType.npc, npcDestinations }
            };
        }

        private List<GameObject> GetAllResources()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(resourceTag);
            List<GameObject> resources = new List<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                resources.Add(go);
            }
            return resources;
        }

        private List<GameObject> GetAllPlaces()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(placeTag);
            List<GameObject> places = new List<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                places.Add(go);
            }
            return places;
        }

        private List<GameObject> GetAllNpcs()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(npcTag);
            List<GameObject> npcs = new List<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                npcs.Add(go);
            }
            return npcs;
        }

    }
}