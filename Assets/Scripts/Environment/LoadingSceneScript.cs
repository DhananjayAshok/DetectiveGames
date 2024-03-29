using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneScript : MonoBehaviour
{
    public AudioGroup loadingSceneClips;
    public AudioGroup loadingSceneBGClips;
    public AudioSource backgroundMusicSource;
    public bool loopSceneClips = false;
    public float minimumWaitTime = 20f;
    float loadStartTime;
    GodScript godScript;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        godScript = GameObject.FindGameObjectWithTag("God").GetComponent<GodScript>();
        StartLoadingScene();
    }

    void StartLoadingScene()
    {
        if (loadingSceneBGClips != null) {
            if (loadingSceneBGClips.getNoClips() > 0) {
                if (backgroundMusicSource != null) {
                    backgroundMusicSource.clip = loadingSceneBGClips.Sample();
                    backgroundMusicSource.Play();
                }
            }
        }
        if (loadingSceneClips != null) {
            if (loadingSceneClips.getNoClips() > 0) {
                StartCoroutine(LoadAudio());
            }
        }
        StartCoroutine(LoadAsync(godScript.nextScene));
    }

    IEnumerator LoadAudio() {
        bool play = true;
        while (play) {
            audioSource.clip = loadingSceneClips.Sample();
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
            play = loopSceneClips;
        }
    }

    IEnumerator LoadAsync(string sceneName) {
        yield return new WaitForSeconds(minimumWaitTime);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        loadStartTime = Time.time;
        while (!op.isDone) {
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
