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
        Debug.Log("Received health value: " + value);  // This will show the value being passed to the method

        // If health is greater than 0, just disable the corresponding heart sprite
        if (value > 0 && value <= hpPointsList.Count)
        {
            Debug.Log("Disabling heart at index: " + (value - 1));
            hpPointsList[value - 1].SetActive(false);  // Disable the heart sprite based on value
        }
        else if (value <= 0)  // If health is 0 or less, disable all heart sprites
        {
            Debug.Log("Disabling all hearts");
            foreach (GameObject heart in hpPointsList)
            {
                heart.SetActive(false);  // Disable all heart sprites
            }
        }
    }


}

