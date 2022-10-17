using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    //refrence to the enemy base class
    public Enemy_Base enemy_Base;

    //for attacking player/motherwhisp
    public Transform mother; //enemy target
    public float moveSpeed; //the move speed obvs
    public float rotationSpeed; //the speed of turning
    public float attackDistance = 3; //the distance for the 
    public float attackTimer = 1; //delay between attacks when within range

    //statemachine stuff
    public EnemyStates enemyStates;
    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Fighting
    }

    //navmesh stuff
    private NavMeshAgent enemy;
    public float radius;
    
    //for dropping the items when killed
    public bool isDropped;
    public GameObject bag;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        mother = GameObject.FindGameObjectWithTag("MotherWhisp").transform; //target the player
        isDropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyStates)
        {
            //Makes the enemy walk to random points
            case EnemyStates.Patrolling:
                if (!enemy.hasPath)
                {
                    enemy.SetDestination(Enemy_Point.Instance.GetRandomPoint(transform, radius));
                }
                if (Vector3.Distance(transform.position, mother.position) < 10)
                {
                    //Makes sure that the patrolling stops so that it will follow the player instead
                    enemy.isStopped = true;
                    enemyStates = EnemyStates.Chasing;
                }
                Debug.LogWarning("isPatrolling");
                break;

            //Makes the enemy target the mother and follows
            case EnemyStates.Chasing:
                //This is what makes the enemy move towards the mother
                transform.LookAt(mother);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                //if the player is far enough away the enemy starts patrolling again
                if (Vector3.Distance(transform.position, mother.position) > 15)
                {
                    //Makes sure that the patrolling start so that it will patrol correctly
                    enemy.isStopped = false;
                    enemyStates = EnemyStates.Patrolling;
                }
                //but if the enemy is close enough to the player it will then attack
                else if (Vector3.Distance(transform.position, mother.position) < attackDistance)
                {
                    enemyStates = EnemyStates.Fighting;
                }
                Debug.LogWarning("isChasing");
                break;

            //Makes the enemy attack the player
            case EnemyStates.Fighting:
                //Makes sure that that the enemy is still moving towards the player
                transform.LookAt(mother);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;



                //if the player is out of attack range the enemy chases again
                if (Vector3.Distance(transform.position, mother.position) > attackDistance)
                {
                    enemyStates = EnemyStates.Chasing;
                }
                Debug.LogWarning("isFighting");
                break;
        }

        if (enemy_Base.Health <= 0)
        {
            Death();
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
    void Death()
    {
        int random = Random.Range(1, enemy_Base.items.Length);

        if (isDropped == false)
        {
            Instantiate(bag, transform.position, Quaternion.Euler(Vector3.up));
            isDropped = true;
        }
    }
}
