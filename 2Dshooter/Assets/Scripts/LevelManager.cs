using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Reset score when starting a new level
    public void StartNewLevel()
    {
        ScoreManager.score = 0;  // Reset the score to 0

        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene by index (make sure it's valid)
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene exists in the build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("No more scenes to load! Current scene index: " + currentSceneIndex);
        }
    }
}
