using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelect : NetworkBehaviour
{
    private Animator CharacterAnimator;

    //network singletons
    [HideInInspector] public CurrentPlayerCharacter currentPlayerCharacter;

    private void Awake()
    {
        CharacterAnimator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        UpdateCharacterImage();
    }

    // Update is called once per frame
    void UpdateCharacterImage()
    {
        if (currentPlayerCharacter != null)
        {
            if (currentPlayerCharacter.currentCharacters.Value.ContainsKey(NetworkManager.Singleton.LocalClientId))
            {
                CharacterAnimator.runtimeAnimatorController = currentPlayerCharacter.SetCharacterSelectImageAnimator(NetworkManager.Singleton.LocalClientId);
            }
        }
    }

    //character select buttons
    public void SelectCharacterDaddyLongLegs()
    {
        currentPlayerCharacter.currentCharacters.Value[NetworkManager.Singleton.LocalClientId] = CurrentPlayerCharacter.CharacterType.DaddyLongLegs;
        UpdateCharacterImage();
    }

    public void SelectCharacterMothEmperor()
    {
        currentPlayerCharacter.currentCharacters.Value[NetworkManager.Singleton.LocalClientId] = CurrentPlayerCharacter.CharacterType.MothEmperor;
        UpdateCharacterImage();
    }

    public void Ready()
    {
        gameObject.SetActive(false);
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerGameStart>().GameStart();
    }
}
