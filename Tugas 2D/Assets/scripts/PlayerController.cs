using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //i should have separated these scripts...
    public float moveSpeed = 5f;
    private float moveInput;
    public float jumpForce;

    private Rigidbody2D rb;
    public float dashDistance;
    private float currentDashCooldown;
    public float dashCooldown;
    private float dashingTime = 0.2f;
    private bool canDash = true;
    private bool isDashing;
    private Animator anim;

    //States and anims
    private enum State { idle, running, jumping, falling, damaged};
    private State state = State.idle;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentDashCooldown = dashCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (state != State.damaged) //makes it so u  cant move when damaged
        {
            Movement();
        }
     
        
        DashCheck();
        AnimState();
        anim.SetInteger("state", (int)state);
    }
    private void Movement()
    {
        //Movement
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        transform.position += new Vector3(moveInput, 0, 0) * Time.deltaTime * moveSpeed;

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        //Rotation
        if (!Mathf.Approximately(0, moveInput))
        {
            transform.rotation = moveInput < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }

        //Dashing
        currentDashCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }
    private bool isGrounded()//groundcheck
    {
        return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;
    }

    //cherry function
    [SerializeField] private int Cherries = 0;
    [SerializeField] private UnityEngine.UI.Text CherryNum;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            Cherries += 1;
            CherryNum.text = Cherries.ToString();
        }
    }

    private void DashCheck()//checks for dashing
    {
        currentDashCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash() //dash function
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(moveInput * dashDistance, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    //Collision with the enemy
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Mob")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>(); //gets enemy ai script
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.damaged;
                if (other.gameObject.transform.position.x > transform.position.x) //pushes player left if enemy is on right
                {
                    rb.velocity = new Vector2(-8f, rb.velocity.y);
                }
                else //pushes to right if enemy is on left
                {
                    rb.velocity = new Vector2(8f, rb.velocity.y);
                }
            }

            //destroys the object with tag gameObject;
        }
    }
    private void AnimState()//changes animation states of player
    {
        if (state == State.jumping)
        {
            //if jumping
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (isGrounded())
            {
                state = State.idle;
            }
        }
        else if (state == State.damaged)
        {
            if (Math.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //if movement greater than e, moving
            state = State.running;
        }

        else
        {
            state = State.idle;
        }
    }
}


