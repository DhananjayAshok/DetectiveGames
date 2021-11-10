using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudioGroup
{
    public List<AudioClip> clips;

    public AudioGroup(List<AudioClip> clips)
    {
        this.clips = clips;
    }
    public int getNoClips(){
        if (clips == null)
        {
            return 0;
        }
        else
        {
            return clips.Count;
        }
    }

    public void addClip(AudioClip clip)
    {
        if (clips == null)
        {
            clips = new List<AudioClip> { };
        }
        clips.Add(clip);
    }

    public AudioClip Sample() {
        // If this throws an Out of Bounds exception it means there are no clips. 
        int r = UnityEngine.Random.Range(0, getNoClips());
        return clips[r];
    }

    public void SetSourceClip(AudioSource audioSource) {
        audioSource.clip = Sample();
        return;
    }

    public float getMaxClipLength() {
        float maxLength = 0;
        for (int iii = 0; iii < getNoClips(); iii++) {
            if(clips[iii].length > maxLength)
            {
                maxLength = clips[iii].length;
            }
        }
        return maxLength;

    }
}
