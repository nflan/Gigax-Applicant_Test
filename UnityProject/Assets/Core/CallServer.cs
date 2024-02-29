using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;


namespace TL.Core
{
    public class NPCAction
    {
        public string npc_name;
        public string action;
        public string location_name;
        public string object_name;
        public string content;
        public string target_name;
    }

    /* Class to get informations from FastAPI server.
        Ways of improvement:
            - Create Websocket to not flood the server with get request if there is no informations and to avoid waiting 1 sec between calls.
            - Add security checks
    */
    public class CallServer : MonoBehaviour
    {
        private const string url = "http://localhost:8000/get_data"; // Put FastAPI url to get informations

        private float pollingInterval = 1f; // Polling interval in seconds

        private Dictionary<string, GameObject> npcObjectMap = new Dictionary<string, GameObject>();

        void Start()
        {
            // Populate the dictionary with npc_name and corresponding GameObjects
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Npc"))
            {
                npcObjectMap.Add(obj.name, obj);
            }
            StartCoroutine(PollForData()); // Pass the name of the coroutine method
        }

        IEnumerator PollForData()
        {
            while (true)
            {
                yield return new WaitForSeconds(pollingInterval);

                using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.result == UnityWebRequest.Result.Success)
                    {
                        if (webRequest.downloadHandler.text != "{\"message\":\"No data available\"}")
                        {
                            NPCAction npc = JsonUtility.FromJson<NPCAction>(webRequest.downloadHandler.text);
                            GameObject obj = GameObject.Find(npc.npc_name);
                            if (obj != null)
                            {
                                // Debug.Log("Npc is: " + npc.npc_name + " locationName is: " + npc.location_name);
                                NPCController npcController = obj.GetComponent<NPCController>();
                                if (npcController != null)
                                {
                                    npcController.putData(npc);
                                }
                            }
                            else
                            {
                                Debug.Log("No npc with this name: " + npc.npc_name);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to get data from server: " + webRequest.error);
                    }
                }
            }
        }
    }
}
