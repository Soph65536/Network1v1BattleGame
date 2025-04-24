using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelect : NetworkBehaviour
{
    private Animator characterAnimator;

    //network singletons
    [HideInInspector] public CurrentPlayerCharacter currentPlayerCharacter;

    private void Awake()
    {
        characterAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentPlayerCharacter = NetworkManager.LocalClient.PlayerObject.GetComponent<CurrentPlayerCharacter>();
        UpdateCharacterImage();
    }

    // Update is called once per frame
    void UpdateCharacterImage()
    {
        if (currentPlayerCharacter != null)
        {
            characterAnimator.runtimeAnimatorController = currentPlayerCharacter.GetCharacterSelectImageAnimator();
        }
    }

    //character select buttons
    public void SelectCharacterDaddyLongLegs()
    {
        SelectCharacter/*ServerRpc*/(CurrentPlayerCharacter.CharacterType.DaddyLongLegs);
        UpdateCharacterImage();
    }

    public void SelectCharacterMothEmperor()
    {
        SelectCharacter/*ServerRpc*/(CurrentPlayerCharacter.CharacterType.MothEmperor);
        UpdateCharacterImage();
    }

    //[Rpc(SendTo.Server)]
    private void SelectCharacter/*ServerRpc*/(CurrentPlayerCharacter.CharacterType characterType)
    {
        //currentPlayerCharacter = GameObject.FindFirstObjectByType<CurrentPlayerCharacter>();
        currentPlayerCharacter = NetworkManager.LocalClient.PlayerObject.GetComponent<CurrentPlayerCharacter>();
        currentPlayerCharacter.currentCharacter.Value = characterType;
    }

    public void Ready()
    {
        gameObject.SetActive(false);
        //NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerGameStart>().GameStart();
        var gameStarts = FindObjectsOfType<PlayerGameStart>();
        foreach (var item in gameStarts)
        {
            item.GameStart();
        }
    }
}
