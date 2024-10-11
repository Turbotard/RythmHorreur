using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public PlayerMovement player;
    public int malusHp = 30;
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object that collided with this is the player or enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerPosition = collision.transform.position;
            Vector3 obstaclePosition = transform.position;
            
            // Vérifiez si le joueur touche l'obstacle par le haut
            if (playerPosition.x < obstaclePosition.x + 0.25f) // Le joueur est à gauche de l'obstacle
            {
                Destroy(gameObject); // Détruire l'obstacle
                player.SetScore(- malusHp);
            }
            else
            {
                Destroy(gameObject, 2f);
            }
            
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Détruire l'obstacle
        }
        
    }
}