using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;



public class Worker : Character
{
    public WorkerState state;

    //AI
    NavMeshAgent agent;

    //Find Target
    public float checkRadius;
    public float activationDistance;
    public LayerMask interactableLayer;
    public Transform closeObject;

    //오브젝트 상호작용 거리
    public float interactionDistance = 3.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        base.Start();
        UI_CompassBar.AddMarker(transform, "Worker");
    }

    private void Update()
    {
        
        if (state == WorkerState.Move)
        {
            if (closeObject == null)
            {
                StateChange(WorkerState.Idle);
            }
        }
        else if (state == WorkerState.Interaction)
        {
            if(closeObject == null)
            {
                StateChange(WorkerState.Idle);
            }
        }
    }


    IEnumerator LookAtTarget()
    {
        
        yield return new WaitForSeconds(1.0f);
        while(closeObject == null)
        {
            FindClosetObject();
            yield return null;
        }
        
        Vector3 destination = new Vector3(closeObject.position.x, transform.position.y, closeObject.position.z);
        SetDestination(destination, ()=> StateChange(WorkerState.Arrived));
        Debug.Log("SetDestination" + destination);
        yield return new WaitForSeconds(0.02f);
        StateChange(WorkerState.Move);
        animator.SetFloat("Speed", 1.0f);
    }

    public void SetDestination(Vector3 pos, Action action)
    {
        agent.SetDestination(pos);
        animator.SetFloat("Speed", 1.0f);
        StartCoroutine(DestinationCoroutine(action));
    }

    IEnumerator DestinationCoroutine(Action action)
    {
        yield return new WaitForSeconds(0.02f);
        while(agent.pathPending)
        {
            yield return null;
        }
        while(agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        action?.Invoke();
    }


    public void StateChange(WorkerState stateValue)
    {
        if (animator == null) Debug.Log("animator is null");
        state = stateValue;
        switch(state)
        {
            case WorkerState.Idle:
                Debug.Log("State Change = Idle");
                animator.SetFloat("Speed", 0.0f);
                animator.SetBool("Interaction", false);
                StopAllCoroutines();
                agent.stoppingDistance = 3.0f;
                DeactiveEquipment();
                StartCoroutine(LookAtTarget());
                break;
            case WorkerState.Move:
                Debug.Log("State Change = Move");
                break;
            case WorkerState.Arrived:
                Debug.Log("State Change = Arrived");
                M_Object subObject = null;
                if (closeObject == null) StateChange(WorkerState.Idle);
                if (closeObject.GetComponent<M_Object>() == null)
                {
                    subObject = closeObject.transform.parent.GetComponent<M_Object>();
                }
                else subObject = closeObject.GetComponent<M_Object>();
                subObject.Interaction(GetComponent<Character>());

                animator.SetBool("Interaction", true);
                animator.SetFloat("Speed", 0.0f);
                transform.LookAt(closeObject.transform);
                if (agent.remainingDistance <= interactionDistance)
                {
                    StateChange(WorkerState.Interaction);
                }
                
                break;
            case WorkerState.Interaction:
                Debug.Log("State Change = Interaction");
                break;


        }
    }

   

    private void FindClosetObject()
    {
        Debug.Log("Call Find Object");
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, checkRadius, interactableLayer);
        closeObject = null;
        float closeDistance = Mathf.Infinity;
        foreach (Collider obj in nearbyObjects)
        {
            if (obj.GetComponent<Interaction_Hit>() != null)
            {
                Transform targetTransform = obj.transform;

                float distance = Vector3.Distance(transform.position, targetTransform.position);

                if (distance <= activationDistance && distance < closeDistance)
                {

                    closeObject = targetTransform;
                    closeDistance = distance;

                }
            }
        }
    }
}
