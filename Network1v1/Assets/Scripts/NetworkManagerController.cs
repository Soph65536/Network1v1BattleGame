using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerController : NetworkBehaviour
{
    //gameobject prefabs and instances
    [SerializeField] private GameObject CurrentPlayerCharacterPrefab;
    [HideInInspector] public GameObject CurrentPlayerCharacterInstance;

    [SerializeField] private GameObject GameSessionManagerPrefab;
    [HideInInspector] public GameObject GameSessionManagerInstance;

    [SerializeField] private GameObject GameFullError;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStart;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;

        GameFullError.SetActive(false);
    }

    private void OnServerStart()
    {
        if (CurrentPlayerCharacterInstance == null) 
        { 
            CurrentPlayerCharacterInstance = Instantiate(CurrentPlayerCharacterPrefab);
            CurrentPlayerCharacterInstance.GetComponent<NetworkObject>().Spawn();
        }
        
        if (GameSessionManagerInstance == null) 
        { 
            GameSessionManagerInstance = Instantiate(GameSessionManagerPrefab);
            GameSessionManagerInstance.GetComponent<NetworkObject>().Spawn();
        }
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
            GameFullError.SetActive(true);
        }
    }
}
