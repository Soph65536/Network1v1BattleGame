using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    //buttons for menu
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PressEnterAsHost()
    {
        //connect as host
        NetworkManager.Singleton.StartHost();
        CloseAllMenus();
    }

    public void PressEnterAsClient()
    {
        //connect as client
        NetworkManager.Singleton.StartClient();
        CloseAllMenus();
    }

    private void CloseAllMenus()
    {
        gameObject.SetActive(false);
    }
}
