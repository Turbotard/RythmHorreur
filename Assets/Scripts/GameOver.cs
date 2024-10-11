using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Animator anim;
    public GameObject player;
    public float delayBeforeLoading = 2f; // Delay in seconds

    void Start()
    {
        if (player != null)
        {
            anim = player.GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogError("Animator component not found on the player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject is not assigned!");
        }
    }
    public void Gameover()
    {
        anim.SetBool("Dead", true);

        StartCoroutine(LoadSceneAfterDelay("GameOver", delayBeforeLoading));
    }
    private IEnumerator LoadSceneAfterDelay(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    // Fonction pour redémarrer le jeu
    void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

}
