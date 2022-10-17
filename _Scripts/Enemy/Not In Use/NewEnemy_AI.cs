using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemy_AI : MonoBehaviour
{
    public Transform mother, enemyHead;
    private NavMeshAgent enemy;
    private float distMother;
    private float chaseDist = 10, attackDist = 3, FOVangle = 30f, wpAccuracy = 2f;
    private float wayPointScaler;
    private int currWP;

    private Vector3[] wayPoints = new Vector3[]
    {
        new Vector3(0, 0, 10),
        new Vector3(7.5f, 0, 7.5f),
        new Vector3(10, 0, 0),
        new Vector3(7.5f, 0, -7.5f),
        new Vector3(0, 0, -10),
        new Vector3(-7.5f, 0, -7.5f),
        new Vector3(-10, 0, 0),
        new Vector3(-7.5f, 0, 7.5f)
    };

    //statemachine stuff
    public EnemyStates enemyStates;
    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Fighting
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i].x = (wayPoints[i].x * wayPointScaler) + transform.position.x;
            wayPoints[i].z = (wayPoints[i].z * wayPointScaler) + transform.position.z;

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(wayPoints[i].x, 300f, wayPoints[i].z), Vector3.down, out hit, 500f))
            {
                wayPoints[i].y = hit.point.y;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Set the direction vector between player and enemy
        Vector3 direction = transform.position - mother.position;
        distMother = Vector3.Distance(transform.position, mother.position);
        //direction.y = 0f;
        Debug.Log(direction.normalized);

        switch (enemyStates)
        {
            case EnemyStates.Patrolling:
                if ((distMother < chaseDist) && (Vector3.Angle(direction, enemyHead.up) < FOVangle))
                {
                    enemyStates = EnemyStates.Chasing;
                }
                break;

            case EnemyStates.Chasing:
                //This is what makes the enemy move towards the mother
                transform.LookAt(mother);
                transform.position += transform.forward * Time.deltaTime;
                //if the player is far enough away the enemy starts patrolling again
                if (Vector3.Distance(transform.position, mother.position) > 15)
                {
                    //Makes sure that the patrolling start so that it will patrol correctly
                    enemy.isStopped = false;
                    enemyStates = EnemyStates.Patrolling;
                }
                //but if the enemy is close enough to the player it will then attack
                else if (Vector3.Distance(transform.position, mother.position) < attackDist)
                {
                    enemyStates = EnemyStates.Fighting;
                }
                break;
            case EnemyStates.Fighting:
                break;
        }
    }
}
