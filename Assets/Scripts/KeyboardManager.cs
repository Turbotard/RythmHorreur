using UnityEngine;

public enum KeyboardLayout
{
    QWERTY,
    AZERTY
}

public class KeyboardManager : MonoBehaviour
{
    public KeyboardLayout playerKeyboardLayout = KeyboardLayout.QWERTY;  // Par défaut, on considère que c'est QWERTY

    // Cette méthode prend une touche et renvoie la touche correcte selon la disposition du clavier
    public KeyCode GetKeyForLayout(KeyCode key)
    {
        if (playerKeyboardLayout == KeyboardLayout.AZERTY)
        {
            switch (key)
            {
                // Changement des touches de base pour AZERTY
                case KeyCode.W: return KeyCode.Z;  // W devient Z
                case KeyCode.A: return KeyCode.Q;  // A devient Q
                case KeyCode.Z: return KeyCode.W;  // Z devient W
                case KeyCode.Q: return KeyCode.A;  // Q devient A

                // Changement des autres touches spécifiques
                case KeyCode.M: return KeyCode.Comma;   // M devient ,
                case KeyCode.Comma: return KeyCode.M;   // , devient M
                case KeyCode.Semicolon: return KeyCode.Period;   // ; devient .
                case KeyCode.Period: return KeyCode.Semicolon;   // . devient ;

                default: return key;  // Pour les autres touches, pas de changement
            }
        }
        return key;  // Si QWERTY, pas de changement
    }

    // Retourne la version affichable de la touche pour l'interface utilisateur
    public string GetDisplayKey(string key)
    {
        if (playerKeyboardLayout == KeyboardLayout.AZERTY)
        {
            switch (key)
            {
                case "W": return "Z";
                case "A": return "Q";
                case "Z": return "W";
                case "Q": return "A";
                case "M": return ",";
                case ",": return "M";
                case ";": return ".";
                case ".": return ";";
                default: return key;
            }
        }
        return key;  // Si QWERTY, pas de changement
    }
    
    public void SetKeyboardAZERTY()
    {
        Debug.Log("AZERTY");
        playerKeyboardLayout = KeyboardLayout.AZERTY;
    }
    
    public void SetKeyboardQWERTY()
    {
        Debug.Log("QWERTY");
        playerKeyboardLayout = KeyboardLayout.QWERTY;
    }
}
