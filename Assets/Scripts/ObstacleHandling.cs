using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandling : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float groundCheckDistance = 5.0f;
    public float jumpForce = 10f; 
    public LayerMask groundLayer;
    private bool isGrounded;
    private Vector2 hitPoint;
    private Animator anim;
    
    void Start()
    {
        if (playerRb == null)
        {
            playerRb = GetComponent<Rigidbody2D>();
        }
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
        if (isGrounded)
        {
            hitPoint = hit.point;
        }

        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);

        if (isGrounded)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
        anim.SetBool("Grounded", isGrounded);

    }

    void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(hitPoint, 0.1f);
        }
    }

    public bool isGroundedMethod()
    {
        return isGrounded;
    }
}
