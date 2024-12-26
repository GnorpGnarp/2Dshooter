using System.Collections; // Make sure this is at the top for IEnumerator and coroutines
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    public GameObject victoryScreen;
    public TMP_Text scoreText;
    public GameObject nextLevelButton;
    public GameObject quitButton;

    private bool victoryShown = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        victoryScreen.SetActive(false);
    }

    // Method to show the victory screen
    public void ShowVictoryScreen()
    {
        if (!victoryShown)
        {
            victoryShown = true;
            Debug.Log("Victory Screen is being shown");
            StartCoroutine(ShowVictoryCoroutine());
        }
    }


    private IEnumerator ShowVictoryCoroutine()
    {
        // Wait for 1 second before showing the victory screen (you can adjust this)
        yield return new WaitForSeconds(1f);

        victoryScreen.SetActive(true);
        scoreText.text = "Score: " + ScoreManager.instance.score.ToString();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < totalScenes)
        {
            nextLevelButton.SetActive(true);
            quitButton.SetActive(false);
        }
        else
        {
            nextLevelButton.SetActive(false);
            quitButton.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        Debug.Log("Leading next level...");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Game Over! Exiting the game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ResetVictoryFlag()
    {
        victoryShown = false;
    }
}
