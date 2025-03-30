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
    }


    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("Client Connected");
        EnterGame();
    }

    private void OnClientDisconnect(ulong obj)
    {
        Debug.Log("Client Disconnected");

        //if (IsHost)
        //{
        //    hasPlayer1.Value = false;
        //}
        //else
        //{
        //    hasPlayer2.Value = false;
        //}
    }


    //set player and things once start game session

    private void EnterGame()
    {
        GameObject playerPrefab = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.Singleton.LocalClientId).gameObject;

        if (NetworkManager.Singleton.IsHost)
        {
            //set player initial position
            playerPrefab.transform.position = new Vector3(-5, 0, 0);

            //set player animator as current character
            playerPrefab.GetComponentInChildren<Animator>().runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator();

            //since player is host then dont flip x for the sprite renderer
            playerPrefab.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else
        {
            //set player initial position
            playerPrefab.transform.position = new Vector3(5, 0, 0);

            //set player animator as current character
            playerPrefab.GetComponentInChildren<Animator>().runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator();

            //since player is client then flip the sprite renderer and attack colliders(child child gameobject)
            playerPrefab.GetComponentInChildren<SpriteRenderer>().flipX = true;
            playerPrefab.transform.GetChild(0).transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
