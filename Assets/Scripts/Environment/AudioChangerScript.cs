using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChangerScript : MonoBehaviour
{
    public AudioGroup musicClips;
    MenuAudioScript menuAudioScript;

    public void Start() {
        menuAudioScript = GameObject.FindGameObjectWithTag("Player Attachment").GetComponent<MenuAudioScript>();
    }

    public void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            menuAudioScript.SetMainClip(musicClips);
        }
    }
}
