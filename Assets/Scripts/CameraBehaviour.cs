using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player;                      // Référence au joueur
    public float smoothSpeed = 0.125f;            // Vitesse de lissage du mouvement de la caméra
    public float maxScore = 100f;                 // Le score maximum
    public float minScore = 0f;                   // Le score minimum
    public float maxRightOffset = 5f;             // Distance maximale à droite de l'écran
    public float maxLeftOffset = 5f;              // Distance maximale à gauche de l'écran
    private Vector3 offset = new Vector3(0, 0, -10); // Position initiale de la caméra par rapport au joueur
    public PlayerMovement movement;               // Référence au mouvement du joueur

    void LateUpdate()
    {
        // Récupérer le score actuel du joueur
        float playerScore = movement.GetScore();

        // Mapper le score du joueur à une position horizontale inversée
        // Si le score est minimum, l'offset est maxRightOffset (la caméra est à droite)
        // Si le score est maximum, l'offset est maxLeftOffset (la caméra est à gauche)
        float horizontalOffset = Mathf.Lerp(maxRightOffset, -maxLeftOffset, Mathf.InverseLerp(minScore, maxScore, playerScore));

        // Position cible de la caméra avec un décalage en fonction du score
        Vector3 desiredPosition = player.position + offset + new Vector3(horizontalOffset, 0, 0);

        // Appliquer un mouvement lissé vers la position cible
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}