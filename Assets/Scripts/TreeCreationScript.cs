using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCreationScript : MonoBehaviour
{
    public AudioClip[] audioClips;
    public string[] questions;
    public ConversationTree baseTree;

    void Awake()
    {
        CreateTree();
    }

    public virtual void CreateTree() { Debug.Log("Error. Should not be reaching here. You likely have not defined CreateTree for a conversation script"); }

    public ConversationTree getBaseTree() {
        return baseTree;
    }
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
    public ConversationTree ConversationTreeFromList(int index) {
        return new ConversationTree(getQuestion(index), getClip(index));
    }

    public ConversationTree CTFL(int index) {
        return ConversationTreeFromList(index);
    }
    
}
