using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LeNetworkManager : NetworkManager
{
    //gameobject prefabs and instances
    [SerializeField] private GameObject CurrentPlayerCharacterPrefab;
    public GameObject CurrentPlayerCharacterInstance;

    [SerializeField] private GameObject GameSessionManagerPrefab;
    public GameObject GameSessionManagerInstance;

    [SerializeField] private GameObject GameFullErrorPrefab;
    public GameObject GameFullErrorInstance;

    private void Start()
    {
        OnServerStarted += OnServerStart;

        OnClientConnectedCallback += OnClientConnect;
        OnClientDisconnectCallback += OnClientDisconnect;

        ConnectionApprovalCallback += ApprovalCheck;
    }

    private void OnServerStart()
    {
        CurrentPlayerCharacterInstance = Instantiate(CurrentPlayerCharacterPrefab);

        GameSessionManagerInstance = Instantiate(GameSessionManagerPrefab);

        GameFullErrorInstance = Instantiate(GameFullErrorPrefab);
        GameFullErrorInstance.SetActive(false);
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("Client Connected");
    }

    private void OnClientDisconnect(ulong obj)
    {
        Debug.Log("Client Disconnected");
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (NetworkManager.Singleton.ConnectedClientsIds.Count < 2)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            return;
        }
        else
        {
            response.Approved = false;
            response.Reason = "Game Session is Full";
            GameFullErrorInstance.SetActive(true);
        }
    }
}
