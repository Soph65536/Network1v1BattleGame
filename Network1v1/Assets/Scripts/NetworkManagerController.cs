using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerController : MonoBehaviour
{
    public NetworkManager networkManager;


    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject AlreadyHostingError;
    [SerializeField] private GameObject GameFullError;

    private int numOfPlayers;

    private void Awake()
    {
        numOfPlayers = 0;
        MainMenu.SetActive(true);

        GameFullError.SetActive(false);
    }


    private void Start()
    {
        networkManager.OnClientConnectedCallback += OnClientConnect;

        networkManager.StartHost();
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("Client Connected");
    }


    private void EnterGame()
    {
        numOfPlayers++;
        MainMenu.SetActive(false);
    }

    public void EnterAsHost()
    {
        EnterGame();
        networkManager.StartHost();
    }

    public void EnterAsClient()
    {
        if (numOfPlayers < 2)
        {
            EnterGame();
            networkManager.StartClient();
        }
        else
        {
            GameFullError.SetActive(true);
        }
    }
}
