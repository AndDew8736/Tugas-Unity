using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    public float fireballCooldown;
    private float currentFbCooldown;

    public GameObject ProjectilePrefab;
    public Transform LaunchOffset;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        transform.position += new Vector3(moveInput, 0, 0) * Time.deltaTime * moveSpeed;
        
        if (!Mathf.Approximately(0, moveInput))
        {
            transform.rotation = moveInput > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }

        currentDashCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        if (Input.GetKeyDown(KeyCode.E) && currentFbCooldown <= 0)
        {
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            currentFbCooldown = fireballCooldown;
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


