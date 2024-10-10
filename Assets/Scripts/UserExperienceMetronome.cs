using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Gère le métronome et l'expérience utilisateur autour du rythme.
/// Le joueur doit appuyer sur une touche en rythme avec le métronome.
/// Le BPM augmente progressivement, et les actions du joueur sont évaluées.
/// </summary>
public class UserExperienceMetronome : MonoBehaviour
{
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
    public float toleranceWindow = 25f;

    /// <summary>
    /// Source audio pour jouer le son du métronome à chaque battement.
    /// </summary>
    public AudioSource metronomeTickSound;

    /// <summary>
    /// Texte UI affichant le BPM actuel.
    /// </summary>
    public TMP_Text bpmText;

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

    [SerializeField]
    private UnityEvent OnBeat;
    
    /// <summary>
    /// Initialise les variables et configure l'interface utilisateur au démarrage du script.
    /// </summary>
    void Start()
    {
        currentBpm = startBpm;
        interval = 60f / currentBpm;
        timer = 0f;
        

        if (beatIndicator != null)
        {
            beatIndicator.color = neutralColor;
        }

        UpdateBpmText();
    }

    /// <summary>
    /// Met à jour le timer et vérifie si un battement doit être déclenché.
    /// Gère également la détection d'appuis corrects ou manqués.
    /// </summary>
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

            
        }
        
    }

    public bool isOnTime()
    {
        Debug.Log("sur le temps");
        return timer < toleranceWindow || interval - timer < toleranceWindow;
    }
    
    /// <summary>
    /// Appelé à chaque battement du métronome.
    /// Joue le son du battement, met à jour l'indicateur visuel et le texte du BPM.
    /// </summary>
    void OnMetronomeBeat()
    {

        if (metronomeTickSound != null)
        {
            metronomeTickSound.Play();
        }

        if (beatIndicator != null)
        {
            beatIndicator.color = neutralColor;
        }

        UpdateBpmText();
        
        OnBeat.Invoke();
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


}