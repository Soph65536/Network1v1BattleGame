using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelect : NetworkBehaviour
{
    private Animator characterAnimator;

    //network singletons
    [HideInInspector] public CurrentPlayerCharacter currentPlayerCharacter;
    [HideInInspector] public GameSessionManager gameSessionManager;

    private void Awake()
    {
        characterAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //get refs to things
        currentPlayerCharacter = NetworkManager.LocalClient.PlayerObject.GetComponent<CurrentPlayerCharacter>();
        gameSessionManager = GameObject.FindFirstObjectByType<GameSessionManager>();

        //set what side we are on based on if we are host or client, then set our position based on that
        currentPlayerCharacter = NetworkManager.LocalClient.PlayerObject.GetComponent<CurrentPlayerCharacter>();
        currentPlayerCharacter.currentSide.Value = IsHost ? CurrentPlayerCharacter.SideSpawned.Left : CurrentPlayerCharacter.SideSpawned.Right;
        currentPlayerCharacter.CharacterInitialPosition();

        //update character image so it goes to default at first
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
        //close menu and set this player to ready
        gameObject.SetActive(false);
        UpdateReadyPlayersServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void UpdateReadyPlayersServerRpc()
    {
        gameSessionManager.clientsReady.Value++;
    }
}
