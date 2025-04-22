using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameSessionManager : NetworkBehaviour
{
    public NetworkVariable<int> clientsReady = new NetworkVariable<int>(
        value: 0,
        readPerm: NetworkVariableReadPermission.Everyone, 
        writePerm: NetworkVariableWritePermission.Owner
        );

    private void Update()
    {
        if(clientsReady.Value >= 2)
        {
            Debug.Log("Game Start");
        }
    }
}
