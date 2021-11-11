using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;


public class SuspectScript : MonoBehaviour
{

    public AudioGroup accusedClips;
    public AudioGroup afterAccusedClips;
    public bool isGuilty;
    [HideInInspector]
    public bool hasBeenAccused = false;
    [HideInInspector]
    public bool hasBeenSpokenTo = false;
    [HideInInspector]
    public bool hasBeenConfronted = false;
    public TextMesh nameText;
    public string suspectName;
    public ConversationTree baseTree;
    public TreeCreationScript treeCreationScript;
    public AudioGroup defaultConfrontationResponse;
    public List<Confrontation> confrontations;
    [HideInInspector]
    public Confrontation defaultConfrontation;

    public Animator animator;


    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        treeCreationScript = GetComponent<TreeCreationScript>();
        baseTree = treeCreationScript.getBaseTree();
        nameText.text = suspectName;
        defaultConfrontation = new Confrontation(defaultConfrontationResponse);
        //Debug.Log(baseTree);

    }

    public float isAccused() {
        if (!hasBeenAccused)
        {
            audioSource.clip = accusedClips.Sample();
            audioSource.Play();
            Debug.Log("Is accused");
            return audioSource.clip.length;
        }
        return 0;
    }


    public float isSpokenToAfterAccuse()
    {
        audioSource.clip = afterAccusedClips.Sample();
        audioSource.Play();
        Debug.Log("Is Spoken to after accused");
        return audioSource.clip.length;
    }

    public Confrontation getConfrontation(string clueName)
    {
        for (int iii = 0; iii < confrontations.Count; iii++)
        {
            if (confrontations[iii].clueName.Equals(clueName))
            {
                return confrontations[iii];
            }
        }
        return defaultConfrontation;
    }

    public void playSuccessfulConfrontationAnimation() {
        animator.Play("Successful Confrontation");
    }

    public void playUnsuccessfulConfrontationAnimation() {
        animator.Play("Unsuccessful Confrontation");
    }

    public void playIdleAnimation() {
        animator.Play("Idle");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
