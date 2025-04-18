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
            //set player initial position based on if client ID is odd or even
            transform.position = new Vector3(OwnerClientId % 2 == 0 ? -5 : 5, 0, 0);

            //enable character controller for server
            cc.enabled = true;
        }

        if (IsOwner)
        {
            //enable playermovementscript if owner
            GetComponent<PlayerMovement>().enabled = true;

            CurrentPlayerCharacter.Instance.currentCharacters.Value.Add(OwnerClientId, CurrentPlayerCharacter.CharacterType.DaddyLongLegs);
        }
    }
}
