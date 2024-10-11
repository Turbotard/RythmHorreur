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
    public int combinationLength = 1;          // Longueur de la combinaison de touches
    public float inputWindowTime = 2f;         // Temps autorisé pour entrer la combinaison
    public KeyboardManager keyboardManager;    // Référence au gestionnaire de clavier

    private string[] possibleKeys = { 
        "B", "C", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", 
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", 
        "UpArrow", "DownArrow", "LeftArrow", "RightArrow", 
        "Space", "Enter", "Backspace", "Tab", "Escape", "Shift", "Control", "Alt", "CapsLock", "NumLock", "ScrollLock", 
        "Insert", "Delete", "Home", "End", "PageUp", "PageDown", 
        "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"
    };  // Les touches possibles pour la combinaison    
    private List<string> currentCombination; // La combinaison actuelle
    private int currentIndex = 0; // Indice pour savoir où on est dans la combinaison

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
        if (obstacleHandler.isGroundedMethod())
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
        if (playerRb != null && obstacleHandler.isGroundedMethod() != null)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, obstacleHandler.jumpForce);
            Debug.Log("Saut réussi !"); 
        }
    }
}
