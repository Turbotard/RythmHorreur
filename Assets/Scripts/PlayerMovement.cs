using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float speed;
    public float input;
    public float stepDistance;   // Distance parcourue à chaque "pas"
    private string lastKey = "";      // Stocke la dernière touche appuyée
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerRb == null)
        {
            playerRb = GetComponent<Rigidbody2D>();
        }
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && lastKey == "d" || Input.GetKeyDown(KeyCode.A) && lastKey == ""){
            lastKey = "q";
            StepForward();
            Debug.Log("HellYes !");
        }
        if (Input.GetKeyDown(KeyCode.D) && lastKey == "q"){
            lastKey = "d";
            StepForward();
            Debug.Log("Yes !");
        }
        anim.SetBool("Run", false);
    }

    // Fonction pour avancer d'un pas
    void StepForward() 
    {
        Debug.Log("StepForward called");
        // Déplace le personnage vers l'avant d'une certaine distance
        playerRb.MovePosition(playerRb.position + new Vector2(stepDistance, 0));
        if (anim != null)
        {
            anim.SetBool("Run", true);
            Debug.Log("Run animation set to true");
        }
    }
}