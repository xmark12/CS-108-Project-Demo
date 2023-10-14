using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //booleans used for checking where the player is
    public bool grounded;
    public bool jump;
    public bool duck;
    public bool isJumping;

    public Animator a;

    private Rigidbody2D rb;
    public float jumpForce = 5;

    //the 3 values below are for checking if the player is grounded
    public Transform feetPos;
    public float checkRadius = 0.3F;
    public LayerMask whatIsGround;

    //the 2 values below are for allowing the player to jump higher the longer they hold
    private float jumpTimeCounter;
    public float jumpTime = 1.0F;

    //the 5 values below are for changing the player's collision detection while ducking
    public BoxCollider2D collider;
    public Vector2 regularSize;
    public Vector2 duckingSize;
    public Vector2 regularOffset;
    public float duckingOffsetY;

    // Start is called before the first frame update
    void Start()
    {
        grounded = true;
        jump = false;
        duck = false;
        isJumping = false;

        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        regularSize = collider.size;
        duckingSize = collider.size;
        duckingSize.y = duckingSize.y / 2;

        regularOffset = collider.offset;
        duckingOffsetY = -duckingSize.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround); //checks if the user is grounded every frame
        jump = !grounded;

        if (grounded && (Input.GetKeyDown("space") || Input.GetKeyDown("w")))
        {
            isJumping = true;
            duck = false;
            collider.size = regularSize; //makes sure user has normal collision while jumping
            collider.offset = regularOffset;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;

        }
        else if (grounded && Input.GetKeyDown("s"))
        {
            duck = true;
            collider.size = duckingSize;
            collider.offset = new Vector2(regularOffset.x, duckingOffsetY); //changes user's collision while ducking
        }

        if (Input.GetKeyDown("d"))
        {
            rb.velocity = Vector2.right * 2;

        }

        if (Input.GetKeyDown("a"))
        {
            rb.velocity = Vector2.right * -2;

        }

        if ((Input.GetKey("space") || Input.GetKey("w")) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;

                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false; //after holding jump for a certain amount of time, can no longer go higher from holding jump
            }
        }

        if (Input.GetKeyUp("space") || Input.GetKeyUp("w"))
        {
            isJumping = false;
        }
        else if (Input.GetKeyUp("s"))
        {
            duck = false;
            collider.size = regularSize;
            collider.offset = regularOffset;
        }

        //based on the position and movement of the player, displays the proper animation
        if (jump && !grounded && !duck)
        {
            a.Play("Jump");
        }
        else if (duck && grounded && !jump)
        {
            a.Play("Duck");
        }
        else
        {
            a.Play("Grounded");
        }
    }
}
