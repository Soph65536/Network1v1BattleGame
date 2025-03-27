using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CurrentPlayerCharacter : MonoBehaviour
{
    //reference this as singleton in other scripts
    [HideInInspector] public static CurrentPlayerCharacter Instance { get { return instance; } }
    private static CurrentPlayerCharacter instance;

    public NetworkManager networkManager;

    public enum CharacterType
    {
        DaddyLongLegs,
        MothEmperor
    }

    public CharacterType currentCharacter;

    [SerializeField] private RuntimeAnimatorController[] CharacterAnimators;

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

        currentCharacter = CharacterType.DaddyLongLegs;
    }

    public RuntimeAnimatorController SetCharacterAnimator()
    {
        Debug.Log((int)currentCharacter);
        return CharacterAnimators[1];
    }
}
