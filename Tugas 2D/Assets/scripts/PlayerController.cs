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
    public float fireballCooldown;
    private float currentFbCooldown;

    public GameObject ProjectilePrefab;
    public Transform LaunchOffset;
    private Animator anim;

    //States and anims
    private enum State { idle, running, jumping, falling};
    private State state = State.idle;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentDashCooldown = dashCooldown;
        currentFbCooldown = fireballCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        Movement();
        DashCheck();
        Fireball();
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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
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

    private void Fireball()//fireball function
    {
         if (Input.GetKeyDown(KeyCode.E) && currentFbCooldown <= 0)
        {
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            currentFbCooldown = fireballCooldown;
        }
        currentFbCooldown -= Time.deltaTime;
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


