// Code by Dachi & Viet
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public CharacterController controller;

    Vector3 playerPos;
    Vector3 movement;
    public Rigidbody playerBody;


    public float speed = 12f;
    public float jumpForce = 100f;
    private float friction = 0.10f;

    // Variables for checking player location with respect to the ground.
    bool onGround;
    RaycastHit groundCheck;
    LayerMask ground;
    Vector3 groundOffset = new Vector3(0f, -1f, 0f);


    void Start()
    {
        playerBody.maxLinearVelocity = 25;
        ground = LayerMask.GetMask("Ground");
    }
    void Update()
    {
        // Add ground detection so we can't just use jump to fly using a Raycast.
        if (Physics.Raycast(transform.position + groundOffset, transform.TransformDirection(Vector3.down), out groundCheck, 0.60f, ground))
        {
            onGround = true;

        } else {
            onGround = false;
        }

        // Mobility functionality
        playerPos = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        print(onGround);
        print(Input.GetAxis("Horizontal") + ", " + Input.GetAxis("Vertical"));

        /*if(onGround == true && ((Mathf.Abs(Input.GetAxis("Horizontal")) < 1f || Mathf.Abs(Input.GetAxis("Vertical")) < 1f) || ((Mathf.Abs(Input.GetAxis("Horizontal")) < 1f) && (Math.Abs(Input.GetAxis("Vertical")) < 1f ))))
        {
           movement = transform.TransformDirection(playerPos) * speed * 0.01f; 
        } 
        else if (onGround == true && (Mathf.Abs(Input.GetAxis("Horizontal")) == 1f|| Mathf.Abs(Input.GetAxis("Vertical")) == 1f || (Mathf.Abs(Input.GetAxis("Horizontal")) == 1f) && (Math.Abs(Input.GetAxis("Vertical")) == 1f )))
        {
        movement = transform.TransformDirection(playerPos) * speed * friction;
        }*/
        if(onGround == true && ((Mathf.Abs(Input.GetAxis("Horizontal")) == 0f || Mathf.Abs(Input.GetAxis("Vertical")) == 0.0f) || (Mathf.Abs(Input.GetAxis("Horizontal")) == 0f && Mathf.Abs(Input.GetAxis("Vertical")) == 0.0f)))
        {
            movement = transform.TransformDirection(playerPos) * speed * 0.01f;
            Move();
        }
        else if (onGround == true && ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0f) || (Mathf.Abs(Input.GetAxis("Horizontal")) > 0f && Mathf.Abs(Input.GetAxis("Vertical")) > 0f)))
        {
            movement = transform.TransformDirection(playerPos) * speed * friction;
            Move();
        }

        
        /*if((Input.GetKeyDown(KeyCode.W)) == true && onGround == true)
        {
            movement = transform.TransformDirection(playerPos) * speed * friction;
            playerBody.AddForce(movement.x * 20, 0f, movement.z * 20, ForceMode.Impulse);
            Move();
            
        }
        else if ( (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) == false && onGround == true)
        {
            movement = transform.TransformDirection(playerPos) * speed * 0.01f;
            Move();
        }
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) == false && onGround == false)
        {
            Move();
        }*/
        
        
        //print(playerBody.linearVelocity);

        

        // Jump Functionality
        if(Input.GetButtonDown("Jump") && onGround == true)
        {
            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.75f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.75f)
            {
                playerBody.AddForce(movement.x, 0f, movement.z, ForceMode.Impulse);
            }
        }

    }

    void Move()
    {
        playerBody.AddForce(movement.x * 20, 0f, movement.z * 20, ForceMode.Impulse);
        playerBody.AddForce(movement.x, 0f, movement.z, ForceMode.Acceleration);
    }
}
