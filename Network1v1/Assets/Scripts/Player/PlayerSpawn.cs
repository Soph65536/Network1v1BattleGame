using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using static CurrentPlayerCharacter;

[RequireComponent(typeof(CharacterController))]
public class PlayerSpawn : NetworkBehaviour
{
    private CharacterController cc;
    public CurrentPlayerCharacter currentPlayerCharacter;

    private void Awake()
    {
        //get character controller
        cc = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            //enable character controller for server
            cc.enabled = true;
        }

        if (IsOwner)
        {
            //enable player scripts if owner
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerAttack>().enabled = true;
        }
    }
}
