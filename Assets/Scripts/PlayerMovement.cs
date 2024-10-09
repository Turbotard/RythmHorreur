using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Composant Rigidbody2D pour déplacer le joueur
    public Rigidbody2D playerRb;
    public float input;
    public float stepDistance;   // Distance parcourue à chaque "pas"
    private string lastKey = ""; // Stocke la dernière touche appuyée

    // Paramètres du métronome
    public float startBpm = 90f; // BPM initial du métronome
    public float bpmIncreaseRate = 0.5f; // Vitesse d'augmentation du BPM
    public float maxBpm = 180f; // BPM maximal
    public float toleranceWindow = 0.5f; // Fenêtre de tolérance pour les touches

    // Audio et UI
    public AudioSource metronomeTickSound; // Son du métronome
    public TMP_Text bpmText; // Texte UI affichant le BPM
    public TMP_Text scoreText; // Texte UI affichant le score
    public Image beatIndicator; // Indicateur visuel du battement
    public Color correctColor = Color.green; // Couleur pour un appui correct
    public Color wrongColor = Color.red; // Couleur pour un appui incorrect
    public Color neutralColor = Color.white; // Couleur par défaut de l'indicateur

    // Variables pour la gestion du BPM et des battements
    private float currentBpm; // BPM actuel
    private float interval; // Intervalle entre les battements
    private float timer; // Timer pour le suivi des battements
    private bool canPressKey; // Indique si le joueur peut appuyer sur une touche
    private bool hasPressedKey; // Indique si le joueur a appuyé sur une touche
    private int score = 60; // Score initial du joueur
    private int minScore = 0; // Score minimum
    private int maxScore = 100; // Score maximum
    private int greenIntervale = 70; // Seuil de la zone verte
    private int yellowIntervale = 40; // Seuil de la zone jaune
    private int redIntervale = 20; // Seuil de la zone rouge

    // Pour le flash
    public Canvas flashImage; // Image pour le flash blanc
    public float flashDuration = 0.5f; // Durée du flash en secondes
    private bool isFlashing = false; // Indique si le flash est actif

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

        // Démarrer la coroutine pour gérer les chances de flash
        StartCoroutine(CheckFlashChance());
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
            if (Input.GetKeyDown(KeyCode.A) && (lastKey == "d" || lastKey == ""))
            {
                lastKey = "q";
                StepForward();
                OnCorrectKeyPress();
            }
            else if (Input.GetKeyDown(KeyCode.D) && lastKey == "q")
            {
                lastKey = "d";
                StepForward();
                OnCorrectKeyPress();
            }
            else if (!canPressKey && Input.GetKeyDown(KeyCode.Space))
            {
                OnIncorrectKeyPress();
            }

            // Vérifier la condition de game over
            if (score <= minScore)
            {
                Gameover();
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

    // Met à jour le texte du score dans l'UI
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
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

    // Coroutine pour vérifier les chances de déclencher un flash
    IEnumerator CheckFlashChance()
    {
        while (true)
        {
            // Zone rouge : vérifier toutes les 3 secondes avec une chance de 50 %
            if (score < redIntervale)
            {
                yield return new WaitForSeconds(3f);
                if (Random.value < 0.5f && !isFlashing)
                {
                    StartCoroutine(Flash());
                }
            }
            // Zone jaune : vérifier toutes les 10 secondes avec une chance de 25 %
            else if (score < yellowIntervale)
            {
                yield return new WaitForSeconds(10f);
                if (Random.value < 0.25f && !isFlashing)
                {
                    StartCoroutine(Flash());
                }
            }
            // Zone verte : aucun flash
            else
            {
                yield return null;
            }
        }
    }

    // Coroutine pour afficher le flash
    IEnumerator Flash()
    {
        isFlashing = true;
        flashImage.gameObject.SetActive(true); // Afficher l'image de flash
        yield return new WaitForSeconds(flashDuration);
        flashImage.gameObject.SetActive(false);// Cacher l'image de flash
        isFlashing = false;
    }
}
