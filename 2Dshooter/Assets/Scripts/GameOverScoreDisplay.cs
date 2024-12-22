using TMPro;  // For TextMesh Pro
using UnityEngine;

public class GameOverScoreDisplay : MonoBehaviour
{
    public TMP_Text gameOverScoreText;  // Reference to the TMP_Text on the Game Over screen

    void Start()
    {
        // Check if the ScoreManager instance exists and show the score
        if (gameOverScoreText != null && ScoreManager.instance != null)
        {
            gameOverScoreText.text = "Your Score: " + ScoreManager.instance.score.ToString();
        }
    }
}
