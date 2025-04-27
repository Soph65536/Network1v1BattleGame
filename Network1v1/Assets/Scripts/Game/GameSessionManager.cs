using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class GameSessionManager : NetworkBehaviour
{
    public NetworkVariable<int> clientsReady = new NetworkVariable<int>(
        value: 0,
        readPerm: NetworkVariableReadPermission.Everyone, 
        writePerm: NetworkVariableWritePermission.Server
        );

    public GameObject WaitingForOtherPlayers; //UI canvas that shows when you are ready but not all players are
    public GameObject RoundEndUI; //UI canvas that shows at the end of a round
    public GameObject SessionEndUI; //UI canvas that shows at the end of the game session

    //set to true if roundend is currently running still
    public NetworkVariable<bool> roundEnding = new NetworkVariable<bool>(
        value: false,
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Server
        );

    public override void OnNetworkSpawn()
    {
        clientsReady.OnValueChanged += ClientsReadyValueChanged;
    }

    private void ClientsReadyValueChanged(int previousValue, int newValue)
    {
        Debug.Log(newValue + " players ready");

        if (newValue == 2)
        {
            WaitingForOtherPlayers.SetActive(false);

            //set player animators
            var gameStarts = FindObjectsOfType<PlayerGameStart>();
            foreach (var item in gameStarts)
            {
                item.GameStart();
                item.GetComponent<CurrentPlayerCharacter>().wins.OnValueChanged += CheckForPlayerWins;
            }
        }
        else
        {
            WaitingForOtherPlayers.SetActive(true);
        }
    }

    private void Update()
    {
        if (!roundEnding.Value) {
            //check players for death
            var playerHealths = FindObjectsOfType<PlayerHealth>();
            foreach (var player in playerHealths)
            {
                if (player.health.Value <= 0)
                {
                    CurrentPlayerCharacter losingPlayer = player.GetComponent<CurrentPlayerCharacter>();
                    ResetGameServerRpc(losingPlayer.currentSide.Value);
                }
            }
        }
    }


    [Rpc(SendTo.Server)]
    private void ResetGameServerRpc(CurrentPlayerCharacter.SideSpawned losingSide)
    {
        if (roundEnding.Value) { return; }
        roundEnding.Value = true;

        //check players for whichever one is this side
        var playerCharacters = FindObjectsOfType<CurrentPlayerCharacter>();
        foreach (var player in playerCharacters)
        {
            if (player.currentSide.Value != losingSide)
            {
                //if not the loser then won so increase wins
                player.IncreaseWinsEveryoneRpc();
            }

            //disable player movement and attack
            player.GetComponent<PlayerMovement>().canMove = false;
            player.GetComponent<PlayerAttack>().canAttack = false;
        }

        //run round end to reset the round
        StartCoroutine("RoundEnd");
    }


    private IEnumerator RoundEnd()
    {
        RoundEndUIActiveEveryoneRpc();
        yield return new WaitForSeconds(2);
        RoundEndUIInactiveEveryoneRpc();

        var playerHealths = FindObjectsOfType<PlayerHealth>();
        foreach (var player in playerHealths)
        {
            Debug.Log(player.health.Value);
            //reset player position and health
            player.transform.position = Vector3.zero;
            player.GetComponent<CurrentPlayerCharacter>().CharacterInitialPosition();
            player.health.Value = PlayerHealth.maxHealth;

            //reenable player movement and attack
            player.GetComponent<PlayerMovement>().canMove = true;
            player.GetComponent<PlayerAttack>().canAttack = true;
        }

        roundEnding .Value = false;
    }

    [Rpc(SendTo.Everyone)]
    private void RoundEndUIActiveEveryoneRpc()
    {
        RoundEndUI.SetActive(true);
    }

    [Rpc(SendTo.Everyone)]
    private void RoundEndUIInactiveEveryoneRpc()
    {
        RoundEndUI.SetActive(false);
    }


    private void CheckForPlayerWins(int previousValue, int newValue)
    {
        //if player has more than 2 wins then session end
        if (newValue >= 2)
        {
            StartCoroutine("SessionEnd");
        }
    }

    private IEnumerator SessionEnd()
    {
        RoundEndUIInactiveEveryoneRpc(); //session ending so disable KO screen

        string winningPlayerName = string.Empty; //this will have the name of the winner in

        var playerCharacters = FindObjectsOfType<CurrentPlayerCharacter>();
        foreach (var player in playerCharacters)
        {
            if (player.wins.Value >= 2)
            {
                //if player has 2 or more wins then they are the winning player
                winningPlayerName = player.currentSide.Value == CurrentPlayerCharacter.SideSpawned.Left ? "Host" : "Client";
            }
        }

        SessionEndUIEveryoneRpc(winningPlayerName);

        yield return new WaitForSeconds(10);
        NetworkManager.Singleton.Shutdown(); //session over so kick players from game
        Application.Quit(); //quit da game
    }

    [Rpc(SendTo.Everyone)]
    private void SessionEndUIEveryoneRpc(string winningPlayerName)
    {
        SessionEndUI.SetActive(true);
        SessionEndUI.GetComponentInChildren<TextMeshProUGUI>().text = winningPlayerName + " Wins!";
    }
}
