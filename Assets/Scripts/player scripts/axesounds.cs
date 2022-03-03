using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axesounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] woosh_Sounds;
    // Start is called before the first frame update
    void PlayWooshSound()
    {
        audioSource.clip = woosh_Sounds[Random.Range(0, woosh_Sounds.Length)];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
