using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;
using UnityEngine.AI;

// Manage movement of the NPCs.
namespace TL.Core
{
    public class MoveController : MonoBehaviour
    {
        public NavMeshAgent         agent;
        public Transform            destination;
        public ThirdPersonCharacter character;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
        }

        // Update is called once per frame
        void Update()
        {}

        // If the NPC is not the as close as StoppingDistance of the object / place / target, it moves without crounching or jumping
        // If he is, we just stop animation
        // used third video there: https://learn.unity.com/tutorial/unity-navmesh?language=en&projectId=5f60d859edbc2a001ee947ea#5c7f8528edbc2a002053b499
        public void MoveTo(Vector3 position)
        {
            position.y = 0.0f;
            agent.SetDestination(position);

            if (Vector3.Distance(agent.transform.position, position) > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }
    }
}
