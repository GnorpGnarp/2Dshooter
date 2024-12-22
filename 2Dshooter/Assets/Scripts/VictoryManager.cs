using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    public GameObject victoryScreen;
    public TMP_Text scoreText;
    public GameObject nextLevelButton;
    public GameObject quitButton;

    // Add a flag to track whether victory has already been shown
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
        // Avoid showing the victory screen multiple times
        if (!victoryShown)
        {
            victoryShown = true;
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
    }

    public void LoadNextLevel()
    {
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

    // Add a method to reset victory flag (optional, in case you need to retry the level)
    public void ResetVictoryFlag()
    {
        victoryShown = false;
    }
}
