using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private MonitorPlayersJoining monitorPlayersJoining;

    [SerializeField] private GameObject AlreadyHostingError;
    [SerializeField] private GameObject GameFullError;
    [SerializeField] private GameObject GameEmptyError;

    [SerializeField] private GameObject CharacterCustomise;

    private void Awake()
    {
        AlreadyHostingError.SetActive(false);
        GameFullError.SetActive(false);
        GameEmptyError.SetActive(false);
        CharacterCustomise.SetActive(false);
    }

    //buttons for menu
    public void Customise()
    {
        CharacterCustomise.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PressEnterAsHost()
    {
        //connect as host
        NetworkManager.Singleton.StartHost();
        CloseAllMenus();

        //if (monitorPlayersJoining.hasPlayer1RO) //if not the only host then stop
        //{
        //    NetworkManager.Singleton.Shutdown();
        //    AlreadyHostingError.SetActive(true);
        //}
        //else //otherwise can host
        //{
        //    monitorPlayersJoining.EnterAsHost();
        //    CloseAllMenus();
        //}
    }

    public void PressEnterAsClient()
    {
        //connect as client
        NetworkManager.Singleton.StartClient();
        CloseAllMenus();

        //if (!monitorPlayersJoining.hasPlayer1RO) //if no host game is empty so cannot join
        //{
        //    NetworkManager.Singleton.Shutdown();
        //    GameEmptyError.SetActive(true);
        //}
        //else if(monitorPlayersJoining.hasPlayer2RO) //if already has player 2 then game is full
        //{
        //    NetworkManager.Singleton.Shutdown();
        //    GameFullError.SetActive(true);
        //}
        //else //otherwise is hosting but no second player so can join
        //{
        //    monitorPlayersJoining.EnterAsClient();
        //    CloseAllMenus();
        //}
    }

    private void CloseAllMenus()
    {
        AlreadyHostingError.SetActive(false);
        GameFullError.SetActive(false);
        GameEmptyError.SetActive(false);
        CharacterCustomise.SetActive(false);

        gameObject.SetActive(false);
    }
}
