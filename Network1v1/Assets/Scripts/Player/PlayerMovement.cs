using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public bool canMove;

    public bool onGround {  get; private set; }

    private float horizontalInput = 0;
    private float moveSpeed = 3.5f;
    private float jumpHeight = 400;

    private float slamAttackSpeed = 700;

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
            //leftrightmovement
            horizontalInput = Input.GetAxisRaw("Horizontal");
            MoveServerRpc(Vector3.right * horizontalInput * moveSpeed);

            //if doing slam attack then call jump downwards
            if (GetComponent<PlayerAttack>().slamRunning)
            {
                JumpServerRpc(Vector2.down * slamAttackSpeed);
            }
            //otherwise can jump
            else
            {
                //jump movement
                if (Input.GetKey(KeyCode.W))
                {
                    JumpServerRpc(Vector2.up * jumpHeight);
                }
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void MoveServerRpc(Vector3 movement)
    {
        transform.position += movement * Time.deltaTime;
    }

    [Rpc(SendTo.Server)]
    private void JumpServerRpc(Vector2 movement)
    {
        rb.velocity = movement * Time.deltaTime;
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
