using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    public CurrentPlayerCharacter currentPlayerCharacter;

    private void Awake()
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            //cap frame rate at 60fps to help avoid desync
            Application.targetFrameRate = 60;

            //enable player scripts if owner
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerMovement>().canMove = false;

            GetComponent<PlayerAttack>().enabled = true;
            GetComponent<PlayerAttack>().canAttack = false;
        }
    }
}
