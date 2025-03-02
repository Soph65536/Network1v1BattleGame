using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerController : MonoBehaviour
{
    public NetworkManager networkManager;

    private void Start()
    {
        networkManager.OnClientConnectedCallback += OnClientConnect;

#if UNITY_EDITOR
        networkManager.StartHost();
#else
        networkManager.StartClient();
#endif
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("Client Connected");
    }
}
