using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;

    public List<GameObject> hpPointsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GameManager.uiManager = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* Player HP
    3         2
    2         1
    1         0
    */
    public void DisableHpSprite(int value)
    {
        // Ensure the value is between 1 and 3, so value - 1 is a valid index (0 to 2)
        if (value > 0 && value <= hpPointsList.Count)  // Ensure the value is within the valid range
        {
            hpPointsList[value - 1].SetActive(false);  // Disable the sprite at the corresponding index
        }
        else if (value == 0)  // If health is 0 (player is dead), disable all sprites
        {
            // Disable all heart UI elements
            foreach (GameObject heart in hpPointsList)
            {
                heart.SetActive(false);
            }
        }

    }
}

