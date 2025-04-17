using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerSpawn : NetworkBehaviour
{
    private CharacterController cc;
    private Animator animator;

    private void Awake()
    {
        //get character controller
        cc = GetComponent<CharacterController>();

        //get animator
        animator = GetComponent<Animator>();
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

            SetupPlayerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void SetupPlayerRpc()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            //since player is host then dont flip x for the sprite renderer
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else
        {
            //since player is client then flip the sprite renderer and attack colliders(child child gameobject)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            transform.GetChild(0).transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
        }

        //set player animator as current character
        animator.runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator();
        //set network animator and controller
        GetComponent<NetworkAnimator>().Animator = GetComponentInChildren<Animator>();
        GetComponent<NetworkAnimator>().Animator.runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator();

        //set character controller size based on current character
        cc.radius = CurrentPlayerCharacter.Instance.SetCharacterColliderSize().x;
        cc.height = CurrentPlayerCharacter.Instance.SetCharacterColliderSize().y;
        cc.center = CurrentPlayerCharacter.Instance.SetCharacterColliderOffset();
    }
}
