using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingEarth : MonoBehaviour
{
    //water

    //earth

    //other stuff
    public float currentEHealth, health2, health3; //earth health

    Renderer rend;

    public Material terrainM;

    void Start()
    {
        //terrainM = gameObject.GetComponent<Renderer>().material;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //TerrainMLerp(); //this is what you call to add do terrain health
        }
    }
    */

        /*void TerrainMLerp()
    {
            currentEHealth = currentEHealth + 0.2f;
        if(currentEHealth <= 1)
=======
    void TerrainMLerp()
    {
        currentEHealth = currentEHealth + 0.2f;

        if (currentEHealth <= 1)
>>>>>>> Stashed changes
        {
            terrainM.SetFloat("DeadTohealthy1Slide_", currentEHealth);
        }

        if (currentEHealth >= 1 && currentEHealth < 2.2f)
        {
            terrainM.SetFloat("Healthy1ToHealthy2Slide_", health2);
            health2 = health2 + 0.2f;
        }

        if (currentEHealth >= 2 && currentEHealth <= 3.2f)
        {
            terrainM.SetFloat("Healthy2ToHealthy3Slide_", health3);
            health3 = health3 + 0.2f;
        }
<<<<<<< Updated upstream
        
    }*/
}


