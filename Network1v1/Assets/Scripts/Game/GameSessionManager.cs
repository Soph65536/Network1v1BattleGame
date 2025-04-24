using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameSessionManager : NetworkBehaviour
{
    public NetworkVariable<int> clientsReady = new NetworkVariable<int>(
        value: 0,
        readPerm: NetworkVariableReadPermission.Everyone, 
        writePerm: NetworkVariableWritePermission.Server
        );

    public GameObject WaitingForOtherPlayers; //UI canvas that shows when you are ready but not all players are

    public override void OnNetworkSpawn()
    {
        clientsReady.OnValueChanged += ClientsReadyValueChanged;
    }

    private void ClientsReadyValueChanged(int previousValue, int newValue)
    {
        Debug.Log(newValue + " players ready");
        WaitingForOtherPlayers.SetActive(true);

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
    }
}
