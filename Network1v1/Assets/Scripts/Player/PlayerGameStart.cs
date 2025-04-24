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
    //[HideInInspector] public CurrentPlayerCharacter currentPlayerCharacter;
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
        SetupPlayers();
    }

    [Rpc(SendTo.Server)]
    private void UpdateReadyPlayersServerRpc()
    {
        gameSessionManager.clientsReady.Value++;
    }

    private void SetupPlayers()
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

        CurrentPlayerCharacter selectedCharacter = GetComponent<CurrentPlayerCharacter>();

        //set player animator as current character
        animator.runtimeAnimatorController = selectedCharacter.GetCharacterAnimator();
        //set network animator and controller
        GetComponent<NetworkAnimator>().Animator = animator;
        GetComponent<NetworkAnimator>().Animator.runtimeAnimatorController = selectedCharacter.GetCharacterAnimator();

        //set character controller size based on current character
        cc.radius = selectedCharacter.GetCharacterColliderSize().x;
        cc.height = selectedCharacter.GetCharacterColliderSize().y;
        cc.center = selectedCharacter.GetCharacterColliderOffset();
    }
}
