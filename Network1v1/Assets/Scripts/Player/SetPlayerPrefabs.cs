using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Animations;
using UnityEngine;

public class SetPlayerPrefabs : MonoBehaviour
{
    public NetworkManager networkManager;

    [SerializeField] private GameObject Player1Prefab;
    [SerializeField] private GameObject Player2Prefab;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = FindFirstObjectByType<NetworkManager>();

        if (networkManager.IsHost)
        {
            //if hosting set player prefab to player 1
            Player1Prefab.GetComponent<NetworkObject>().SpawnAsPlayerObject(networkManager.LocalClientId, true);

            //set player animator as p1 version of selected current character
            Player1Prefab.GetComponentInChildren<Animator>().runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetAnimatorHost();
        }
        else
        {
            //if client set player prefab to player 2
            Player2Prefab.GetComponent<NetworkObject>().SpawnAsPlayerObject(networkManager.LocalClientId, true);

            //set player animator as p2 version of selected current character
            Player1Prefab.GetComponentInChildren<Animator>().runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetAnimatorClient();
        }
    }
}
