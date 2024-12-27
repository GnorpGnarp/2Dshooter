using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Singleton instance
    public static ScoreManager instance;

    // Static score variable to store score globally
    public static int score = 0;

    public TMP_Text scoreText;  // For displaying the score in UI

    void Awake()
    {
        // Ensure that there is only one instance of ScoreManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Optionally make the ScoreManager persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }

        // Initialize the score display immediately after the script starts
        UpdateScoreDisplay();
    }

    // Method to add score
    public void AddScore(int points)
    {
        score += points;  // Modify the static score
        UpdateScoreDisplay();  // Update the UI with the new score
    }

    // Update the UI with the current score
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();  // Update TMP_Text with the static score
        }
    }
}
