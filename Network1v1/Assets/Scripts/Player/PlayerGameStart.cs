using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerGameStart : NetworkBehaviour
{
    private CharacterController cc;
    private Animator animator;

    //network singletons
    [HideInInspector] public CurrentPlayerCharacter currentPlayerCharacter;
    [HideInInspector] public GameSessionManager gameSessionManager;

    private void Awake()
    {
        //get character controller
        cc = GetComponent<CharacterController>();

        //get animator
        animator = GetComponentInChildren<Animator>();
    }

    public void GameStart()
    {
        SetupPlayersServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void SetupPlayersServerRpc()
    {
        gameSessionManager.clientsReady.Value++;

        //check to make sure the gameobject we are doing this on is our player
        foreach (var client in NetworkManager.Singleton.ConnectedClients.Values)
        {
            Debug.Log(((int)currentPlayerCharacter.currentCharacters.Value[client.ClientId]));

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
            animator.runtimeAnimatorController = currentPlayerCharacter.SetCharacterAnimator(client.ClientId);
            //set network animator and controller
            GetComponent<NetworkAnimator>().Animator = animator;
            GetComponent<NetworkAnimator>().Animator.runtimeAnimatorController = currentPlayerCharacter.SetCharacterAnimator(client.ClientId);

            //set character controller size based on current character
            cc.radius = currentPlayerCharacter.SetCharacterColliderSize(client.ClientId).x;
            cc.height = currentPlayerCharacter.SetCharacterColliderSize(client.ClientId).y;
            cc.center = currentPlayerCharacter.SetCharacterColliderOffset(client.ClientId);
        }
    }
}
