using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombinationJump : MonoBehaviour
{
    public Rigidbody2D playerRb;               // Le Rigidbody du joueur pour le saut
    public ObstacleHandling obstacleHandler;   // Référence au script ObstacleHandling
    public TMP_Text combinationText;           // Texte UI affichant la combinaison à l'écran
    public int combinationLength = 3;          // Longueur de la combinaison de touches
    public float inputWindowTime = 2f;         // Temps autorisé pour entrer la combinaison
    public KeyboardManager keyboardManager;    // Référence au gestionnaire de clavier

    private string[] possibleKeys = { "R", "T", "F", "C", "V" };  // Les touches possibles pour la combinaison
    private List<string> currentCombination;                 // La combinaison actuelle
    private int currentIndex = 0;                            // Indice pour savoir où on est dans la combinaison
    private bool isGrounded = true;                          // Indique si le joueur est au sol

    void Start()
    {
        if (keyboardManager == null)
        {
            keyboardManager = FindObjectOfType<KeyboardManager>();  // Trouver le KeyboardManager dans la scène si non assigné
        }

        GenerateNewCombination();  // Génère une première combinaison aléatoire
        DisplayCombination();      // Affiche la combinaison
    }

    void Update()
    {
        // Vérifie si le joueur est au sol
        if (isGrounded)
        {
            // Vérifie l'entrée du joueur
            CheckPlayerInput();

            // Si la combinaison est terminée correctement
            if (currentIndex >= currentCombination.Count)
            {
                Jump();  // Effectue le saut
                GenerateNewCombination();  // Génère une nouvelle combinaison
                DisplayCombination();      // Affiche la nouvelle combinaison
                currentIndex = 0;          // Réinitialise l'index pour la nouvelle combinaison
            }
        }
    }

    // Génère une nouvelle combinaison aléatoire
    void GenerateNewCombination()
    {
        currentCombination = new List<string>();
        for (int i = 0; i < combinationLength; i++)
        {
            string randomKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            currentCombination.Add(randomKey);
        }
    }

    // Affiche la combinaison à l'écran
    void DisplayCombination()
    {
        combinationText.text = "";
        foreach (string key in currentCombination)
        {
            combinationText.text += keyboardManager.GetDisplayKey(key) + " ";  // Utilise la méthode GetDisplayKey pour afficher la bonne touche selon la disposition
        }
    }

    // Vérifie l'entrée du joueur pour chaque touche
    void CheckPlayerInput()
    {
        if (currentIndex < currentCombination.Count)
        {
            string expectedKey = currentCombination[currentIndex];
            KeyCode expectedKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), expectedKey);

            // Utilise le KeyboardManager pour obtenir la touche correcte selon la disposition
            if (Input.GetKeyDown(keyboardManager.GetKeyForLayout(expectedKeyCode)))
            {
                Debug.Log("Touche correcte : " + expectedKey);  // Vérifie que la bonne touche est appuyée
                currentIndex++;  // Passe à la touche suivante dans la combinaison
            }
        }
    }

    // Fait sauter le joueur si la combinaison est correcte
    void Jump()
    {
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, obstacleHandler.jumpForce);
            Debug.Log("Saut réussi !"); 
        }
    }
}
