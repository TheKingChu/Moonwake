using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "EnemyType")]

public class Enemy_Base : ScriptableObject
{
    public float Health;
    public float maxHealth = 100f;

    //this is for the random dmg the enemy does
    public int minDmg = 1;
    public int maxDmg = 15;
    public float cooldown = 1;

    //for when the enemy is roaming vs. when it goes to attack
    public float walkingSpeed;
    public float chasingSpeed;

    public Item[] items;
}
