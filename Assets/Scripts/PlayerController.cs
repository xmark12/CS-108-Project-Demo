using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /******
     * BUGS/Additions:
     * - Added temp sprint speed until can get dash into sprint working.
     * - Sprinting while wall jumping gives you ridiculous speed.
     ******/

    private float movementInputDirection;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    public bool isFacingRight = true;
    public bool isWalking;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isWallSliding;

    public int amountOfJumps = 1;

    private Rigidbody2D rb;
    private Animator anim;

    public SpriteRenderer sr;

    private float currentSpeed;
    public float movementSpeed = 2.0f;
    public float jumpForce = 10.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.1f;
    public float wallJumpForce;

    public float sprintSpeed = 2.0f;

    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallJumpDirection.Normalize();
    }

    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfWallSliding();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Sprint is being called");
            currentSpeed = movementSpeed + sprintSpeed;
        }
        else
        {
            currentSpeed = movementSpeed;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (sr.flipX)
            isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, wallCheckDistance, whatIsGround);
        else if (!sr.flipX)
            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void Jump()
    {
        if (isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0)
        {
            
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }

    private void ApplyMovement()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(currentSpeed * movementInputDirection, rb.velocity.y);
        }
        else if (!isGrounded && !isWallSliding && movementInputDirection != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if (Mathf.Abs(rb.velocity.x) > currentSpeed)
            {
                rb.velocity = new Vector2(currentSpeed * movementInputDirection, rb.velocity.y);
            }
        }
        else if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        if (rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void Flip()
    {
        if (!isWallSliding)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            sr.flipX = !sr.flipX;
        }
        else if ((isWallSliding || isTouchingWall) && movementInputDirection != 0) //Wall jump
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
