using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifiez si l'objet qui a heurté cet ennemi est le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("L'ennemi a touché le joueur !");
            PlayerMovement.isGameOver = true; // Définir isGameOver sur true
        }
    }

}
