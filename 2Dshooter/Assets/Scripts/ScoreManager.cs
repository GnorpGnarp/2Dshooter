using UnityEngine;
using TMPro;  // Ensure TextMesh Pro is being used

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;  // Singleton for easy access
    public int score = 0;
    public TMP_Text scoreText;  // Use TMP_Text instead of Text

    void Awake()
    {
        // Ensure there's only one instance of ScoreManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to add score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    // Update the UI with the current score
    private void UpdateScoreDisplay()
    {
        scoreText.text = score.ToString();  // Update TMP_Text with the current score
    }
}
