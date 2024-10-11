using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Gameover()
    {
        anim.SetBool("Dead", true);
        
        SceneManager.LoadScene("Home");
    }
    

    // Fonction pour redémarrer le jeu
    void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    
}
