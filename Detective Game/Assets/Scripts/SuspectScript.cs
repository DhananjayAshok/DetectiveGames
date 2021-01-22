using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SuspectScript : MonoBehaviour
{

    public AudioClip accusedClip;
    public AudioClip afterAccusedClip;
    public bool isGuilty;
    public bool hasBeenAccused;
    public ConversationTree baseTree;
    public TreeCreationScript treeCreationScript;


    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        treeCreationScript = GetComponent<TreeCreationScript>();
        baseTree = treeCreationScript.getBaseTree();
        //Debug.Log(baseTree);

    }

    public void isAccused() {
        if (!hasBeenAccused)
        {
            audioSource.clip = accusedClip;
            audioSource.Play();
            Debug.Log("Is accused");
        }
    }


    public void isSpokenToAfterAccuse()
    {
        audioSource.clip = afterAccusedClip;
        audioSource.Play();
        Debug.Log("Is Spoken to after accused");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
