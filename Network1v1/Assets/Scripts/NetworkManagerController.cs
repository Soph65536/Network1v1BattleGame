using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerController : NetworkBehaviour
{
    //reference this as singleton in other scripts
    [HideInInspector] public static NetworkManagerController Instance { get { return instance; } }
    private static NetworkManagerController instance;

    [SerializeField] private GameObject GameFullError;

    private void Awake()
    {
        //makes sure there is only one network manager controller and it is set to this
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        GameFullError.SetActive(false);
    }


    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
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
        }
        else
        {
            response.Approved = false;
            response.Reason = "Game Session is Full";
            GameFullError.SetActive(true);
        }
    }
}
