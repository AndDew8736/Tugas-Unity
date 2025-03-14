using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance;
    public float dashCooldown;
    private float dashingTime = 0.2f;
    private float currentDashCooldown;
    private bool canDash = true;
    private bool isDashing;
    private float moveInput;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDashCooldown = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing)
        {
             return;
        }

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
