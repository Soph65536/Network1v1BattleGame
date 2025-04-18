using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CurrentPlayerCharacter : NetworkBehaviour
{
    //reference this as singleton in other scripts
    [HideInInspector] public static CurrentPlayerCharacter Instance { get { return instance; } }
    private static CurrentPlayerCharacter instance;

    public enum CharacterType
    {
        DaddyLongLegs,
        MothEmperor
    }

    public NetworkVariable<Dictionary<ulong, CharacterType>> currentCharacters = new NetworkVariable<Dictionary<ulong, CharacterType>>(
        readPerm: NetworkVariableReadPermission.Everyone, 
        writePerm: NetworkVariableWritePermission.Owner
        );

    [SerializeField] private RuntimeAnimatorController[] CharacterAnimators;
    [SerializeField] private RuntimeAnimatorController[] CharacterSelectImageAnimators;
    [SerializeField] private Vector2[] CharacterOffsets;
    [SerializeField] private Vector2[] CharacterSizes;

    private void Awake()
    {
        //makes sure there is only one network manager controller and it is set to this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        currentCharacters.Value = new Dictionary<ulong, CharacterType>();
    }

    private void Update()
    {
        foreach (CharacterType character in currentCharacters.Value.Values)
        {
            if (currentCharacters.Value == null) { Debug.Log("Q"); }
            else { Debug.Log(character.ToString()); }
        }
    }

    public RuntimeAnimatorController SetCharacterAnimator(ulong clientId)
    {
        if (!IsOwner) return null;

        return CharacterAnimators[(int)currentCharacters.Value[clientId]];
    }

    public RuntimeAnimatorController SetCharacterSelectImageAnimator(ulong clientId)
    {
        if (!IsOwner) return null;

        return CharacterSelectImageAnimators[(int)currentCharacters.Value[clientId]];
    }

    public Vector2 SetCharacterColliderSize(ulong clientId)
    {
        if (!IsOwner) return Vector2.zero;

        return CharacterSizes[(int)currentCharacters.Value[clientId]];
    }

    public Vector2 SetCharacterColliderOffset(ulong clientId)
    {
        if (!IsOwner) return Vector2.zero;

        return CharacterOffsets[(int)currentCharacters.Value[clientId]];
    }
}
