using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public PlayerMovement player;
    private int malusHp = 30;
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object that collided with this is the player or enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerPosition = collision.transform.position;
            Vector3 obstaclePosition = transform.position;
            
            Debug.Log("Player X : " + playerPosition.x + "Player Y : "+ playerPosition.y + " Obstacle : " + obstaclePosition);
            // Vérifiez si le joueur touche l'obstacle à partir de la gauche
            if (playerPosition.y > obstaclePosition.y + 0.25f)// Le joueur est au-dessus de l'obstacle
            {
                Debug.Log("haut");
               
            }
            // Vérifiez si le joueur touche l'obstacle par le haut
            else if (playerPosition.x < obstaclePosition.x) // Le joueur est à gauche de l'obstacle
            {
                Debug.Log("gauche.");
                Destroy(gameObject); // Détruire l'obstacle
                player.SetScore(- malusHp);
                Debug.Log(player.GetScore());
            }
            
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Détruire l'obstacle
        }
        
    }
}