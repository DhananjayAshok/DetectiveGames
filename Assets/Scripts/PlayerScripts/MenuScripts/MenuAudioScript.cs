using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioScript : MonoBehaviour
{

    [Space(10)]
    [Header("Menu Background Music Clips")]
    private AudioGroup pauseMenuClips;
    private AudioGroup mainMenuClips;


    [Space(10)]
    [Header("Clicks and Transition Clips")]
    private AudioGroup clueClickClips;
    private AudioGroup backClickClips;
    private AudioGroup transitionClips;
    private AudioGroup transitionBackClips;

    [Space(10)]
    [Header("Lab Request SFX Clips")]
    private AudioGroup autopsyRequestClips;
    private AudioGroup autopsyFailClips;
    private AudioGroup autopsySuccesClips;
    private AudioGroup autopsyErrorClips;

    [Space(10)]
    [Header("Confrontation Clips")]
    private AudioGroup confrontClips;

    [Space(10)]
    [Header("Internal Variables (Can ignore)")]
    public AudioSource pauseMenuAudioSource;
    public AudioManagementScript audioManagementScript;



    AudioSource audioSource;


    void Awake() {
        pauseMenuClips = audioManagementScript.pauseMenuClips;
        mainMenuClips = audioManagementScript.mainMenuClips;
        clueClickClips = audioManagementScript.clueClickClips;
        backClickClips = audioManagementScript.backClickClips;
        transitionClips = audioManagementScript.transitionClips;
        transitionBackClips = audioManagementScript.transitionBackClips;
        autopsyRequestClips = audioManagementScript.autopsyRequestClips;
        autopsyFailClips = audioManagementScript.autopsyFailClips;
        autopsySuccesClips = audioManagementScript.autopsySuccessClips;
        autopsyErrorClips = audioManagementScript.autopsyErrorClips;
        confrontClips = audioManagementScript.confrontClips;
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayMain();
    
    }

    void PlayMain() {
        mainMenuClips.SetSourceClip(audioSource);
        audioSource.loop = true;
        audioSource.Play();
    }

    void PlayPause() {
        pauseMenuClips.SetSourceClip(audioSource);
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
        transitionClips.SetSourceClip(audioSource);
        audioSource.loop = false;
        audioSource.Play();
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
        PlayPause();
    }

    IEnumerator PauseToMainCoroutine()
    {
        transitionBackClips.SetSourceClip(audioSource);
        audioSource.loop = false;
        audioSource.Play();
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
        PlayMain();
    }

    public void ClueClick() {
        clueClickClips.SetSourceClip(pauseMenuAudioSource);
        pauseMenuAudioSource.Play();
    }

    public void BackClick() {
        backClickClips.SetSourceClip(pauseMenuAudioSource);
        pauseMenuAudioSource.Play();
    }

    public float getAutopsyWaitTime() {
        return autopsyRequestClips.getMaxClipLength();
    }

    public void AutopsyRequest() {
        autopsyRequestClips.SetSourceClip(pauseMenuAudioSource);
        pauseMenuAudioSource.Play();
    }

    public void AutopsySuccess() {
        autopsySuccesClips.SetSourceClip(pauseMenuAudioSource);
        pauseMenuAudioSource.Play();
    }

    public void AutopsyFailure()
    {
        autopsyFailClips.SetSourceClip(pauseMenuAudioSource);
        pauseMenuAudioSource.Play();
    }

    public void AutopsyError()
    {
        autopsyErrorClips.SetSourceClip(pauseMenuAudioSource);
        pauseMenuAudioSource.Play();
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
