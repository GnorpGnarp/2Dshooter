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
    public EnemySpawner[] enemySpawners;
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

        victoryScreen.SetActive(false);  // Hide the victory screen initially
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);  // Show the victory screen
        scoreText.text = "Score: " + ScoreManager.score.ToString();        // Display score

        // Disable enemy spawners when the victory screen is shown
        foreach (var spawner in enemySpawners)
        {
            spawner.enabled = false;  // Disable the spawner to stop enemy spawning
        }

        // Show next level button or quit button based on the current scene
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
    // Button function to load the next level
    public void LoadNextLevel()
    {
        Debug.Log("Loading next level...");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Load the next scene by index
        SceneManager.LoadScene(nextSceneIndex);
    }

    // Button function to quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Stop the game in the editor
#else
            Application.Quit();  // Quit the game if it's a build
#endif

        Debug.Log("Game is quitting...");
    }
}

