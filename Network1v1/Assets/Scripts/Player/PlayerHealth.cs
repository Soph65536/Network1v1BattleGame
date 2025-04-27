using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    //character controller for collisions
    private CharacterController cc;

    //health vars
    const int maxHealth = 100;

    public NetworkVariable<int> health = new NetworkVariable<int>(
        value: maxHealth,
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner
        );

    //ui reference to health slider
    public Slider healthSlider;

    //getting hit vars
    const int gotHitAnimationDuration = 200; //(duration in seconds) x 100

    public bool gotHit;
    private bool beingHit;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        health.Value = maxHealth;
        health.OnValueChanged += UpdateHealthSlider;

        gotHit = false;
        beingHit = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (gotHit && !beingHit)
        {
            StartCoroutine("GetHit");
        }
    }

    public void SetHealthSliderValues()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health.Value;
    }

    private void UpdateHealthSlider(int previousValue, int newValue)
    {
        healthSlider.value = newValue;
    }

    public IEnumerator GetHit()
    {
        beingHit = true;

        //set animator trigger to being hit
        //


        int gotHitTimer = 0;

        //while you are currently in being hit and the timer is still going
        while(beingHit && gotHitTimer < gotHitAnimationDuration)
        {
            yield return new WaitForSeconds(0.01f);
            gotHitTimer++;
        }

        //if still being hit and the while loop stopped from timer
        //then stop being hit and got hit
        //this prevent these bools from being reset/changed if you get hit multiple times in a row
        if (beingHit)
        {
            gotHit = false;
            beingHit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsOwner)
        {
            HitCollider hitCollider = collision.GetComponent<HitCollider>();
            PlayerAttack collisionAttack = hitCollider.GetComponentInParent<PlayerAttack>();

            //if own hitcollider then ignore
            if(collisionAttack.gameObject == this) return;

            //otherwise take damage
            if (hitCollider != null && hitCollider.enabled)
            {
                //remove damage from health
                health.Value -= hitCollider.damage * collisionAttack.damageMultipler;

                //got rehit so beinghit is reset and gothit is set to true
                beingHit = false;
                gotHit = true;
            }
        }
    }
}
