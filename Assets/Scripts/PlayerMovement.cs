using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;

    [SerializeField] private int runSpeed;

    [SerializeField] private int jumpSpeed;

    [SerializeField] private int climbSpeed;

    [SerializeField] private GameObject bullet;

    public Transform gunTransform;

    private Animator playerAnimator;

    private Rigidbody2D _rb;

    private CapsuleCollider2D playerCollider;
    private BoxCollider2D feetColliderOfPlayer;

    private bool _isAlive;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetColliderOfPlayer = GetComponent<BoxCollider2D>();
        _isAlive = true;
    }

    void Update()
    {
        if(!_isAlive){return;}
        Run();
        FlipSprite();
        ClimbStairs();
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!_isAlive){return;}
        moveInput = value.Get<Vector2>(); // we are getting Horizontal input of use movement
    }
    
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, _rb.velocity.y);// we will give a velocity our rb
        _rb.velocity = playerVelocity;
        bool playerHasMovement = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon; // if player has a movement in horizontal, we can change localscale of character and flip the character.
        if (playerHasMovement)
        {
            playerAnimator.SetBool("isRunning",true);// we can access bool value that in the animator with SetBool()

        }
        else
        {
            playerAnimator.SetBool("isRunning",false);

        }
    }

    void ClimbStairs()
    {

        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Stairs")))
        {
            Vector2 climbVelocity = new Vector2(_rb.velocity.x, moveInput.y * climbSpeed); // we will give a velocity our rb
            _rb.velocity = climbVelocity;
            _rb.gravityScale = 0;
            playerAnimator.SetBool("isClimbing",true);
            bool playerHasYMovement = Mathf.Abs(_rb.velocity.y) > Mathf.Epsilon;
            if (!playerHasYMovement)
            {
                playerAnimator.SetBool("isClimbing",false);
            }
        }
        else
        {
            _rb.gravityScale = 2.5f;
            playerAnimator.SetBool("isClimbing",false);
        }

    }

    void OnJump(InputValue value)
    {
        if(!_isAlive){return;}
        if (value.isPressed && feetColliderOfPlayer.IsTouchingLayers(LayerMask.GetMask("Ground"))) // if space button is pressed and player on ground
        {
            _rb.velocity += new Vector2(0f, jumpSpeed); // we gave jump power
            
            playerAnimator.SetBool("isJumping",true);
            
        }
        

    }

    void OnFire(InputValue value)
    {
        if(!_isAlive){return;}
        if (value.isPressed)
        {
            Instantiate(bullet, gunTransform.position, transform.rotation);
        }
    }

    void FlipSprite()
    {
        bool playerHasMovement = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon; // if player has a movement in horizontal, we can change localscale of character and flip the character.
        if (playerHasMovement)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rb.velocity.x), 1f);

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Stairs") )
        {
            playerAnimator.SetBool("isJumping",false);

        }
        {
            
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazard")))
        {
            _isAlive = false;
            playerAnimator.SetTrigger("Dying");
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    
    
}
