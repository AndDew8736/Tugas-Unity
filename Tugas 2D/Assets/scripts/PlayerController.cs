using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float moveInput;
    private Rigidbody2D rb;
    public float dashDistance;
    private float currentDashCooldown;
    public float dashCooldown;
    private float dashingTime = 0.2f;
    private bool canDash = true;
    private bool isDashing;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDashCooldown = dashCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        if(isDashing)
        {
             return;
        }
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        currentDashCooldown -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
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
}


