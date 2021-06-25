using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSound : MonoBehaviour
{
    AudioSource backSound;
    void Start()
    {
        backSound = GetComponent<AudioSource>();
        backSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
