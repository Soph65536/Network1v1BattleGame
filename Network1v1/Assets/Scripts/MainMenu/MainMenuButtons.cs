using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject AlreadyHostingError;
    [SerializeField] private GameObject GameFullError;
    [SerializeField] private GameObject GameEmptyError;

    private void Awake()
    {
        AlreadyHostingError.SetActive(false);
        GameFullError.SetActive(false);
        GameEmptyError.SetActive(false);
    }

    //buttons for menu
    public void Customise()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PressEnterAsHost()
    {
        if (NetworkManagerController.Instance.numOfPlayers == 0) //if no players then noone is hosting so can host
        {
            NetworkManagerController.Instance.EnterAsHost();
        }
        else
        {
            AlreadyHostingError.SetActive(true);
        }
    }

    public void PressEnterAsClient()
    {
        switch (NetworkManagerController.Instance.numOfPlayers)
        {
            case 0:
                //game is empty
                GameEmptyError.SetActive(true);
                break;
            case 1:
                //if someone is hosting but game not full then can join
                NetworkManagerController.Instance.EnterAsClient();
                break;
            default:
                //otherwise game will be full
                GameFullError.SetActive(true);
                break;
        }
    }
}
