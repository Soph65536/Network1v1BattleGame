using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject CharacterCustomise;

    private void Awake()
    {
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
        Customise();
    }

    public void PressEnterAsClient()
    {
        //connect as client
        NetworkManager.Singleton.StartClient();
        CloseAllMenus();
        Customise();
    }

    private void CloseAllMenus()
    {
        CharacterCustomise.SetActive(false);

        gameObject.SetActive(false);
    }
}
