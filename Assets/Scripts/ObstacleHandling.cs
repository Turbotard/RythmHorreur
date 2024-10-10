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

    void Start()
    {
        if (playerRb == null)
        {
            playerRb = GetComponent<Rigidbody2D>();
        }
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

        // Vérifie si le saut est activé seulement lorsque le joueur est au sol
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            Debug.Log("Saut avec touche Espace");  // Pour s'assurer que le saut de base fonctionne encore
        }
    }

    void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(hitPoint, 0.1f);
        }
    }
}
