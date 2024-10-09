using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player;                      // Référence au joueur
    public UserExperienceMetronome metronome;     // Référence à la classe UserExperienceMetronome pour récupérer le score
    public float smoothSpeed = 0.125f;            // Vitesse de lissage du mouvement de la caméra
    public float scoreTarget = 50f;               // Le score à partir duquel la caméra est centrée
    public float maxOffset = 5f;                  // Décalage maximal de la caméra
    private Vector3 offset = new Vector3(0, 0, -10); // Position initiale de la caméra par rapport au joueur

    void Update()
    {
        // Récupérer le score actuel du joueur depuis UserExperienceMetronome
        int playerScore = metronome.GetScore();

        // Calculer le décalage en fonction de la différence entre le score du joueur et le score cible
        float scoreDifference = playerScore - scoreTarget;

        // Calculer l'offset sur l'axe X, en fonction de la différence de score
        float xOffset = Mathf.Clamp(scoreDifference * 0.2f, -maxOffset, maxOffset);

        // Position cible de la caméra, avec décalage horizontal
        Vector3 desiredPosition = player.position + offset + new Vector3(xOffset, 0, 0);

        // Appliquer un mouvement lissé vers la position cible
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}