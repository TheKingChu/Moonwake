using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class USEABLE_Enemy_AI : MonoBehaviour
{
    // TODO REMOVE LATER
    public Inventory inventory;
    private MonoDatabase monoDb;
    
    public NavMeshAgent enemy;
    public Transform mother;
    public LayerMask layerGround, layerMother;

    //For patrolling
    public Vector3 walkpoint;
    bool isWalkpointSet;
    public float walkpointRange;

    //For attacking
    public float cooldownAttack;
    bool hasAttacked;
    public int dmg;

    //For the states
    public float sightRange, attackRange;
    public bool targetInSight, targetAttRange;

    [SerializeField] private GameObject attackSphere;

    private Animator animator;
    
    
    // Runes stuff
    public Transform target;
    private GameManager gm;
    private bool targetMother;
    
    private void Awake()
    {
        // TODO REMOVE LATER
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        monoDb = gm.GetComponent<MonoDatabase>();


        //mother = GameObject.FindGameObjectWithTag("MotherWhisp").transform;
        enemy = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        
    }


    private void ChangeTarget()
    {
        if (gm.machines.Count != 0)
        {
            float tempDistance = Mathf.Infinity;
            float tempCounter = 0;
            // Rune stuff
            for (int i = 0; i < gm.machines.Count; i++)
            {
                float x = Vector3.Distance(transform.position, gm.machines[i].transform.position);
            
                if (x < tempDistance)
                {
                    tempDistance = x;
                    tempCounter = i;
                }
            
                if (i == gm.machines.Count - 1)
                {
                    target = gm.machines[i].transform;
                }
            }
        }
        else
        {
            target = mother;
        }
    }
    
    public void SwitchToMother()
    {
        target = mother;
    }
    
    private void FixedUpdate()
    {
        //checks if the mother is in sight and for if its in attack range
        if (target == null)
        {
            ChangeTarget();
        }
        
        targetInSight = Physics.CheckSphere(transform.position, sightRange, layerMother);
        targetAttRange = Physics.CheckSphere(transform.position, attackRange, layerMother);

        if (!targetInSight && !targetAttRange) Patrolling();
        if (targetInSight && !targetAttRange) Chasing();
        if (targetAttRange && targetInSight) Attacking();
    }

    private void Patrolling()
    {
        if (!isWalkpointSet) SearchWalkpoint();

        if (isWalkpointSet)
        {
            enemy.SetDestination(walkpoint);
        }

        float distanceToPoint = Vector3.Distance(transform.position,walkpoint);
        //If the enemy has reached the walkpoint
        if(distanceToPoint < 1f)
        {
            isWalkpointSet = false; //it will then automatically search for a new point
        }
    }

    private void SearchWalkpoint()
    {
                
        // Changed because this didn't necessarily return a valid destination
        
        /*
        //This calculates random points that are in range of the enemy
        float randomZ = Random.Range(-walkpointRange, walkpointRange);
        float randomX = Random.Range(-walkpointRange, walkpointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkpoint, -transform.up, 2f, layerGround))
        {
            isWalkpointSet = true;
        }
        */

        
        walkpoint.x = 0;

        while (walkpoint.x == 0)
        {
            RaycastHit hit;

            Vector2 rand = Random.insideUnitCircle * 10f;
            
            if(Physics.Raycast(new Vector3(transform.position.x + rand.x, transform.position.y + 10f, transform.position.z + rand.y), Vector3.down,  out hit))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    walkpoint = hit.point;
                }
            }
        }
    }

    private void Chasing()
    {
        //Debug.Log("Chasing");
        enemy.SetDestination(target.position);
    }

    private void Attacking()
    {
        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            enemy.SetDestination(target.position);
        }
        else
        {
            //This makes sure that the enemy doesnt move when attacking
            enemy.SetDestination(transform.position);
            
            if (!hasAttacked/* && CompareTag("Machine")*/)
            {
                animator.SetBool("isAttacking", true);
                Invoke(nameof(ResetAttackAnimation), 2.5f);
            
                //Invoke(nameof(ResetAttackSphere), 0.1f);

                hasAttacked = true;
                //Invoke(nameof(ResetAttack), cooldownAttack);
                Invoke(nameof(ResetAttack), 2.8f);

                float angle = Vector3.Angle(transform.position, target.position);

                if (angle > 180)
                {
                    transform.RotateAround(transform.position, Vector3.up, 40f * Time.deltaTime);
                }
                else
                {
                    transform.RotateAround(transform.position, Vector3.up, -40f * Time.deltaTime);
                }
            }
        }
    }

    private void ResetAttackSphere()
    {
        
        attackSphere.SetActive(false);
    }

    private void ResetAttack()
    {
        attackSphere.SetActive(false);
        hasAttacked = false;
    }

    private void ResetAttackAnimation()
    {
        attackSphere.SetActive(true);
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
