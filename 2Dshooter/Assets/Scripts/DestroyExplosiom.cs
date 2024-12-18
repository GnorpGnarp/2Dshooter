using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 2f; // Set the time to destroy the explosion

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after 'lifetime' seconds
    }
}
