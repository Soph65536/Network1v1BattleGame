using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameStart : NetworkBehaviour
{
    private CapsuleCollider2D capsuleCollider;
    private Animator animator;

    //network singletons
    [HideInInspector] public CurrentPlayerCharacter currentPlayerCharacter;

    private void Awake()
    {
        //get character controller
        capsuleCollider = GetComponent<CapsuleCollider2D>();

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
        PlayerHealth characterHealth = GetComponent<PlayerHealth>();

        //Debug.Log(selectedCharacter.IsLocalPlayer+"\n"+selectedCharacter.currentCharacter.Value+"\n"+selectedCharacter.currentSide.Value);

        if (selectedCharacter.currentSide.Value == CurrentPlayerCharacter.SideSpawned.Right)
        {
            //since player is client then flip the sprite renderer and attack colliders(child child gameobject)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            transform.GetChild(0).transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);

            //set health slider to left side
            characterHealth.healthSlider 
                = GameObject.FindGameObjectWithTag("RightPlayerHealthSlider").GetComponent<Slider>();
        }
        else
        {
            //set health slider to right side
            characterHealth.healthSlider
                = GameObject.FindGameObjectWithTag("LeftPlayerHealthSlider").GetComponent<Slider>();
        }

        //health slider is now set so run playerhealth sethealthslidervalues
        characterHealth.SetHealthSliderValues();

        //set player animator as current character
        animator.runtimeAnimatorController = selectedCharacter.GetCharacterAnimator();
        ////set network animator and controller
        //GetComponent<NetworkAnimator>().Animator = animator;
        //GetComponent<NetworkAnimator>().Animator.runtimeAnimatorController = selectedCharacter.GetCharacterAnimator();

        //set collider size/offset based on current character
        capsuleCollider.size = selectedCharacter.GetCharacterColliderSize();
        capsuleCollider.offset = selectedCharacter.GetCharacterColliderOffset();
    }
}
