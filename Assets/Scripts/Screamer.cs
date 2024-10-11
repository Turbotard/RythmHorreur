using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screamer : MonoBehaviour
{
    /// <summary>
    /// Le score actuel du joueur, mis à jour depuis PlayerMovement.
    /// </summary>
    public float playerScore= 50;

    /// <summary>
    /// Seuils pour déclencher les événements de screamer.
    /// </summary>
    public int redIntervale = 20; // Seuil de la zone rouge
    public int yellowIntervale = 50; // Seuil de la zone jaune

    /// <summary>
    /// Image utilisée pour le screamer.
    /// </summary>
    public Canvas flashImage;

    /// <summary>
    /// Durée du flash de screamer.
    /// </summary>
    public float flashDuration = 0.5f;

    /// <summary>
    /// Liste de sons pour la zone rouge.
    /// </summary>
    public List<AudioClip> redZoneSounds;

    /// <summary>
    /// Liste de sons pour la zone jaune.
    /// </summary>
    public List<AudioClip> yellowZoneSounds;

    /// <summary>
    /// AudioSource pour jouer les sons.
    /// </summary>
    private AudioSource audioSource;

    private bool isFlashing = false;

    public PlayerMovement playerMovement;

    private void Start()
    {
        // Récupérer le score actuel du joueur depuis le script PlayerMovement.
        playerScore = playerMovement.GetScore();
        // Démarrer la coroutine pour gérer les chances de screamer.
        StartCoroutine(CheckFlashChance());

        // Récupérer l'AudioSource attachée au GameObject.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Aucun AudioSource trouvé sur le GameObject. Veuillez en ajouter un.");
        }
    }

    private void Update()
    {
        // Mettre à jour le score depuis le script PlayerMovement.
        if (playerMovement != null)
        {
            playerScore = playerMovement.GetScore();
        }
        else
        {
            Debug.LogError("Aucun script PlayerMovement trouvé. Veuillez ajouter un script PlayerMovement au GameObject.");
        }
    }

    // Coroutine pour vérifier les chances de déclencher un flash de screamer.
    IEnumerator CheckFlashChance()
    {
        while (true)
        {
            // Zone jaune : vérifier toutes les 10 secondes avec une chance de 25 %.
            if (playerScore > redIntervale && playerScore <= yellowIntervale)
            {
                yield return new WaitForSeconds(3f);
                if (UnityEngine.Random.value < 0.33f && !isFlashing)
                {
                    StartCoroutine(Flash(yellowZoneSounds));
                }
            }
            // Zone rouge : vérifier toutes les 3 secondes avec une chance de 50 %.
            else if (playerScore >= 0f && playerScore < redIntervale)
            {
                yield return new WaitForSeconds(2f);
                if (UnityEngine.Random.value < 0.5f && !isFlashing)
                {
                    StartCoroutine(Flash(redZoneSounds));
                }
            }

            // Zone verte : aucun flash.
            else
            {
                yield return null;
            }
        }
    }

    // Coroutine pour afficher le flash de screamer et jouer un son.
    IEnumerator Flash(List<AudioClip> sounds)
    {
        isFlashing = true;

        // Afficher le screamer.
        flashImage.gameObject.SetActive(true);

        // Jouer un son aléatoire depuis la liste fournie.
        if (sounds.Count > 0 && audioSource != null)
        {
            AudioClip randomSound = sounds[UnityEngine.Random.Range(0, sounds.Count)];
            audioSource.PlayOneShot(randomSound);
        }

        // Attendre la durée du flash.
        yield return new WaitForSeconds(flashDuration);

        // Masquer le screamer.
        flashImage.gameObject.SetActive(false);

        isFlashing = false;
    }
}
