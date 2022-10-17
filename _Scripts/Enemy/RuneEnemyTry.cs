using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RuneEnemyTry : MonoBehaviour
{
    private GameManager gm;
    private MonoDatabase mdb;

    private NavMeshAgent agent;
    [SerializeField] private Transform mother;
    [SerializeField] private Transform target;
    public float sightRange, attackRange, fov;
    [SerializeField] private Vector3 destination;

    [SerializeField] private GameObject lootbag;

    [SerializeField] private GameObject attackSphere;

    public int damage;
    private Animator anim;
    private DayCycleManager dcm;

    private bool hasAttacked;

    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public EnemyStates _currentState;

    private void Start()
    {
        /*
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mother = GameObject.FindGameObjectWithTag("MotherWhisp").GetComponent<Transform>();
        dcm = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<DayCycleManager>();

        mdb = gm.gameObject.GetComponent<MonoDatabase>();
        */
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        destination = transform.position;
        FindNewTarget();

        _currentState = EnemyStates.Patrolling;
    }

    public void SetDependencies(GameManager gm, DayCycleManager dcm, MonoDatabase mdb, Transform mother)
    {
        this.gm = gm;
        this.dcm = dcm;
        this.mdb = mdb;
        this.mother = mother;
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

        if (target == null)
        {
            target = mother;
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            FindNewTarget();
        }

        float distance = Vector3.Distance(transform.position, target.position);
        float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);


        if (_currentState == EnemyStates.Patrolling)
        {
            agent.SetDestination(destination);

            if (agent.hasPath)
            {
                if (agent.remainingDistance > 30f)
                {
                    FindNewDestination();
                }
            }

            if (Vector3.Distance(transform.position, destination) < attackRange)
            {
                FindNewDestination();

                return;
            }

            if (distance < sightRange && angle < fov)
            {
                agent.SetDestination(target.transform.position);
                _currentState = EnemyStates.Chasing;
            }
        }

        if (_currentState == EnemyStates.Chasing)
        {
            agent.SetDestination(target.transform.position);

            if (distance < attackRange)
            {
                agent.SetDestination(transform.position);
                _currentState = EnemyStates.Attacking;
            }
            else if (distance > sightRange || angle > fov)
            {
                FindNewDestination();
                _currentState = EnemyStates.Patrolling;
            }
        }

        if (_currentState == EnemyStates.Attacking)
        {
            agent.SetDestination(transform.position);
            
            if (angle > 10)
            {
                Vector3 cross = Vector3.Cross(transform.forward, target.transform.position);
                if (cross.y < 0)
                    transform.RotateAround(transform.position, transform.up, -60f * Time.fixedDeltaTime);
                else
                    transform.RotateAround(transform.position, transform.up, 60f * Time.fixedDeltaTime);
            }

            if (distance <= attackRange)
            {
                if (!hasAttacked)
                {
                    anim.SetBool("isAttacking", true);

                    hasAttacked = true;

                    Invoke(nameof(ResetAttackAnimation), 2.5f);
                    Invoke(nameof(ResetAttack), 2.65f);
                }
            }
            else
            {
                if (target != mother)
                {
                    FindNewDestination();    
                }
                _currentState = EnemyStates.Chasing;
            }
        }
    }

    public void SwitchToMother()
    {
        target = mother;

        _currentState = EnemyStates.Attacking;
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