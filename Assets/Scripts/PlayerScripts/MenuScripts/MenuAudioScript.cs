using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioScript : MonoBehaviour
{

    public AudioSource pauseMenuAudioSource;
    public AudioClip transitionClip;
    public AudioClip pauseMenuClip;
    public AudioClip transitionBackClip;
    public AudioClip mainMenuClip;
    public AudioClip autopsyRequestClip;
    public AudioClip clueClickClip;
    public AudioClip confrontClips;
    public AudioClip backClickClip;
    public AudioClip autopsyFailClip;
    public AudioClip autopsySuccesClip;
    public AudioClip autopsyErrorClip;


    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMain();
    
    }

    void PlayMain() {
        audioSource.clip = mainMenuClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    void PlayPause() {
        audioSource.clip = pauseMenuClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void MainToPause() {
        StartCoroutine(MainToPauseCoroutine());
    }

    public void PauseToMain()
    {
        StartCoroutine(PauseToMainCoroutine());
    }

    IEnumerator MainToPauseCoroutine()
    {
        audioSource.clip = transitionClip;
        audioSource.loop = false;
        audioSource.Play();
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(transitionClip.length + 0.5f);
        PlayPause();
    }

    IEnumerator PauseToMainCoroutine()
    {
        audioSource.clip = transitionBackClip;
        audioSource.loop = false;
        audioSource.Play();
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(transitionBackClip.length + 0.5f);
        PlayMain();
    }

    public void ClueClick() {
        pauseMenuAudioSource.clip = clueClickClip;
        pauseMenuAudioSource.Play();
    }

    public void BackClick() {
        pauseMenuAudioSource.clip = backClickClip;
        pauseMenuAudioSource.Play();
    }

    public float getAutopsyWaitTime() {
        return autopsyRequestClip.length;
    }

    public void AutopsyRequest() {
        pauseMenuAudioSource.clip = autopsyRequestClip;
        pauseMenuAudioSource.Play();
    }

    public void AutopsySuccess() {
        pauseMenuAudioSource.clip = autopsySuccesClip;
        pauseMenuAudioSource.Play();
    }

    public void AutopsyFailure()
    {
        pauseMenuAudioSource.clip = autopsyFailClip;
        pauseMenuAudioSource.Play();
    }

    public void AutopsyError()
    {
        pauseMenuAudioSource.clip = autopsyErrorClip;
        pauseMenuAudioSource.Play();
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
