using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    //health vars
    public const int maxHealth = 100;

    public NetworkVariable<int> health = new NetworkVariable<int>(
        value: maxHealth,
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Server
        );

    //getting hit vars
    const int gotHitAnimationDuration = 100; //(duration in seconds) x 100

    public bool gotHit;
    private bool beingHit;

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        gotHit = false;
        beingHit = false;

        //get animator
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gotHit && !beingHit)
        {
            StartCoroutine("GetHit");
        }
    }

    public IEnumerator GetHit()
    {
        beingHit = true;

        //set animator trigger to being hit
        HitAnimationTrueServerRpc();

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
            HitAnimationFalseServerRpc(); //no longer being hit so animator hit is false

            gotHit = false;
            beingHit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsOwner)
        {
            HitCollider hitCollider = collision.GetComponent<HitCollider>();

            //if own hitcollider then ignore
            if(hitCollider == GetComponentInChildren<HitCollider>()) return;

            //otherwise take damage
            if (hitCollider != null && hitCollider.enabled)
            {
                //remove damage from health
                RemoveHealthServerRpc(hitCollider.damage);

                //got rehit so beinghit and animator are reset and gothit is set to true
                HitAnimationFalseServerRpc();

                beingHit = false;
                gotHit = true;
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void RemoveHealthServerRpc(int damage)
    {
        health.Value -= damage;
    }

    [Rpc(SendTo.Server)]
    private void HitAnimationTrueServerRpc()
    {
        animator.SetBool("Hit", true);
    }

    [Rpc(SendTo.Server)]
    private void HitAnimationFalseServerRpc()
    {
        animator.SetBool("Hit", false);
    }
}
