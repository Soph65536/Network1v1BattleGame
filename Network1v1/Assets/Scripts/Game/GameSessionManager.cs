using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static CurrentPlayerCharacter;

public class GameSessionManager : NetworkBehaviour
{
    //reference this as singleton in other scripts
    [HideInInspector] public static GameSessionManager Instance { get { return instance; } }
    private static GameSessionManager instance;

    public NetworkVariable<int> clientsReady = new NetworkVariable<int>(
        readPerm: NetworkVariableReadPermission.Everyone, 
        writePerm: NetworkVariableWritePermission.Owner
        );

    private void Awake()
    {
        //makes sure there is only one network manager controller and it is set to this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        clientsReady.Value = 0;
    }

    private void Update()
    {
        if(clientsReady.Value >= 2)
        {
            Debug.Log("Game Start");
        }
    }
}
