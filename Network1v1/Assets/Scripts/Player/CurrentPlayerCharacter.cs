using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CurrentPlayerCharacter : NetworkBehaviour
{
    public enum CharacterType
    {
        DaddyLongLegs,
        MothEmperor
    }

    public NetworkVariable<CharacterType> currentCharacter = new NetworkVariable<CharacterType>(
        value:CharacterType.DaddyLongLegs,
        readPerm: NetworkVariableReadPermission.Everyone, 
        writePerm: NetworkVariableWritePermission.Owner
        );

    [SerializeField] private RuntimeAnimatorController[] CharacterAnimators;
    [SerializeField] private RuntimeAnimatorController[] CharacterSelectImageAnimators;
    [SerializeField] private Vector2[] CharacterOffsets;
    [SerializeField] private Vector2[] CharacterSizes;

    private void Update()
    {
        Debug.Log(currentCharacter.Value.ToString());
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(!IsOwner)
        {
            enabled = false;
        }
    }

    public RuntimeAnimatorController GetCharacterAnimator()
    {
        return CharacterAnimators[(int)currentCharacter.Value];
    }

    public RuntimeAnimatorController GetCharacterSelectImageAnimator()
    {
        return CharacterSelectImageAnimators[(int)currentCharacter.Value];
    }

    public Vector2 GetCharacterColliderSize()
    {
        return CharacterSizes[(int)currentCharacter.Value];
    }

    public Vector2 GetCharacterColliderOffset()
    {
        return CharacterOffsets[(int)currentCharacter.Value];
    }
}
