using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerController : MonoBehaviour
{
    //reference this as singleton in other scripts
    [HideInInspector] public static NetworkManagerController Instance { get { return instance; } }
    private static NetworkManagerController instance;

    public NetworkManager networkManager;
    public NetworkVariable<int> numOfPlayers;

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

        numOfPlayers = new NetworkVariable<int>(0);
    }


    private void Start()
    {
        networkManager.OnClientConnectedCallback += OnClientConnect;
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("Client Connected");
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if(networkManager.ConnectedClientsIds.Count < 2)
        {
            response.Approved = true;
        }
        else
        {
            response.Approved = false;
            response.Reason = "Game Session is Full";
        }
    }


    //main menu stuff

    private void EnterGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        numOfPlayers.Value = numOfPlayers.Value + 1;
    }

    public void EnterAsHost()
    {
        EnterGame();
        networkManager.StartHost();
    }

    public void EnterAsClient()
    {
        EnterGame();
        networkManager.StartClient();
    }

    private void Update()
    {
        Debug.Log(numOfPlayers.Value);
    }
}
