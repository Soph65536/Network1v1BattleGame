using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public bool canMove;

    private float moveSpeed = 2.5f;
    private float jumpHeight = 3;

    private Rigidbody2D rb;

    private void Awake()
    {
        enabled = false;  //this will only be enabled by the owning player

        canMove = true;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //leftrightmovement
            if (Input.GetKey(KeyCode.A))
            {
                MoveServerRpc(Vector3.left * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                MoveServerRpc(Vector3.right * moveSpeed * Time.deltaTime);
            }

            //jump movement
            if (Input.GetKey(KeyCode.W))
            {
                JumpServerRpc(Vector2.up * jumpHeight);
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void MoveServerRpc(Vector3 movement)
    {
        transform.position += movement;
    }

    [Rpc(SendTo.Server)]
    private void JumpServerRpc(Vector2 movement)
    {
        rb.velocity = movement;
    }
}
