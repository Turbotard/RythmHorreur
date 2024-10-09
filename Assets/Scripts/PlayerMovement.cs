using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Composant Rigidbody2D pour déplacer le joueur
    public Rigidbody2D playerRb;
    public float input;
    public float stepDistance; // Distance parcourue à chaque "pas"
    private string lastKey = ""; // Stocke la dernière touche appuyée
    public static bool isGameOver;
    public Screamer screamer; // Référence au script Screamer pour gérer le screamer indépendamment.
    public UserExperienceMetronome metronome;
    public GameOver gameover;
    private void Awake()
    {
        isGameOver = false;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && lastKey == "d" || Input.GetKeyDown(KeyCode.A) && lastKey == "")
        {
            lastKey = "q";
            StepForward();
            metronome.OnCorrectKeyPress();
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastKey == "q")
        {
            lastKey = "d";
            StepForward();
            metronome.OnCorrectKeyPress();
        }
        if (isGameOver)
        {
            gameover.GameOver();
        }
    }

    // Déplace le joueur vers l'avant d'une certaine distance
    void StepForward()
    {
        playerRb.MovePosition(playerRb.position + new Vector2(stepDistance, 0));
    }

    


}
