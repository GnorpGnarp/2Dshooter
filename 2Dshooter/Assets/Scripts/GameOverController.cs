using UnityEngine;
using UnityEngine.SceneManagement; // To load scenes
using TMPro; // To interact with TextMesh Pro buttons
using UnityEngine.UI; // Required for UI buttons


public class GameOverMenu : MonoBehaviour
{
    public Button restartButton; // Link your restart button here in the Inspector
    public Button mainMenuButton; // Link your main menu button here in the Inspector

    void Start()
    {
        // Check if buttons are assigned, then add listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
    }

    public void OnRestartButtonClicked()
    {
        Debug.Log("Restart Button Clicked!");
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel")); // Restart the last level
    }

    public void OnMainMenuButtonClicked()
    {
        Debug.Log("Main Menu Button Clicked!");
        SceneManager.LoadScene("MainMenu"); // Assuming you have a MainMenu scene
    }
}
