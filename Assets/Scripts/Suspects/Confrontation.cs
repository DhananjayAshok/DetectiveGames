using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Confrontation
{
    public string clueName;
    public AudioGroup responses;
    public bool isSuccessfulConfrontation;

    public Confrontation(AudioGroup defaultAudioGroup) {
        this.clueName = "";
        this.responses = defaultAudioGroup;
        this.isSuccessfulConfrontation = false;
    }
}

