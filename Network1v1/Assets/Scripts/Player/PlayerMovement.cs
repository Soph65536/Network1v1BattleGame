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
            JumpServerRPC(Vector3.up * jumpHeight * Time.deltaTime);
        }

        GravityServerRpc(Vector3.down * Time.deltaTime);
    }

    [Rpc(SendTo.Server)]
    private void MoveServerRPC(Vector3 movement)
    {
        cc.Move(movement);
    }

    [Rpc(SendTo.Server)]
    private void JumpServerRPC(Vector3 jumpMovement)
    {
        cc.Move(jumpMovement);
    }

    [Rpc(SendTo.Server)]
    private void GravityServerRpc(Vector3 gravityMovement)
    {
        cc.Move(gravityMovement);
    }
}
