using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
   
    public void Gameover()
    {
        SceneManager.LoadScene("Home");
    }
    

    // Fonction pour redémarrer le jeu
    void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    
}
