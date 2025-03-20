using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Animations;
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

    [SerializeField] private AnimatorController[] HostAnimators;
    [SerializeField] private AnimatorController[] ClientAnimators;

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

    public AnimatorController SetAnimatorHost()
    {
        Debug.Log((int)currentCharacter);
        return HostAnimators[1];
    }

    public AnimatorController SetAnimatorClient()
    {
        return ClientAnimators[(int)currentCharacter];
    }
}
