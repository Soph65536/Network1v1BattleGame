using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public bool canMove;

    public bool onGround;

    private float moveSpeed = 2.5f;
    private float jumpHeight = 3;

    private float slamAttackSpeed = 5;

    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        enabled = false;  //this will only be enabled by the owning player

        canMove = true;

        onGround = false;

        //get animator and rigidbody
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //if doing plunge attack then call jump downwards
            if (GetComponent<PlayerAttack>().slamRunning)
            {
                JumpServerRpc(Vector2.down * jumpHeight);
            }
            //otherwise can move normally
            else
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
                    JumpServerRpc(Vector2.up * slamAttackSpeed);
                }
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

    [Rpc(SendTo.Everyone)]
    private void UpdateOnGroundServerRpc(bool value)
    {
        onGround = value;
        animator.SetBool("Ground", value);
    }

    //grounded stuff
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        { 
            UpdateOnGroundServerRpc(true);
            GetComponent<PlayerAttack>().StopSlamAttack();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        { 
            UpdateOnGroundServerRpc(false);
        }
    }
}
