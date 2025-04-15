using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private CapsuleCollider2D PlayerCollider;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (NetworkManager.Singleton.IsHost)
        {
            //set player initial position
            transform.position = Vector3.zero + Vector3.left * 5;
            //since player is host then dont flip x for the sprite renderer
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else
        {
            //set player initial position
            transform.position = Vector3.zero + Vector3.right * 5;

            //since player is client then flip the sprite renderer and attack colliders(child child gameobject)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            transform.GetChild(0).transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
        }

        //set player animator as current character
        GetComponentInChildren<Animator>().runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator();

        //set player colliders based on current character
        PlayerCollider.size = CurrentPlayerCharacter.Instance.SetCharacterColliderSize();
        PlayerCollider.offset = CurrentPlayerCharacter.Instance.SetCharacterColliderOffset();

        //sync network animator
        GetComponent<NetworkAnimator>().Animator = GetComponentInChildren<Animator>();
    }
}
