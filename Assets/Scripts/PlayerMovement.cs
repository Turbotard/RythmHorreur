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

    /// <summary>
    /// BPM initial du métronome.
    /// </summary>
    public float startBpm = 90f;

    /// <summary>
    /// Vitesse d'augmentation du BPM en BPM par seconde.
    /// </summary>
    public float bpmIncreaseRate = 0.5f;

    /// <summary>
    /// BPM maximal que le métronome peut atteindre.
    /// </summary>
    public float maxBpm = 180f;

    /// <summary>
    /// Fenêtre de tolérance en secondes pendant laquelle le joueur peut appuyer sur la touche.
    /// </summary>
    public float toleranceWindow = 0.5f;

    /// <summary>
    /// Source audio pour jouer le son du métronome à chaque battement.
    /// </summary>
    public AudioSource metronomeTickSound;

    /// <summary>
    /// Texte UI affichant le BPM actuel.
    /// </summary>
    public TMP_Text bpmText;

    /// <summary>
    /// Texte UI affichant le score du joueur.
    /// </summary>
    public TMP_Text scoreText;

    /// <summary>
    /// Indicateur visuel du battement, utilisé pour signaler au joueur l'état du métronome.
    /// </summary>
    public Image beatIndicator;

    /// <summary>
    /// Couleur de l'indicateur visuel lorsque le joueur appuie au bon moment.
    /// </summary>
    public Color correctColor = Color.green;

    /// <summary>
    /// Couleur de l'indicateur visuel lorsque le joueur appuie au mauvais moment ou rate le battement.
    /// </summary>
    public Color wrongColor = Color.red;

    /// <summary>
    /// Couleur par défaut de l'indicateur visuel entre les battements.
    /// </summary>
    public Color neutralColor = Color.white;

    private float currentBpm; // BPM actuel du métronome.
    private float interval; // Intervalle en secondes entre chaque battement du métronome.
    private float timer; // Timer pour suivre le temps écoulé entre les battements.
    private bool canPressKey; // Indique si le joueur peut appuyer sur la touche dans la fenêtre de tolérance.
    private bool hasPressedKey; // Indique si le joueur a appuyé sur la touche pendant la fenêtre de tolérance.
    private int score = 50; // Score actuel du joueur.
    private int minScore = 0;
    private int maxScore = 100;
    public static bool isGameOver;
    private int greenIntervale = 70; // Seuil de la zone verte
    private int yellowIntervale = 40; // Seuil de la zone jaune
    private int redIntervale = 20; // Seuil de la zone rouge

    // Référence au script Screamer pour gérer le screamer indépendamment.
    public Screamer screamer;

    private void Awake()
    {
        isGameOver = false;
    }

    void Start()
    {
        // Initialisation du BPM et du timer
        currentBpm = startBpm;
        interval = 60f / currentBpm;
        timer = 0f;
        canPressKey = false;
        hasPressedKey = false;

        // Définir la couleur initiale de l'indicateur
        if (beatIndicator != null)
        {
            beatIndicator.color = neutralColor;
        }

        // Mettre à jour les textes UI
        UpdateBpmText();
        UpdateScoreText();
    }

    void Update()
    {
        // Mise à jour du timer
        timer += Time.deltaTime;

        // Gérer les battements du métronome
        if (timer >= interval)
        {
            OnMetronomeBeat();
            timer -= interval;

            // Augmenter le BPM jusqu'à un maximum
            if (currentBpm < maxBpm)
            {
                currentBpm += bpmIncreaseRate * Time.deltaTime;
                currentBpm = Mathf.Min(currentBpm, maxBpm);
                interval = 60f / currentBpm;
            }

            // Autoriser l'appui sur une touche pendant la fenêtre de tolérance
            canPressKey = true;
            hasPressedKey = false;
            Invoke("CheckMissedKeyPress", toleranceWindow);
        }

        // Gérer l'appui sur les touches
        if (canPressKey)
        {
            if (Input.GetKeyDown(KeyCode.A) && lastKey == "d" || Input.GetKeyDown(KeyCode.A) && lastKey == "")
            {
                lastKey = "q";
                StepForward();
                Debug.Log(currentBpm);
                OnCorrectKeyPress();
            }
            else if (Input.GetKeyDown(KeyCode.D) && lastKey == "q")
            {
                lastKey = "d";
                StepForward();
                Debug.Log(currentBpm);
                OnCorrectKeyPress();
            }
            else if (!canPressKey && Input.GetKeyDown(KeyCode.Space))
            {
                OnIncorrectKeyPress();
            }

            if (isGameOver)
            {
                Gameover();
            }
            else if (!isGameOver)
            {
                Debug.Log(score);
            }
        }
    }

    // Déplace le joueur vers l'avant d'une certaine distance
    void StepForward()
    {
        playerRb.MovePosition(playerRb.position + new Vector2(stepDistance, 0));
    }

    // Appelé à chaque battement du métronome
    void OnMetronomeBeat()
    {
        // Jouer le son du métronome
        if (metronomeTickSound != null)
        {
            metronomeTickSound.Play();
        }

        // Remettre la couleur de l'indicateur à neutre
        if (beatIndicator != null)
        {
            beatIndicator.color = neutralColor;
        }

        // Mettre à jour l'affichage du BPM
        UpdateBpmText();
    }

    // Appelé lorsque le joueur appuie au bon moment
    void OnCorrectKeyPress()
    {
        score += 10;
        UpdateScoreText();

        // Changer la couleur de l'indicateur pour un appui correct
        if (beatIndicator != null)
        {
            beatIndicator.color = correctColor;
        }

        hasPressedKey = true;
        canPressKey = false;
    }

    // Appelé lorsque le joueur appuie au mauvais moment
    void OnIncorrectKeyPress()
    {
        score -= 2;
        UpdateScoreText();

        // Changer la couleur de l'indicateur pour un appui incorrect
        if (beatIndicator != null)
        {
            beatIndicator.color = wrongColor;
        }
    }

    // Vérifie si le joueur a manqué d'appuyer sur une touche
    void CheckMissedKeyPress()
    {
        if (!hasPressedKey)
        {
            score -= 5;
            UpdateScoreText();

            // Changer la couleur de l'indicateur pour un appui manqué
            if (beatIndicator != null)
            {
                beatIndicator.color = wrongColor;
            }
        }

        canPressKey = false;
    }

    // Met à jour le texte du BPM dans l'UI
    void UpdateBpmText()
    {
        if (bpmText != null)
        {
            bpmText.text = "BPM: " + Mathf.RoundToInt(currentBpm).ToString();
        }
    }

    // Met à jour le texte du score dans l'UI et le transmet au script Screamer
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        // Mettre à jour le score dans le script Screamer
        if (screamer != null)
        {
            screamer.UpdateScore(score);
        }
    }

    // Réinitialise le jeu en cas de game over
    void Gameover()
    {
        score = 50;
        currentBpm = startBpm;
        interval = 60f / currentBpm;
        timer = 0f;
        canPressKey = false;
        hasPressedKey = false;

        if (beatIndicator != null)
        {
            beatIndicator.color = neutralColor;
        }

        UpdateBpmText();
        UpdateScoreText();
    }

    // Fonction pour redémarrer le jeu
    void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
