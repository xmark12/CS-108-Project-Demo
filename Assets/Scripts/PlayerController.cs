using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*************************************************************************
     * BUGS/Additions:
     * - Added temp sprint speed until can get dash into sprint working.
     * - Super Jump if press space and movement direction while wall sliding.
     *************************************************************************/

    private float movementInputDirection;

    private int facingDirection = 1;

    public bool isFacingRight = true;
    public bool isWalking;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isWallSliding;
    public bool isTouchingLedge;
    public bool ledgeDetected;

    //public Vector2 ledgePosBot;
    public Vector2 offset1;
    public Vector2 offset2;
    public Vector2 climbBegunPosition;
    public bool canGrabLedge = true;
    public bool canClimb;

    private Rigidbody2D rb;
    private Animator anim;

    public SpriteRenderer sr;

    private float currentSpeed;
    public float movementSpeed = 2.0f;
    public float jumpForce = 10.0f;
    public float maxFallSpeed = 4.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.1f;
    public float wallHopForce;
    public float wallJumpForce;

    public float sprintSpeed = 2.0f;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //wallJumpDirection.Normalize();
    }

    void Update()
    {
        CheckInput();
        UpdateAnimations();
        CheckIfWallSliding();
        CheckLedgeClimb();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = movementSpeed + sprintSpeed;
        }
        else
        {
            currentSpeed = movementSpeed;
        }
        
        if (rb.velocity.y < -maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
    }

    private void FixedUpdate()
    {
        CheckMovementDirection();
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

    private void CheckLedgeClimb()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = transform.position;

            climbBegunPosition = ledgePosition + offset1;

            canClimb = true;
        }

        if (canClimb)
        {
            //transform.position = climbBegunPosition;
            if (sr.flipX)
            {
                rb.velocity = new Vector2(wallJumpForce * -wallJumpDirection.x, jumpForce);
            }
            if (!sr.flipX)
            {
                rb.velocity = new Vector2(wallJumpForce * wallJumpDirection.x, jumpForce);
            }

            canClimb = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (sr.flipX)
            isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, wallCheckDistance, whatIsGround);
        else if (!sr.flipX)
            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if (sr.flipX) { 
            isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, -transform.right, wallCheckDistance, whatIsGround);
        }
        else if (!sr.flipX) { 
            isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);
        }

        if (isTouchingWall && !isTouchingLedge)
        {
            ledgeDetected = true;
            //ledgePosBot = wallCheck.position;
            //canClimbLedge = true;
        }
        else
        {
            ledgeDetected = false;
            //canClimbLedge = false;
            canGrabLedge = true;
        }
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

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);

            coyoteTimeCounter = 0f;
        }
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(currentSpeed * movementInputDirection, rb.velocity.y);
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
        else if ((isWallSliding || isTouchingWall) && ((Input.GetButtonUp("Jump") || Input.GetButtonDown("Jump")) || movementInputDirection != 0)) //Wall jump
        {
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
}
