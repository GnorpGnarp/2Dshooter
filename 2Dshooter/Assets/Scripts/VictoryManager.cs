using UnityEngine;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    public GameObject victoryScreen;
    public TMP_Text scoreText;
    public GameObject nextLevelButton;
    public GameObject quitButton;

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
        scoreText.text = "Score: " + ScoreManager.instance.score.ToString();  // Display score

        // Show next level button or quit button based on the current scene
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int totalScenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

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
