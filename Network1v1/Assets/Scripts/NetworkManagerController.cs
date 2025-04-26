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
    [SerializeField] private GameObject CharacterSelectPrefab;
    [HideInInspector] public GameObject CharacterSelectInstance;

    [SerializeField] private GameObject GameSessionManagerPrefab;
    [SerializeField] private GameObject GameSessionManagerUIGameObject;
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
        if(CharacterSelectInstance == null)
        {
            CharacterSelectInstance = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(CharacterSelectPrefab.GetComponent<NetworkObject>()).gameObject;
        }

        if (GameSessionManagerInstance == null)
        {
            GameSessionManagerInstance = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(GameSessionManagerPrefab.GetComponent<NetworkObject>()).gameObject;
            GameSessionManagerInstance.GetComponent<GameSessionManager>().WaitingForOtherPlayers = GameSessionManagerUIGameObject;
        }
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("Client Connected");

        //get game session manager and make sure its waitinforotherplayers is set to the correct gameobject
        GameObject.FindFirstObjectByType<GameSessionManager>().GetComponent<GameSessionManager>().WaitingForOtherPlayers = GameSessionManagerUIGameObject;
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
        }
    }
}
