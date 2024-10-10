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
    private bool canPressKey;  // Indicate si le joueur peut appuyer sur la touche dans la fenêtre de tolérance.
    private bool hasPressedKey;// Indique si le joueur a appuyé sur la touche pendant la fenêtre de tolérance.
    private int score = 50;
    public GameOver gameover;
    
    /// <summary>
    /// Texte UI affichant le score du joueur.
    /// </summary>
    public TMP_Text scoreText;// Score actuel du joueur.
    //public GameOver gameover;
    private void Awake()
    {
        isGameOver = false;
    }

    void Start()
    {
        canPressKey = false;
        hasPressedKey = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (lastKey == "d" || lastKey == "")
            {
                if (metronome.isOnTime())
                {
                    OnCorrectKeyPress();
                    lastKey = "q";
                }
                else
                {
                    OnIncorrectKeyPress();
                }
            }
            else
            {
                OnIncorrectKeyPress();
            }
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            if (lastKey == "q")
            {
                if (metronome.isOnTime())
                {
                    OnCorrectKeyPress();
                    lastKey = "d";
                }
                else
                {
                    OnIncorrectKeyPress();
                }
            }
            else
            {
                OnIncorrectKeyPress();
            }
        }

        if (score <= 0)
        {
            isGameOver = true;
        }
        
        if (isGameOver)
        {
           gameover.Gameover();
        }
        
        UpdateScoreText();

    }

    // Déplace le joueur vers l'avant d'une certaine distance
    void StepForward()
    {
        playerRb.MovePosition(playerRb.position + new Vector2(stepDistance, 0));
    }

    /// <summary>
    /// Appelé lorsque le joueur appuie sur la touche au bon moment.
    /// Augmente le score et change la couleur de l'indicateur visuel.
    /// </summary>
    public void OnCorrectKeyPress()
    {
        Debug.Log("Touche appuyée au bon moment !");
        score += 10;
        UpdateScoreText();
        StepForward();

        if (metronome.beatIndicator != null)
        {
            metronome.beatIndicator.color = metronome.correctColor;
        }

        hasPressedKey = true;
    }

    /// <summary>
    /// Appelé lorsque le joueur appuie sur la touche en dehors de la fenêtre de tolérance.
    /// Réduit le score et change la couleur de l'indicateur visuel.
    /// </summary>
    void OnIncorrectKeyPress()
    {
        Debug.Log("Touche appuyée au mauvais moment !");
        score -= 2;
        UpdateScoreText();

        if (metronome.beatIndicator != null)
        {
            metronome.beatIndicator.color = metronome.wrongColor;
        }

        hasPressedKey = true;
    }

    /// <summary>
    /// Vérifie si le joueur n'a pas appuyé sur la touche pendant la fenêtre de tolérance.
    /// Si le joueur n'a pas appuyé, une pénalité est appliquée au score.
    /// </summary>
    public void CheckMissedKeyPress()
    {
        if (!hasPressedKey)
        {
            Debug.Log("Touche manquée !");
            score -= 5;
            UpdateScoreText();

            if (metronome.beatIndicator != null)
            {
                metronome.beatIndicator.color = metronome.wrongColor;
            }
        }

        hasPressedKey = false;
    }
    
    /// <summary>
    /// Met à jour l'affichage du score dans l'interface utilisateur.
    /// </summary>
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
    
    public int GetScore()
    {
        return score;
    }
}
