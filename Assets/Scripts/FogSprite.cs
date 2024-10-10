using UnityEngine;

public class FogSpriteFollower : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public Vector3 offset = new Vector3(0, 0, -1); // Offset pour positionner le brouillard derrière le joueur

    void Update()
    {
        // Le brouillard suit la position du joueur avec un léger décalage
        transform.position = player.position + offset;
    }
}