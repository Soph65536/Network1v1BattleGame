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

    // Start is called before the first frame update
    void OnEnable()
    {
        currentPlayerCharacter = GameObject.FindFirstObjectByType<CurrentPlayerCharacter>();
        UpdateCharacterImage();
    }


    // Update is called once per frame
    void UpdateCharacterImage()
    {
        if (currentPlayerCharacter != null)
        {
            characterAnimator.runtimeAnimatorController = currentPlayerCharacter.SetCharacterSelectImageAnimator();
        }
    }

    //character select buttons
    public void SelectCharacterDaddyLongLegs()
    {
        SelectCharacterServerRpc(CurrentPlayerCharacter.CharacterType.DaddyLongLegs);
        UpdateCharacterImage();
    }

    public void SelectCharacterMothEmperor()
    {
        SelectCharacterServerRpc(CurrentPlayerCharacter.CharacterType.MothEmperor);
        UpdateCharacterImage();
    }

    [Rpc(SendTo.Server)]
    private void SelectCharacterServerRpc(CurrentPlayerCharacter.CharacterType characterType)
    {
        currentPlayerCharacter = GameObject.FindFirstObjectByType<CurrentPlayerCharacter>();
        currentPlayerCharacter.currentCharacter.Value = characterType;
    }

    public void Ready()
    {
        gameObject.SetActive(false);
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerGameStart>().GameStart();
    }
}
