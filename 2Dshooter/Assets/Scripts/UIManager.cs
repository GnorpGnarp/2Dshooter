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
    public void UpdateHpSprites(int currentHealth)
    {
        // Loop through the heart icons in reverse order (index 2 -> 1 -> 0)
        for (int i = 0; i < hpPointsList.Count; i++)
        {
            // Enable hearts up to the current health
            if (i < currentHealth)
            {
                hpPointsList[hpPointsList.Count - 1 - i].SetActive(true);  // Enable hearts in reverse order
            }
            else
            {
                hpPointsList[hpPointsList.Count - 1 - i].SetActive(false);  // Disable hearts beyond current health
            }
        }
    }



}

