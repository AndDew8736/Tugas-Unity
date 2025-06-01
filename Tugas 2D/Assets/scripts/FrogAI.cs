using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrogAI : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 20f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    private bool facingLeft = true;
    protected override void Start() //overrides enemy start func
    {
        base.Start();//accesses base start from the enemy start func
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (anim.GetBool("Jumping"))
            {
                if (rb.velocity.y < .1)
                {
                    anim.SetBool("Falling", true);
                    anim.SetBool("Jumping", false);
                }
            }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }

    }
    private void Move()
    {
        if (facingLeft)
        {
            //if jumping left wouldnt go off the edge
            if (transform.position.x > leftCap)
            {
                //makes sure sprite faces right
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //if its grounded, jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            //if jumping left wouldnt go off the edge
            if (transform.position.x < rightCap)
            {
                //makes sure sprite faces right
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                //if its grounded, jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
