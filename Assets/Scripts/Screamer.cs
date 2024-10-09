using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Screamer : MonoBehaviour
{
    /// <summary>
    /// Le score actuel du joueur, mis à jour depuis PlayerMovement.
    /// </summary>
    public int playerScore;

    /// <summary>
    /// Seuils pour déclencher les événements de screamer.
    /// </summary>
    public int redIntervale = 20; // Seuil de la zone rouge
    public int yellowIntervale = 40; // Seuil de la zone jaune

    /// <summary>
    /// Image utilisée pour le screamer.
    /// </summary>
    public Canvas flashImage;

    /// <summary>
    /// Durée du flash de screamer.
    /// </summary>
    public float flashDuration = 0.5f;

    private bool isFlashing = false;

    private void Start()
    {
        // Démarrer la coroutine pour gérer les chances de screamer.
        StartCoroutine(CheckFlashChance());
    }

    // Fonction pour mettre à jour le score depuis PlayerMovement
    public void UpdateScore(int newScore)
    {
        playerScore = newScore;
    }

    // Coroutine pour vérifier les chances de déclencher un flash de screamer.
    IEnumerator CheckFlashChance()
    {
        while (true)
        {
            // Zone rouge : vérifier toutes les 3 secondes avec une chance de 50 %.
            if (playerScore >= 0 && playerScore < redIntervale)
            {
                yield return new WaitForSeconds(3f);
                if (UnityEngine.Random.value < 0.5f && !isFlashing)
                {
                    StartCoroutine(Flash());
                }
            }
            // Zone jaune : vérifier toutes les 10 secondes avec une chance de 25 %.
            else if (playerScore >= redIntervale && playerScore < yellowIntervale)
            {
                yield return new WaitForSeconds(10f);
                if (UnityEngine.Random.value < 0.25f && !isFlashing)
                {
                    StartCoroutine(Flash());
                }
            }
            // Zone verte : aucun flash.
            else
            {
                yield return null;
            }
        }
    }

    // Coroutine pour afficher le flash de screamer.
    IEnumerator Flash()
    {
        isFlashing = true;
        flashImage.gameObject.SetActive(true); // Afficher le screamer.
        yield return new WaitForSeconds(flashDuration);
        flashImage.gameObject.SetActive(false); // Masquer le screamer.
        isFlashing = false;
    }
}
