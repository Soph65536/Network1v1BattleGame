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

    private void Start()
    {
        clientsReady.OnValueChanged += ClientsReadyValueChanged;
    }

    private void ClientsReadyValueChanged(int previousValue, int newValue)
    {
        if(newValue == 2)
        {
            Debug.Log("Game Start");
        }
    }
}
