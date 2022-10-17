using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject deerPrefab;
    
    private int numberOfEnemies;

    [SerializeField] private int spawnTimeFrom;
    [SerializeField] private int spawnTimeTo;

    private int xLower = 0;
    private int xHigher = 40;
    private int zLower = -57;
    private int zHigher = -32;

    private Vector3 xzPosition;
    private Vector3 spawnPosition;

    private int enemySpawnTime = 1;
    private float timer;

    private DayCycleManager dcm;
    private bool night;

    private Tutorial tutorial;

    private Transform motherwhisp;

    private GameManager gm;

    private MonoDatabase mdb;

    private void Start()
    {
        tutorial = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Tutorial>();

        dcm = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<DayCycleManager>();

        motherwhisp = GameObject.FindGameObjectWithTag("MotherWhisp").GetComponent<Transform>();

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mdb = gm.GetComponent<MonoDatabase>();
        
        InvokeRepeating(nameof(CheckEnemyAmount), 1f, 1f);
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        CheckNight();

        if (night)
        {
            if (timer > enemySpawnTime)
            {
                if (numberOfEnemies < 1)
                {
                    FindSpawnPoint();

                    enemySpawnTime = Random.Range(spawnTimeFrom, spawnTimeTo);
                    timer = 0;
                }
            }
        }
    }

    void CheckEnemyAmount()
    {
        numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void FindSpawnPoint()
    {
        xzPosition = new Vector3(Random.Range(xLower, xHigher), 40, Random.Range(zLower, zHigher));

        RaycastHit hit;
        Physics.Raycast(xzPosition, Vector3.down, out hit, 100f,1<<8);
        if (hit.collider.name == "Terrain")
        {
            //Debug.Log("Found Terrain");
            spawnPosition = hit.point;

            SpawnEnemy(spawnPosition);
        }
        else
        {
            FindSpawnPoint();
        }
    }

    void CheckNight()
    {
        if (dcm.TimeOfDay < 0.33f)
        {
            night = true;
        }
        else
        {
            night = false;
        }
    }

    void SpawnEnemy(Vector3 spawnPosition)
    {
        GameObject temp = Instantiate(deerPrefab, spawnPosition, Quaternion.identity);

        temp.GetComponent<DeertyAI>().SetDependencies(gm, dcm, mdb, motherwhisp);

        numberOfEnemies++;
        /*
        if (dcm.day > 1)
        {
            if (Random.Range(0f, 1f) > 0.65f)
            {
                GameObject temp = Instantiate(deerPrefab, spawnPosition, Quaternion.identity);

                temp.GetComponent<DeertyAI>().SetDependencies(gm, dcm, mdb, motherwhisp);

                numberOfEnemies++;
            }
            else
            {
                GameObject temp = Instantiate(slimePrefab, spawnPosition, Quaternion.identity);

                temp.GetComponent<RuneEnemyTry>().SetDependencies(gm, dcm, mdb, motherwhisp);

                numberOfEnemies++;
            }
        }
        else
        {
            GameObject temp = Instantiate(slimePrefab, spawnPosition, Quaternion.identity);

            temp.GetComponent<RuneEnemyTry>().SetDependencies(gm, dcm, mdb, motherwhisp);

            numberOfEnemies++;
        }
        
    */
    }

    public void ReduceEnemyAmount()
    {
        numberOfEnemies--;
    }
}