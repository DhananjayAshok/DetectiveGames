using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeCreationTool
{
    public AudioClip[] audioClips;
    public string[] questions;
    public ConversationTree baseTree;

    public TreeCreationTool(AudioClip[] audioClips, string[] questions)
    {
        this.audioClips = audioClips;
        this.questions = questions;
        CreateTree();
    }

    public void CreateTree() { }

    public AudioClip getClip(int index)
    {
        if (index > audioClips.Length)
        {
            return (AudioClip)audioClips.GetValue(0);
        }
        return (AudioClip)audioClips.GetValue(index);
    }
    public string getQuestion(int index)
    {
        if (index > questions.Length)
        {
            return (string)questions.GetValue(0);
        }
        return (string)questions.GetValue(index);
    }

}
