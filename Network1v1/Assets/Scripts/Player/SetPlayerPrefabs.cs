using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

            //set player animator as current character
            Player1Prefab.GetComponentInChildren<Animator>().runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator();
            //since player is host then dont flip x for the sprite renderer and attack colliders(child child gameobject)
            Player1Prefab.GetComponentInChildren<SpriteRenderer>().flipX = false;
            Player1Prefab.transform.GetChild(0).transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            //if client set player prefab to player 2
            Player2Prefab.GetComponent<NetworkObject>().SpawnAsPlayerObject(networkManager.LocalClientId, true);

            //set player animator as current character
            Player2Prefab.GetComponentInChildren<Animator>().runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterAnimator();
            //since player is client then flip the sprite renderer
            Player2Prefab.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }
}
