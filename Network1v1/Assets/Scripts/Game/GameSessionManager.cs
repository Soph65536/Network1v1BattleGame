using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
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

    private bool roundEnding; //set to true if roundend is currently running still

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
            }
        }
        else
        {
            WaitingForOtherPlayers.SetActive(true);
        }
    }

    private void Update()
    {
        if (!roundEnding) {
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

    [Rpc(SendTo.Everyone)]
    private void ResetGameServerRpc(CurrentPlayerCharacter.SideSpawned losingSide)
    {
        roundEnding = true;

        bool sessionEnded = false; //this is set to true if session ended so it doesn't show the KO screen

        //check players for whichever one is this side
        var playerCharacters = FindObjectsOfType<CurrentPlayerCharacter>();
        foreach (var player in playerCharacters)
        {
            if (player.currentSide.Value != losingSide)
            {
                //if not the loser then won so increase wins
                player.wins.Value++;

                //if player has more than 2 wins then session end
                if (player.wins.Value >= 2)
                {
                    sessionEnded = true;
                    StartCoroutine("SessionEnd", player);
                }
            }

            //disable player movement and attack
            player.GetComponent<PlayerMovement>().canMove.Value = false;
            player.GetComponent<PlayerAttack>().canAttack.Value = false;
        }

        if (!sessionEnded)
        {
            //session hasn't ended so run round end to reset the round
            StartCoroutine("RoundEnd");
        }
    }

    private IEnumerator RoundEnd()
    {
        RoundEndUI.SetActive(true);
        yield return new WaitForSeconds(2);
        RoundEndUI.SetActive(false);

        var playerHealths = FindObjectsOfType<PlayerHealth>();
        foreach (var player in playerHealths)
        {
            //reset player position and health
            player.transform.position = Vector3.zero;
            player.GetComponent<CurrentPlayerCharacter>().CharacterInitialPosition();
            player.health.Value = PlayerHealth.maxHealth;

            //reenable player movement and attack
            player.GetComponent<PlayerMovement>().canMove.Value = true;
            player.GetComponent<PlayerAttack>().canAttack.Value = true;
        }

        roundEnding = false;
    }

    private IEnumerator SessionEnd(CurrentPlayerCharacter winningPlayer)
    {
        string winningPlayerName = winningPlayer.currentSide.Value == CurrentPlayerCharacter.SideSpawned.Left ? "Host" : "Client";

        SessionEndUI.SetActive(true);
        SessionEndUI.GetComponentInChildren<TextMeshProUGUI>().text = winningPlayerName + " Wins!";

        yield return new WaitForSeconds(10);
        NetworkManager.Singleton.Shutdown(); //session over so kick players from game
    }

    [Rpc(SendTo.Everyone)]
    private void RoundEndUIActiveServerRpc()
    {
        
    }

    [Rpc(SendTo.Everyone)]
    private void RoundEndUIInactiveServerRpc()
    {
        
    }

    [Rpc(SendTo.Everyone)]
    private void SessionEndUIServerRpc(string winningPlayerName)
    {
        
    }
}
