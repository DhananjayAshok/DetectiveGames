using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectScript : MonoBehaviour
{

    public AudioClip introClip;
    public AudioClip accusedClip;
    public bool isGuilty;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void isSuspected() {
        audioSource.clip = accusedClip;
        audioSource.Play();
    }

    public void isSpokenTo()
    {
        audioSource.clip = introClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
