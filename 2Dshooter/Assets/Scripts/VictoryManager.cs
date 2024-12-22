using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // For loading scenes

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;  // Static instance of VictoryManager for easy access

    public GameObject victoryScreen;  // The UI element to show on victory (like a panel with text)
    public TMP_Text scoreText;  // The TextMeshPro text to display the score
    public GameObject nextLevelButton;  // Button to proceed to the next level
    public GameObject quitButton;  // Button to quit the game if no more levels are available

    void Awake()
    {
        // Make sure there's only one instance of VictoryManager
        if (instance == null)
        {
            instance = this;  // Set the static instance to this object
        }
        else
        {
            Destroy(gameObject);  // If another instance exists, destroy this one to avoid duplicates
        }

        // Ensure the victory screen is disabled when the game starts
        victoryScreen.SetActive(false);
    }

    // This function will be called by the EnemySpawner when the game is won
    public void ShowVictoryScreen()
    {
        // Only show the screen if it's not already shown
        if (!victoryScreen.activeSelf)
        {
            // Show the victory screen UI
            victoryScreen.SetActive(true);

            // Display the current score
            scoreText.text = "Score: " + ScoreManager.instance.score.ToString();

            // Check if there is a next level and enable/disable buttons accordingly
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int totalScenes = SceneManager.sceneCountInBuildSettings;

            if (currentSceneIndex + 1 < totalScenes)
            {
                // Enable the 'Next Level' button if there is another level
                nextLevelButton.SetActive(true);
                quitButton.SetActive(false);  // Hide the quit button
            }
            else
            {
                // If no more levels, show the 'Quit' button
                nextLevelButton.SetActive(false);
                quitButton.SetActive(true);
            }
        }
    }

    // This function will load the next scene
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

    // This function will quit the game if no more levels are available
    public void QuitGame()
    {
        // Print a message to the console (for debugging)
        Debug.Log("Game Over! Exiting the game...");

        // Close the application (works only in a build, not in the editor)
        Application.Quit();

        // If running in the editor, stop playing the scene (optional)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
