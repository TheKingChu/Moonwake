using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DeertyAI : MonoBehaviour
{
    private GameManager gm;
    private MonoDatabase mdb;

    private NavMeshAgent agent;
    private Transform mother;
    private Transform target;
    public float sightRange, attackRange;
    private Vector3 destination;

    [SerializeField] private GameObject lootbag;

    [SerializeField] private GameObject attackSphere;

    public int damage;
    public Animator anim;
    private DayCycleManager dcm;

    private bool hasAttacked;

    private Vector3 attackStartPoint;

    private bool canAttack;
    
    public float runSpeed;
    
    public enum EnemyStates
    {
        Chasing,
        Turning,
        Attacking,
        CheckAttack
    }

    public EnemyStates _currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        _currentState = EnemyStates.Chasing;
    }

    public void SetDependencies(GameManager gm, DayCycleManager dcm, MonoDatabase mdb, Transform mother)
    {
        this.gm = gm;
        this.dcm = dcm;
        this.mdb = mdb;
        this.mother = mother;

        target = this.mother;
    }

    private void FindNewTarget()
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
                target = gm.machines[(int)tempCounter].transform;
            }
        }

        target = mother;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);

        Debug.Log(canAttack);
        
        if (_currentState == EnemyStates.Chasing)
        {
            agent.isStopped = false;
            
            agent.SetDestination(target.position);

            if (Vector3.Distance(transform.position, target.position) < attackRange - 1)
            {
                //agent.isStopped = true;
                
                _currentState = EnemyStates.CheckAttack; 
                
                return;
            }
        }

        if (_currentState == EnemyStates.CheckAttack)
        {
            //agent.SetDestination(transform.position);

            
            if (angle > 3f)
            {
                _currentState = EnemyStates.Turning;

                return;
            }
            

            if (angle < 3f)
            {
                anim.SetTrigger("getready");
            
                Invoke(nameof(StartAttack), 1f);
                
                _currentState = EnemyStates.Attacking;
            }
            else if (distance > attackRange + 1f)
            {
                _currentState = EnemyStates.Chasing;
            }
        }
        
        if (_currentState == EnemyStates.Turning)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 10f * Time.fixedDeltaTime);

            if (angle < 2.5f)
                _currentState = EnemyStates.CheckAttack;

            if (distance > attackRange + 1f)
                _currentState = EnemyStates.Chasing;
        }
        

        if (_currentState == EnemyStates.Attacking)
        {
            agent.isStopped = true;

            if (canAttack)
            {
                transform.position += transform.forward * runSpeed * Time.fixedDeltaTime;

                Debug.Log("Distance from start: " + Vector3.Distance(transform.position, attackStartPoint));
                
                if (Vector3.Distance(transform.position, attackStartPoint) > 21f)
                {
                    //_currentState = EnemyStates.Turning;

                    canAttack = false;
                    
                    _currentState = EnemyStates.CheckAttack;
                }
            }

        }
    }
    
    
    private void StartAttack()
    {
        canAttack = true;
        
        attackStartPoint = transform.position;
        
        anim.SetTrigger("attack");
    }
    
    private void ResetAttack()
    {
        attackSphere.SetActive(true);
        hasAttacked = false;
    }

    private void ResetAttackAnimation()
    {
        attackSphere.SetActive(false);
        anim.SetBool("isAttacking", false);
    }

    public void DropLootbag()
    {
        Instantiate(lootbag, transform.position, Quaternion.Euler(Vector3.up), gm.gameObject.transform);
    }
    
    /*

    attack
    getready
    attack 2xspeed
    idle
    attack

    */

    private void FindNewDestination()
    {
        RaycastHit hit;

        Vector2 rand = Random.insideUnitCircle * 10f;

        if (Physics.Raycast(new Vector3(transform.position.x + rand.x, transform.position.y + 10f, transform.position.z + rand.y), Vector3.down,  out hit))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                destination = hit.point;
            }
            else
            {
                FindNewDestination();
            }
        }
    }
}