using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    MachineUpdate machineUpdate;

    //Slime, Deerty, Motherwhisp(player), miniwhisp(whisp), machine sounds
    public AudioSource[] s_Sources, d_Sources, p_Sources, w_Sources, m_Sources;

    //Sound bools
    bool toggleChange, mPlay;

    //for knowing movement
    bool up;
    bool down;
    bool left;
    bool right;

    private void Start()
    {
        mPlay = true;

        up = Input.GetKeyDown(KeyCode.W);
        down = Input.GetKeyDown(KeyCode.S);
        left = Input.GetKeyDown(KeyCode.A);
        right = Input.GetKeyDown(KeyCode.D);
    }

    //Sounds for enemies
    public void AttackSound()
    {
        if (gameObject.name == "TestingEnemy_Rune" || gameObject.name == "TestingEnemy_Rune" + "(Clone)")
        {
            s_Sources[0].Play();
        }

        else if (gameObject.name == "Deerty_Enemy_pivot" || gameObject.name == "Deerty_Enemy_pivot" + "(Clone)")
        {
            d_Sources[0].Play();
        }
    }

    public void MovementSound()
    {
        if (gameObject.name == "TestingEnemy_Rune" || gameObject.name == "TestingEnemy_Rune" + "(Clone)")
        {
            s_Sources[1].Play();
        }

        else if (gameObject.name == "Deerty_Enemy_pivot" || gameObject.name == "Deerty_Enemy_pivot" + "(Clone)")
        {
            d_Sources[1].Play();
        }
    }

    public void DeathSound()
    {
        if (gameObject.name == "TestingEnemy_Rune" || gameObject.name == "TestingEnemy_Rune" + "(Clone)")
        {
            s_Sources[2].Play();
        }

        else if (gameObject.name == "Deerty_Enemy_pivot" || gameObject.name == "Deerty_Enemy_pivot" + "(Clone)")
        {
            d_Sources[2].Play();
        }
    }

    //Sounds for player
    public void PlayerAttackingS()
    {
        if (gameObject.CompareTag("Player"))
        {
            p_Sources[1].Play();
        }
        else
        {
            p_Sources[1].Stop();
        }
    }

    /*public void PlayerIdleS()
    {
        if (gameObject.CompareTag("Player") && up || down || left || right)
        {
            //Check to see if you just set the toggle to positive
            if (mPlay == true && toggleChange == true)
            {
                //Play the audio you attach to the AudioSource component
                p_Sources[0].Play();
                //Ensure audio doesn�t play more than once
                toggleChange = false;
            }

            //Check if you just set the toggle to false
            if (mPlay == false && toggleChange == true)
            {
                p_Sources[0].mute = true;
                
                toggleChange = false;
            }
        }
    }*/

    public void PlayerMovementS()
    {
        if (gameObject.CompareTag("Player"))
        {
            p_Sources[2].Play();
        }
    }

    public void PlayerPickupS()
    {
        if (gameObject.CompareTag("Player") && gameObject.CompareTag("Enemyloot"))
        {
            p_Sources[1].Play();
        }
        else
        {
            p_Sources[1].Stop();
        }
    }

    //public void PlayerHarvestS()

    //Player sound for intro scene
    public void PlayerIntroS()
    {
        if(gameObject.CompareTag("Player"))
        {
            p_Sources[0].Play();
            p_Sources[1].Play();
        }
    }


    //Sounds miniwhisps
    public void WhispIdleS()
    {
        
    }

    //Sounds for machine
    //This is the part that needs to be changed, make sure it doesnt start playing before the whisp is on and working
    public void MachineWorkingS()
    {
        if (gameObject.CompareTag("Machine") && machineUpdate.hasWorker == true)
        {
            m_Sources[0].Play();
            machineUpdate.hasWorker = false;
        }
        else
        {
            m_Sources[0].Stop();
        }
    }

}
