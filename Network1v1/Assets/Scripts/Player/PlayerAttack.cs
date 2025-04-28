using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    const int slamDamage = 8;

    const int lightUpper1Damage = 7;
    const int lightUpper2Damage = 10;
    const int lightUpper3Damage = 15;

    const float lightUpperAnimationTime = 0.5f; //how long it takes for light upper animation to play
    const float lightUpperComboTime = 0.3f; //how long after light upper finishing can the combo be triggered
    const int maxLightUpperCombo = 3;

    public bool canAttack;

    private Animator animator;

    public int damageMultipler;

    [SerializeField] public bool slamRunning;

    [SerializeField] private bool lightUpperRunning;
    [SerializeField] private int lightUpperComboNumber;

    private void Awake()
    {
        enabled = false; //this will only be enabled by the owning player

        canAttack = true;

        //get animator
        animator = GetComponentInChildren<Animator>();

        slamRunning = false;

        lightUpperRunning = false;
        lightUpperComboNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            if (Input.GetMouseButtonDown(0) && !slamRunning && !lightUpperRunning) //if not attacking and get lmb then attack
            {
                HitCollider hitCollider = GetComponentInChildren<HitCollider>();

                if (GetComponent<PlayerMovement>().onGround) //if on ground run light upper
                {
                    StartCoroutine("WaitForLightUpperCombo");

                    switch (lightUpperComboNumber) //set damage for specific light upper combo
                    {
                        case 2:
                            hitCollider.SetDamageServerRpc(10);
                            break;
                        case 3:
                            hitCollider.SetDamageServerRpc(15);
                            break;
                        default:
                            hitCollider.SetDamageServerRpc(7);
                            break;
                    }

                    LightUpperAttackServerRpc(lightUpperComboNumber); //run light upper attack
                }
                else //otherwise run slam attack
                {
                    hitCollider.SetDamageServerRpc(slamDamage); //set damage for slam attack
                    SlamAttackServerRpc(true); //start slam attack
                }
            }
        }
    }

    private IEnumerator WaitForLightUpperCombo()
    {
        lightUpperRunning = true;

        lightUpperComboNumber++;

        if(lightUpperComboNumber > maxLightUpperCombo)
        {
            //if combo has been completed it is set back to the first attack of the combo
            lightUpperComboNumber = 1;
        }

        //store combo current state
        int currentComboNumber = lightUpperComboNumber;

        //wait for duration of animation to run then stop light upper running
        yield return new WaitForSeconds(lightUpperAnimationTime);
        lightUpperRunning = false;


        //wait for the max time between combos
        yield return new WaitForSeconds(lightUpperComboTime);

        //if combo state has stayed the same then light upper hasn't been pressed
        //so reset the combo
        if(currentComboNumber == lightUpperComboNumber)
        {
            lightUpperComboNumber = 0;
        }
    }


    [Rpc(SendTo.Server)]
    private void LightUpperAttackServerRpc(int comboNumber)
    {
        switch (comboNumber)
        {
            case 2:
                animator.SetTrigger("Light Upper 2");
                break;
            case 3:
                animator.SetTrigger("Light Upper 3");
                break;
            default:
                animator.SetTrigger("Light Upper 1");
                break;
        }
    }

    public void StopSlamAttack()
    {
        SlamAttackServerRpc(false);
    }

    [Rpc(SendTo.Everyone)]
    private void SlamAttackServerRpc(bool running)
    {
        slamRunning = running;
        animator.SetBool("Plunge", running);
    }
}
