using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*************************************************************************
     * BUGS/Additions:
     * - Super Jump if press space and movement direction while wall sliding.
     *************************************************************************/

    public float movementInputDirection;

    private int facingDirection = 1;

    public bool isFacingRight = true;
    public bool isWalking;
    public bool isSprinting;
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

    private float currentSpeed = 0;
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

    public float acceleration = 0.01f;
    public float currentForwardDirection = 1;
    public float currentAccel = 0;

    public float sprintSpeed = 2.0f;
    public float distanceBetweenImages = 1f;
    private float lastImageXpos;

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

    [SerializeField] private AudioSource runSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource breezeSoundEffect;
    [SerializeField] private AudioSource wallConnectSoundEffect;

    public bool isJumpSound = false;
    public bool isRunSound = false;
    public bool isWallConnectSound = false;

    public bool pauseTrigger = false;
    public NewGame ng;
    public NewGame ng2;
    public FinishLine fl;

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
        
        if (rb.velocity.y < -maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            fl.stagePaused = true;
        }

        if (fl.stagePaused)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                fl.stagePaused = false;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ng.NextScene();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ng2.NextScene();
            }
        }
    }

    private void FixedUpdate()
    {
        CheckMovementDirection();
	    CheckIfWallSliding();
	    CheckLedgeClimb();
        ApplyMovement();
        CheckSurroundings();

        if (((Mathf.Abs(movementInputDirection) > 0) && !isWallSliding) || ((rb.velocity.y == -maxFallSpeed && !isWallSliding)))
        {
            currentAccel += acceleration * (Time.fixedDeltaTime * 0.05f);
        }
        else if (isWallSliding)
        {

        }
        else if (isTouchingWall && isGrounded)
        {
            currentAccel = 0;
        }
        else
        {
            currentAccel -= acceleration * (Time.fixedDeltaTime * 0.5f);
        }

        currentAccel = Mathf.Clamp(currentAccel, 0, 2);

        if (((rb.velocity.x != 0 || !isGrounded) && !isTouchingWall))// && !isWallSliding)
        {
            isSprinting = true;

            currentSpeed = movementSpeed + sprintSpeed + currentAccel;

            if (currentSpeed >= (movementSpeed + 1.25f + sprintSpeed))
            {
                AfterimagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    AfterimagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            else
            {

            }
        }
        else if (isTouchingWall && isGrounded)
        {
            currentSpeed = 0;
            isSprinting = false;
        }
        else
        {
            currentSpeed = movementSpeed;
            isSprinting = false;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, (movementSpeed + 1.5f + sprintSpeed));
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

            currentAccel -= acceleration * (Time.fixedDeltaTime);
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

            currentAccel -= acceleration * (Time.fixedDeltaTime);
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
        anim.SetBool("isSprinting", isSprinting);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (!isRunSound && isSprinting)
        {
            runSoundEffect.Play();
            isRunSound = true;
        }
        if (rb.velocity.x == 0 || !isGrounded || !isSprinting)
        {
            runSoundEffect.Stop();
            isRunSound = false;
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            breezeSoundEffect.Stop();
            isJumpSound = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (!isJumpSound && (!isGrounded || !isWallSliding))
        {
            breezeSoundEffect.PlayDelayed(0.5f);
            isJumpSound = true;
        }

        if (isWallSliding && !isWallConnectSound)
        {
            wallConnectSoundEffect.Play();
            isWallConnectSound = true;
        }

        if (!isWallSliding)
        {
            wallConnectSoundEffect.Stop();
            isWallConnectSound = false;
        }

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpSoundEffect.Play();
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

        if ((Input.GetButtonUp("Jump") && rb.velocity.y > 0f) || (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f) || (Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0f))
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

        if (rb.velocity.x != 0 && !isSprinting)
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

            breezeSoundEffect.Stop();
            isJumpSound = false;
            //Debug.Log("Wall sliding");
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
        else if ((isWallSliding || isTouchingWall) && ((Input.GetButtonDown("Jump")) || Input.GetKeyDown(KeyCode.W) || movementInputDirection != 0)) //Wall jump
        {
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpSoundEffect.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
}
