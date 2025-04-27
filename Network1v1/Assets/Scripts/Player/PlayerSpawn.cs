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
            //enable player scripts if owner
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerAttack>().enabled = true;
        }
    }
}
