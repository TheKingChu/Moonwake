using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyLoot : MonoBehaviour
{
    [Header("Select One")]
    [SerializeField] private bool isRandomDrop;
    public bool controlledRandom;
    [Space(20
    )]

    
    public List<Item> droppableItems;
    [Range(0,1)] public List<float> chanceToDrop;
    [SerializeField] private int repeatAmount;

    private List<int> itemsToDrop = new List<int>();

    private Inventory playerInventory;

    private Tutorial tutorial;

    private DayCycleManager dcm;
    private bool spawnedAtDay;

    public bool enemy;

    public int repeatMin;
    public int repeatMax;

    public List<int> guaranteed;

    void Start()
    {
        tutorial = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Tutorial>();
        
        playerInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        dcm = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<DayCycleManager>();

        if (enemy)
        {
            for (int i = 0; i < chanceToDrop.Count; i++)
            {
                chanceToDrop[i] = Random.Range(0.4f, 0.9f);
            }
            
            if (dcm.TimeOfDay >= 0.33f)
            {
                Destroy(gameObject);
            }
        }

        repeatAmount = Random.Range(repeatMin, repeatMax);


        if (isRandomDrop)
        {        
            for (int i = 0; i < repeatAmount; i++)
            {
                for (int j = 0; j < droppableItems.Count; j++)
                {
                    if (chanceToDrop[j] > Random.Range(0f, 1f))
                    {
                        itemsToDrop.Add(droppableItems[j].id);
                    }
                }
            }
        }


        if (controlledRandom)
        {
            for (int i = 0; i < droppableItems.Count; i++)
            {
                for (int j = 0; j < guaranteed[i]; j++)
                {
                    itemsToDrop.Add(droppableItems[i].id);
                }
            }
            
            for (int i = 0; i < repeatAmount; i++)
            {
                for (int j = 0; j < droppableItems.Count; j++)
                {
                    if (chanceToDrop[j] > Random.Range(0f, 1f))
                    {
                        itemsToDrop.Add(droppableItems[j].id);
                    }
                }
            }
        }
        
        
    }
    private void GiveItem()
    {
        for (int i = 0; i < itemsToDrop.Count; i++)
        {
            playerInventory.AddToInventoryByID(itemsToDrop[i], 1);
        }
        
        Destroy(gameObject);
    }
}
