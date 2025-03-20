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

    public int numOfPlayers;

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

        numOfPlayers = 0;
    }


    private void Start()
    {
        networkManager.OnClientConnectedCallback += OnClientConnect;
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("Client Connected");
    }


    //main menu stuff

    private void EnterGame()
    {
        numOfPlayers++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
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
}
