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

    private void SetupPlayers()
    {
        CurrentPlayerCharacter selectedCharacter = GetComponent<CurrentPlayerCharacter>();

        //Debug.Log(selectedCharacter.IsLocalPlayer+"\n"+selectedCharacter.currentCharacter.Value+"\n"+selectedCharacter.currentSide.Value);

        if (selectedCharacter.currentSide.Value == CurrentPlayerCharacter.SideSpawned.Right)
        {
            //since player is client then flip the sprite renderer and attack colliders(child child gameobject)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            transform.GetChild(0).transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
        }

        //set player animator as current character
        animator.runtimeAnimatorController = selectedCharacter.GetCharacterAnimator();
        ////set network animator and controller
        //GetComponent<NetworkAnimator>().Animator = animator;
        //GetComponent<NetworkAnimator>().Animator.runtimeAnimatorController = selectedCharacter.GetCharacterAnimator();

        //set character controller size based on current character
        cc.radius = selectedCharacter.GetCharacterColliderSize().x;
        cc.height = selectedCharacter.GetCharacterColliderSize().y;
        cc.center = selectedCharacter.GetCharacterColliderOffset();
    }
}
