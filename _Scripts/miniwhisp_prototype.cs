using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class miniwhisp_prototype : MonoBehaviour
{
    public Animator anim;
    public Transform miniWhisp;
    NavMeshAgent agent;
    public Transform target; //Where miniwhisp is to go to
    private float distanceToTarget;

        void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    public enum State

    {
        Working, Hauling,
        Waiting, //for when the action is done and they cant work anymore
        NotWorking, //when its not in use for a machine
    }

    public State currentState;

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Working:
                IsWorking();
                anim.SetBool("IsWorking", true);
                break;
            case State.Hauling:
                IsHauling();
                anim.SetBool("IsHauling", true);
                break;
            case State.Waiting:
                IsWaiting();
                anim.SetBool("IsWaiting", true);
                break;
            case State.NotWorking:
                IsNotWorking();
                anim.SetBool("IsNotWorking", true);
                break;
        }
    }

    void Update()
    {

    }

    private void IsWorking()
    {
        if(transform.parent.name == "WaterCollector")//checks parent name hopefully
        {
            anim.Play("PumpWork");
        }
    }

    private void IsHauling()
    {
        agent.SetDestination(target.position);//sends agent to target
    }

    private void IsWaiting()
    {

    }

    private void IsNotWorking()
    {

    }
}
