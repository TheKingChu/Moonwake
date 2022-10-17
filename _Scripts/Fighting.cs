using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Fighting : MonoBehaviour
{
    private PlayerResources _playerResources;
    private Vector3 hitPosition;
    public EnemyHealth enemy;
    public ParticleSystem explosionParticle;
    public GameObject magicLaserParent, magicLaser, laser;
    public float lazerSpeed;
    int attDmg = 16;
    private float cooldown = 0.3f;
    float attTime;
    bool canAttack;
    
    //maybe this helps?
    public GameObject enemyObj;

    //sound stuff
    public AudioSource[] audioSources;

    void Start()
    {   
        _playerResources = GetComponent<PlayerResources>();
    }

    void Update()
    {
        if(laser)
        {
            laser.transform.position += (hitPosition - transform.position).normalized * lazerSpeed;
        }

        attTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && attTime < 0f)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 5, 1 << 10))
            {
                //Debug.Log(hit.distance);
                
                if (hit.transform.CompareTag("Enemy"))
                {
                    Instantiate(explosionParticle, hit.point, rotation: (Quaternion)Quaternion.identity);
                    
                    laser = Instantiate(magicLaser, magicLaserParent.transform.position, magicLaserParent.transform.rotation);//spawns in worldspace unparented
                    hitPosition = hit.collider.transform.position;
                    //magicLaser.transform.position = hit.point;

                    attTime = cooldown;
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage();

                    if (audioSources[0] != null)
                    {
                        audioSources[0].Play();
                    }
                    //GameObject.Destroy(hit.collider.gameObject);
                    //enemy.MotherAttacking();

                }
                else if (hit.transform.CompareTag("Weak"))
                {
                    Instantiate(explosionParticle, hit.point, rotation: (Quaternion)Quaternion.identity);
                    
                    laser = Instantiate(magicLaser, magicLaserParent.transform.position, magicLaserParent.transform.rotation);//spawns in worldspace unparented
                    hitPosition = hit.collider.transform.position;
                    //magicLaser.transform.position = hit.point;

                    attTime = cooldown;
                    
                    hit.collider.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage();

                    if (audioSources[0] != null)
                    {
                        audioSources[0].Play();

                    }
                }
            }
        }
    }
}