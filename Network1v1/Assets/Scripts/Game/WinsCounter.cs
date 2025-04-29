using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WinsCounter : NetworkBehaviour
{
    const string winsTextStart = "Wins: ";

    //ui reference to wins text
    public TextMeshProUGUI winsText;

    [SerializeField] private CurrentPlayerCharacter.SideSpawned whichSide;

    CurrentPlayerCharacter ownerPlayerCharacter;

    private void Awake()
    {
        winsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //try get owner player character if not already gotten
        if (ownerPlayerCharacter == null)
        {
            //check players for whichever one is this side
            var playerCharacters = FindObjectsOfType<CurrentPlayerCharacter>();
            foreach (var player in playerCharacters)
            {
                if (player.currentSide.Value == whichSide)
                {
                    //set player character to this
                    ownerPlayerCharacter = player;

                    ownerPlayerCharacter.wins.OnValueChanged += UpdateWinsTextServerRpc;
                    UpdateWinsTextServerRpc(0, 0); //update text once at start with 0 as params because the game just started probably
                    return;
                }
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    private void UpdateWinsTextServerRpc(int previousValue, int newValue)
    {
        winsText.text = winsTextStart + ownerPlayerCharacter.wins.Value;
    }
}
