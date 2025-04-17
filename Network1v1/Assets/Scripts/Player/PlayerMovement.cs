using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    private float moveSpeed = 2.5f;
    private float jumpHeight = 3;

    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        cc.enabled = false;
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //leftrightmovement
        if (Input.GetKey(KeyCode.A))
        {
            MoveServerRPC(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveServerRPC(Vector3.right * moveSpeed * Time.deltaTime);
        }

        //jump movement
        if (Input.GetKey(KeyCode.W))
        {
            MoveServerRPC(Vector3.up * jumpHeight * Time.deltaTime);
        }

        //add gravity to player
        MoveServerRPC(Vector3.down * Time.deltaTime);
    }

    [Rpc(SendTo.Server)]
    private void MoveServerRPC(Vector3 movement)
    {
        cc.Move(movement);
    }
}
