using UnityEngine;

public class PlayerSpeedController : MonoBehaviour
{
    public float baseSpeed = 1f; // Vitesse de base du joueur
    public float scoreMultiplier = 0.15f; // Multiplicateur qui ajuste la vitesse en fonction du score
    private float score = 50f; // Score initial du joueur

    /// <summary>
    /// Calcule et retourne la vitesse ajustée en fonction du score actuel.
    /// </summary>
    public float GetAdjustedSpeed()
    {
        Debug.Log("Vitesse ajustée appelée : " + (baseSpeed + (score - 50f) * scoreMultiplier));
        return baseSpeed + (score - 50f) * scoreMultiplier;
    }
}