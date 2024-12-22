using UnityEngine;
using UnityEngine.SceneManagement; // To load scenes
using TMPro; // To interact with TextMesh Pro buttons

public class GameOverController : MonoBehaviour
{
    // This method will restart the last level the player was on
    public void RestartLevel()
    {
        // Retrieve the last played level index saved in PlayerPrefs
        int lastLevelIndex = PlayerPrefs.GetInt("LastLevel", 0);  // Default to 0 if no level is saved

        // Load the last played level
        SceneManager.LoadScene(lastLevelIndex);
    }

    // This method will load the Main Menu scene
    public void GoToMainMenu()
    {
        // Assuming your Main Menu scene is named "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}
