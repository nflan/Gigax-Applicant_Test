using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TL.Components;

namespace TL.Core
{
    public enum State
    {
        idle,
        move,
        target,
        resource,
        execute
    }

    public class NPCController : MonoBehaviour
    {
        public MoveController   mover { get; set; }
        public CallServer       server { get; set; }
        public Action[]         actionsAvailable;
        public Action           actionToDo { get; set; }
        public State            currentState { get; set; }
        public Context          context;
        public List<NPCAction>  actions = new List<NPCAction>();
        public bool             finishedTask { get; set; }
        public Transform        RequiredDestination { get; set; }
        public SpeechBubble     canva;
        public Animator         animator;


        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<MoveController>();
            animator = GetComponent<Animator>();
            finishedTask = false;
        }

        // Update is called once per frame
        void Update()
        {
            FMSTick();
        }

        // Check state to know what the NPC is doing. Doing the actions form the beginning to the end, delete the actions[0] at the end to manage infinite actions.
        public void FMSTick()
        {
            if (currentState == State.idle)
            {
                mover.MoveTo(this.transform.position);
                if (actions.Count > 0)
                {
                    actionToDo = findAction();
                    if (actionToDo is null)
                    {
                        Debug.Log("Action not found!");
                        actions.RemoveAt(0);
                        return;
                    }
                    finishedTask = false;
                    if (actions[0].location_name.Length == 0)
                    {
                        manageState();
                    }
                    else
                    {
                        //Debug.Log("action[0].location_name = " + actions[0].location_name);
                        actionToDo.SetRequiredDestinationPlace(this, actions[0].location_name);
                        if (Vector3.Distance(RequiredDestination.position, this.transform.position) < 2f)
                        {
                            manageState();
                        }
                        else
                        {
                            currentState = State.move;
                        }
                    }
                }
            }
            else if (currentState == State.move)
            {
                actionToDo.SetRequiredDestinationPlace(this, actions[0].location_name);
                if (Vector3.Distance(RequiredDestination.position, this.transform.position) < 2f)
                {
                    manageState();
                }
                else
                {
                    //Debug.Log("Name = " + this.name + "RequiredDestination = " + RequiredDestination.position);
                    mover.MoveTo(RequiredDestination.position);
                }
            }
            else if (currentState == State.resource)
            {
                //Debug.Log(RequiredDestination.position);
                //Debug.Log(this.transform.position);
                actionToDo.SetRequiredDestinationResource(this, actions[0]);
                if (Vector3.Distance(RequiredDestination.position, this.transform.position) < 2f)
                {
                    currentState = State.execute;
                }
                else
                {
                    //Debug.Log("Name = " + this.name + "RequiredDestination = " + RequiredDestination.position);
                    mover.MoveTo(RequiredDestination.position);
                }
            }
            else if (currentState == State.target)
            {
                actionToDo.SetRequiredDestinationTarget(this, actions[0].target_name);
                if (Vector3.Distance(RequiredDestination.position, this.transform.position) < 2f)
                {
                    currentState = State.execute;
                }
                else
                {
                    //Debug.Log("Name = " + this.name + "RequiredDestination = " + RequiredDestination.position);
                    mover.MoveTo(RequiredDestination.position);
                }
            }
            else if (currentState == State.execute)
            {
                mover.MoveTo(this.transform.position);
                if (finishedTask == false)
                {
                    actionToDo.Execute(this, actions[0]);
                }
                else
                {
                    actions.RemoveAt(0);
                    currentState = State.idle;
                    actionToDo = null;
                }
            }
        }

        // Check what is the next State for the NPC (refactorization).
        void    manageState()
        {
            if (actions[0].object_name.Length > 0)
            {
                currentState = State.resource;
            }
            else if (actions[0].target_name.Length > 0)
            {
                currentState = State.target;
            }
            else
            {
                currentState = State.execute;
            }
        }

        // Add the Action to the corresponding NPC from the CallServer.
        public void putData(NPCAction add)
        {
            actions.Add(add);
        }

        // Check Action to do depending of the action of the get in the request.
        public Action   findAction()
        {
            for (int i = 0; i < actionsAvailable.Length; i++)
            {
                if (actions[0].action == actionsAvailable[i].Name)
                {
                    return (actionsAvailable[i]);
                }
            }
            return (null);
        }

        #region Coroutine
        // There are the actions. Using a corouting to manage action one by one for each NPC.
        public void DoEat(int time)
        {
            StartCoroutine(EatCoroutine(time));
        }

        public void DoInspect(int time)
        {
            animator.SetTrigger("Press");
            StartCoroutine(InspectCoroutine(time));
        }

        public void DoMove(int time)
        {
            StartCoroutine(MoveCoroutine(time));
        }

        public void DoPlay(int time)
        {
            animator.SetTrigger("Press");
            StartCoroutine(PlayCoroutine(time));
        }

        public void DoRepair(int time)
        {
            animator.SetTrigger("Press");
            StartCoroutine(RepairCoroutine(time));
        }

        public void DoSay(int time, string content)
        {
            StartCoroutine(SayCoroutine(time, content));
        }

        IEnumerator EatCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            //Debug.Log("I ate food!");
            finishedTask = true;
        }

        IEnumerator InspectCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
                if (counter == 1)
                {
                    animator.SetTrigger("Unpress");
                }
            }
            //Debug.Log("I inspected!");
            finishedTask = true;
        }

        IEnumerator MoveCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            //Debug.Log("I moved!");
            finishedTask = true;
        }

        IEnumerator PlayCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
                if (counter == 1)
                {
                    animator.SetTrigger("Unpress");
                }
            }
            //Debug.Log("I played!");
            finishedTask = true;
        }

        IEnumerator RepairCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
                if (counter == 1)
                {
                    animator.SetTrigger("Unpress");
                }
            }
            //Debug.Log("I repaired!");
            finishedTask = true;
        }

        IEnumerator SayCoroutine(int time, string content)
        {
            int counter = time;

            canva.ShowSpeech(content);
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            canva.HideSpeech();
            //Debug.Log("I spoke!");
            finishedTask = true;
        }

        #endregion
    }
}

/* There is a way of improvement I've tried to use: use raycasting on NPC to dodge themselves but it was not really working
    public float            raycastDistance = 1.0f;
    public float rotationSpeed = 5.0f;
    public float movementSpeed = 1.0f;
    public LayerMask        obstacleLayer;

    void Start()
    {
        obstacleLayer = LayerMask.GetMask("Npc");
    }
    void Update()
    {
        if (RequiredDestination && Vector3.Distance(this.transform.position, RequiredDestination.position) > 10f && mover.agent.velocity.magnitude < 0.2f)
            RaycastForObstacles();
    }

    private void RaycastForObstacles()
    {
        Vector3 targetPosition = transform.position + transform.forward * raycastDistance;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            
            // Adjust rotation to avoid the obstacle
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // Calculate dodge position relative to obstacle hit point
            Vector3 dodgePosition = hit.point + hit.normal * raycastDistance;

            // Move to the dodge position
            mover.MoveTo(dodgePosition);
            Debug.Log("I try to dodge");
        }
    }
*/