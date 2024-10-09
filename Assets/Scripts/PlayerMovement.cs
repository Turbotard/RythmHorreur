using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float input;
    public float stepDistance;   // Distance parcourue à chaque "pas"
    private string lastKey = "";      // Stocke la dernière touche appuyée
    /// <summary>
    /// BPM initial du métronome.
    /// </summary>
    private Animator anim;
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

    private float currentBpm;  // BPM actuel du métronome.
    private float interval;    // Intervalle en secondes entre chaque battement du métronome.
    private float timer;       // Timer pour suivre le temps écoulé entre les battements.
    private bool canPressKey;  // Indicate si le joueur peut appuyer sur la touche dans la fenêtre de tolérance.
    private bool hasPressedKey;// Indique si le joueur a appuyé sur la touche pendant la fenêtre de tolérance.
    private int score = 50;     // Score actuel du joueur.
    private int minScore = 0;
    private int maxScore = 100;
    
    // Start is called before the first frame update
    void Start()
    {
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
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            OnMetronomeBeat();
            timer -= interval;

            if (currentBpm < maxBpm)
            {
                currentBpm += bpmIncreaseRate * Time.deltaTime;
                currentBpm = Mathf.Min(currentBpm, maxBpm);
                interval = 60f / currentBpm;
            }

            canPressKey = true;
            hasPressedKey = false;
            Invoke("CheckMissedKeyPress", toleranceWindow);
        }

        if (canPressKey)
        {
            if (Input.GetKeyDown(KeyCode.A) && lastKey == "d" || Input.GetKeyDown(KeyCode.A) && lastKey == ""){
            lastKey = "q";
            StepForward();
            Debug.Log("HellYes !");
            OnCorrectKeyPress();
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastKey == "q"){
            lastKey = "d";
            StepForward();
            Debug.Log("Yes !");
            OnCorrectKeyPress();
        }
        else if (!canPressKey && Input.GetKeyDown(KeyCode.Space))
        {
            OnIncorrectKeyPress();
        }
        if (score <= minScore){
            Gameover();
        }    
        else if(score >= minScore)
        {
            Debug.Log(score);
        }    
        
    }
    }

    // Fonction pour avancer d'un pas
    void StepForward()
    {
        Debug.Log("StepForward called");
        // Déplace le personnage vers l'avant d'une certaine distance
        playerRb.MovePosition(playerRb.position + new Vector2(stepDistance, 0));
    }

     void OnMetronomeBeat()
    {
        Debug.Log("Battement du métronome à " + currentBpm + " BPM");

        if (metronomeTickSound != null)
        {
            metronomeTickSound.Play();
        }

        if (beatIndicator != null)
        {
            beatIndicator.color = neutralColor;
        }

        UpdateBpmText();
    }

    /// <summary>
    /// Appelé lorsque le joueur appuie sur la touche au bon moment.
    /// Augmente le score et change la couleur de l'indicateur visuel.
    /// </summary>
    void OnCorrectKeyPress()
    {
        Debug.Log("Touche appuyée au bon moment !");
        score += 10;
        UpdateScoreText();

        if (beatIndicator != null)
        {
            beatIndicator.color = correctColor;
        }

        hasPressedKey = true;
        canPressKey = false;
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

        if (beatIndicator != null)
        {
            beatIndicator.color = wrongColor;
        }
    }

    /// <summary>
    /// Vérifie si le joueur n'a pas appuyé sur la touche pendant la fenêtre de tolérance.
    /// Si le joueur n'a pas appuyé, une pénalité est appliquée au score.
    /// </summary>
    void CheckMissedKeyPress()
    {
        if (!hasPressedKey)
        {
            Debug.Log("Touche manquée !");
            score -= 5;
            UpdateScoreText();

            if (beatIndicator != null)
            {
                beatIndicator.color = wrongColor;
            }
        }

        canPressKey = false;
    }

    /// <summary>
    /// Met à jour l'affichage du BPM dans l'interface utilisateur.
    /// </summary>
    void UpdateBpmText()
    {
        if (bpmText != null)
        {
            bpmText.text = "BPM: " + Mathf.RoundToInt(currentBpm).ToString();
        }
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

    void Gameover(){
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

    void RestartGame(){
        
    }

}