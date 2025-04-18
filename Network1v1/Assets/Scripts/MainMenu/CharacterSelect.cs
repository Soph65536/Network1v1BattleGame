using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelect : NetworkBehaviour
{
    private Animator CharacterAnimator;

    // Start is called before the first frame update
    void OnEnable()
    {
        CharacterAnimator = GetComponentInChildren<Animator>();
        UpdateCharacterImage();
    }

    // Update is called once per frame
    void UpdateCharacterImage()
    {
        CharacterAnimator.runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterSelectImageAnimator(NetworkManager.Singleton.LocalClientId);
    }

    //character select buttons
    public void SelectCharacterDaddyLongLegs()
    {
        CurrentPlayerCharacter.Instance.currentCharacters.Value[NetworkManager.Singleton.LocalClientId] = CurrentPlayerCharacter.CharacterType.DaddyLongLegs;
        UpdateCharacterImage();
    }

    public void SelectCharacterMothEmperor()
    {
        CurrentPlayerCharacter.Instance.currentCharacters.Value[NetworkManager.Singleton.LocalClientId] = CurrentPlayerCharacter.CharacterType.MothEmperor;
        UpdateCharacterImage();
    }

    public void Ready()
    {
        gameObject.SetActive(false);
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerGameStart>().GameStart();
    }
}
