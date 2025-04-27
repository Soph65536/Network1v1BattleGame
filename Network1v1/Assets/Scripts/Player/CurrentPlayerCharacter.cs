using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CurrentPlayerCharacter : NetworkBehaviour
{
    //which character the player is
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

    //whether the player is spawned on the left(host) or the right(client)
    public enum SideSpawned
    {
        Left,
        Right
    }

    public NetworkVariable<SideSpawned> currentSide = new NetworkVariable<SideSpawned>(
        value:SideSpawned.Left,
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner
        );

    //how many wins the player has
    public NetworkVariable<int> wins = new NetworkVariable<int>(
        value: 0,
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner
        );

    //stats for different character types
    [SerializeField] private RuntimeAnimatorController[] CharacterAnimators;
    [SerializeField] private RuntimeAnimatorController[] CharacterSelectImageAnimators;
    [SerializeField] private Vector2[] CharacterOffsets;
    [SerializeField] private Vector2[] CharacterSizes;

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

    public void CharacterInitialPosition()
    {
        float offset = currentSide.Value == SideSpawned.Left ? -5 : 5;

        CharacterPositionServerRpc(offset);
    }

    [Rpc(SendTo.Server)]
    private void CharacterPositionServerRpc(float offset)
    {
        transform.position += Vector3.right * offset;
    }
}
