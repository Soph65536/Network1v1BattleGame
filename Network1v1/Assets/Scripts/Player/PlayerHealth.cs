using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    const int maxHealth = 100;

    const int gotHitAnimationDuration = 200; //(duration in seconds) x 100

    public int health;
    public bool gotHit;

    private bool beingHit;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            health = maxHealth;

            gotHit = false;
            beingHit = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner)
        {
            if (gotHit && !beingHit)
            {
                StartCoroutine("GetHit");
            }
        }
    }

    public IEnumerator GetHit()
    {
        beingHit = true;

        //set animator trigger to being hit
        //

        //move this to gamesessionmanager
        if (health < 0)
        {
            //lose
        }


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

            if (hitCollider != null && hitCollider.enabled)
            {
                //remove damage from health
                health -= hitCollider.damage;

                //got rehit so beinghit is reset and gothit is set to true
                beingHit = false;
                gotHit = true;
            }
        }
    }
}
