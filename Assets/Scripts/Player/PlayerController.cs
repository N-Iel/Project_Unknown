using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script will manage all the inputs and animations related with movement
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region variables
    [Header("Movement")]
    public float speed;
    public float acceleration;  // Time that the player will need to reach topSpeed
    public float jumpForce;     
    float h;
    bool isJumping;
    bool isJumpingReleased;
    bool canMove;               
    Rigidbody2D rb;             

    [Header("Anim")]
    SpriteRenderer spriteRenderer;
    Animator animator;

    [Header("Raycast")]
    public Transform groundCheck;   // Origin point for raycast
    public LayerMask groundLayer;   // Ground layer
    public float rayLength;         // Length of the ray
    public bool isGrounded;         // var that store if the player is or not in ground
    #endregion

    #region Life Cycle

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player Input
        MovementInput();
        JumpInput();

        // Non Physics Movement
        IsGrounded();

        // Animations
        Flip();
        Animate();
        //Animating();
    }

    // Manage the physics updates
    private void FixedUpdate()
    {
        // Movement
        Move();

        // Movement released
        if (h == 0) ReleaseMove();

        // Jump
        if (isJumping) Jump();

        // Jump Released
        if (isJumpingReleased) ReleaseJump();
    }

    #endregion

    #region Input

    // It will get the player horizontal input and update the values
    void MovementInput()
    {
        h = Input.GetAxisRaw("Horizontal");
    }

    // It will get the player jump input and update the values
    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            isJumping = true;

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
            isJumpingReleased = true;
    }

    #endregion

    #region Movement

    // It will apply the input to the player movement usign the targetVelocity
    void Move()
    {
        rb.velocity = new Vector2( h * speed , rb.velocity.y);
    }

    // It will forcly stop the horizontal movement
    void ReleaseMove()
    {
        rb.velocity = new Vector2( 0 , rb.velocity.y);
    }

    // It will update the vertical speed of the player
    void Jump()
    {
        isJumping = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // It will forcly stop the vertical movement
    void ReleaseJump()
    {
        isJumpingReleased = false;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    #endregion

    #region Check

    // It will check if the player is touching the ground
    void IsGrounded()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * rayLength, Color.red);
    }

    #endregion

    #region Animations

    // It will get the movement dir and flipping the player sprite in consecuence
    void Flip()
    {
        if (h < 0) spriteRenderer.flipX = true;
        if (h > 0) spriteRenderer.flipX = false;
    }

    // It will trigger moving animation
    void Animate()
    {
        // Moving animation
        animator.SetBool("isMoving", h != 0);

        // Jumping animation
        animator.SetBool("isJumping", !isGrounded);

        // Falling animation
        animator.SetBool("isFalling", rb.velocity.y < 0.01);
    }
    #endregion

    #region Other
    #endregion
}
