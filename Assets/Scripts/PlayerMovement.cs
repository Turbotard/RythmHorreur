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
    public PlayerSpeedController speedController;
    public float stepDistance; // Distance parcourue à chaque "pas"
    private string lastKey = ""; // Stocke la dernière touche appuyée
    public static bool isGameOver;
    public Screamer screamer; // Référence au script Screamer pour gérer le screamer indépendamment.
    public UserExperienceMetronome metronome;
    private bool hasPressedKey; // Indique si le joueur a appuyé sur la touche pendant la fenêtre de tolérance.
    private float score = 50f;
    public GameOver gameover;

    public TMP_Text scoreText; // Texte UI affichant le score

    private void Awake()
    {
        isGameOver = false;
    }

    void Start()
    {
        hasPressedKey = false;
    }

    void Update()
    {
        // Le joueur avance tout seul avec une vitesse ajustée selon son score

        // Gestion des touches A et D pour avancer en rythme
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (lastKey == "d" || lastKey == "")
            {
                if (metronome != null && metronome.isOnTime())
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
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (lastKey == "q")
            {
                if (metronome != null && metronome.isOnTime())
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
        
        StepForwardConstantly();

        if (score <= 0f)
        {
            isGameOver = true;
        }

        if (isGameOver && gameover != null)
        {
            gameover.Gameover();
        }

        UpdateScoreText();
    }

    // Déplace le joueur constamment à une vitesse ajustée selon son score
    void StepForwardConstantly()
    {
        if (playerRb != null && speedController != null)
        {
            float adjustedSpeed = speedController.GetAdjustedSpeed();
            playerRb.MovePosition(playerRb.position + new Vector2(adjustedSpeed * Time.deltaTime, 0));
        }
    }

    // Appelé lorsque le joueur appuie sur la touche au bon moment
    public void OnCorrectKeyPress()
    {
        Debug.Log("Touche appuyée au bon moment !");
        score += 10f;
        UpdateScoreText();

        if (metronome.beatIndicator != null)
        {
            metronome.beatIndicator.color = metronome.correctColor;
        }
        hasPressedKey = true;
    }

    // Appelé lorsque le joueur appuie sur la touche en dehors de la fenêtre de tolérance
    void OnIncorrectKeyPress()
    {
        Debug.Log("Touche appuyée au mauvais moment !");
        score -= 2f;
        UpdateScoreText();

        if (metronome.beatIndicator != null)
        {
            metronome.beatIndicator.color = metronome.wrongColor;
        }
        hasPressedKey = true;
    }

    // Vérifie si le joueur n'a pas appuyé sur la touche pendant la fenêtre de tolérance
    public void CheckMissedKeyPress()
    {
        if (!hasPressedKey)
        {
            Debug.Log("Touche manquée !");
            score -= 5f;
            UpdateScoreText();

            if (metronome.beatIndicator != null)
            {
                metronome.beatIndicator.color = metronome.wrongColor;
            }
        }

        hasPressedKey = false;
    }

    // Met à jour l'affichage du score dans l'interface utilisateur
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public float GetScore()
    {
        return score;
    }
}