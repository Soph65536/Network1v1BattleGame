using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerGameStart : NetworkBehaviour
{
    private CharacterController cc;
    private Animator animator;

    private void Awake()
    {
        //get character controller
        cc = GetComponent<CharacterController>();

        //get animator
        animator = GetComponentInChildren<Animator>();
    }

    public void GameStart()
    {
        //GameSessionManager.Instance.clientsReady.Value++;
        if (IsServer)
        {
            SetupPlayersServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void SetupPlayersServerRpc()
    {
        //check to make sure the gameobject we are doing this on is our player
        foreach (var client in NetworkManager.Singleton.ConnectedClients.Values)
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
            animator.runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator(client.ClientId);
            //set network animator and controller
            GetComponent<NetworkAnimator>().Animator = animator;
            GetComponent<NetworkAnimator>().Animator.runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator(client.ClientId);

            //set character controller size based on current character
            cc.radius = CurrentPlayerCharacter.Instance.SetCharacterColliderSize(client.ClientId).x;
            cc.height = CurrentPlayerCharacter.Instance.SetCharacterColliderSize(client.ClientId).y;
            cc.center = CurrentPlayerCharacter.Instance.SetCharacterColliderOffset(client.ClientId);
        }
    }
}
