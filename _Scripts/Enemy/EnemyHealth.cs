using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{
    private RuneEnemyTry Enemy;
    private Fighting mother;

    //Healthbar stuff
    public float currentHealth, maxHealth;
    public GameObject healthBarUI;
    public Slider slider;
    public int dmgTaken = 10;

    //For dropping the items when killed
    public bool isDropped;
    //public GameObject bag;

    //animation stuff
    int isAttacking;
    Animator anim;
    public Animation heartAnim;

    public GameObject hitParticle;

    private DayCycleManager dcm;

    private float dyingTimer;
    
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        currentHealth = maxHealth;
        slider.value = CalculateHealthBar();

        Enemy = GetComponent<RuneEnemyTry>();
        
        heartAnim["Take 001"].speed = 0;
        heartAnim.Play("Take 001");

        dcm = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<DayCycleManager>();
        dyingTimer = Time.time;
    }

    private void FixedUpdate()
    {
        slider.value = CalculateHealthBar();

        
        
        if (dcm.TimeOfDay > 0.33f && Time.time > dyingTimer)
        {
            TakeDamage();
            dyingTimer = Time.time + Random.Range(0.3f, 1.2f);
        }
        /*
        if(currentHealth <= 0)
        {
            Death();
        }
        */
    }

    public void TakeDamage()
    {
        currentHealth -= UnityEngine.Random.Range(Mathf.FloorToInt(dmgTaken * 0.5f), Mathf.FloorToInt(dmgTaken * 1.5f));
 
        if (hitParticle != null)
            hitParticle.SetActive(true);
        
        Enemy.SwitchToMother();
        
        SetHeartAnimAmount();
        
        if (currentHealth <= 0)
        {
            Enemy.DropLootbag();
            GetComponent<Collider>().isTrigger = true;
            Invoke(nameof(DestroyGO), 0.05f);
            
        }
    }
    
    private void SetHeartAnimAmount()
    {
        heartAnim["Take 001"].time = CalculateHealth();
    }
    
    float CalculateHealth()
    {
        var temp = 0.036 * currentHealth;

        var temp2 = 3.625f - temp;
        
        return (float)temp2;
    }

    //new healthstuff to try
    /*
    public void MotherAttacking()
    {
        if (gameObject.CompareTag("MotherWhispAttackSphere"))
        {
            currentHealth -= dmgTaken;
            anim.SetBool(isAttacking, true);
        }
        else
        {
            anim.SetBool(isAttacking, false);
        }
    }
    */

    float CalculateHealthBar()
    {
        return currentHealth / maxHealth;
    }

    private void DestroyGO()
    {
        Destroy(gameObject);
    }
}
