using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SuspectScript : MonoBehaviour
{

    [Header("Suspect AudioClips")]
    public AudioGroup accusedClips;
    public AudioGroup afterAccusedClips;
    public AudioGroup defaultConfrontationResponse;
    [Space(10)]
    [Header("Suspect Data")]
    public string suspectName;
    public bool isReasonableSuspect;
    [Space(10)]
    [Header("Confrontation Records")]
    public List<Confrontation> confrontations;
    [Space(10)]
    [Header("Internal Variables (Can ignore)")]
    public TextMesh nameText;
    public ConversationTree baseTree;
    public TreeCreationScript treeCreationScript;
    public Animator animator;
    [HideInInspector]
    public bool hasBeenAccused = false;
    [HideInInspector]
    public bool hasBeenSpokenTo = false;
    [HideInInspector]
    public int noConfrontations = 0;
    [HideInInspector]
    public Confrontation defaultConfrontation;


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
        audioSource.clip = accusedClips.Sample();
        audioSource.Play();
        return audioSource.clip.length;
    }


    public float isSpokenToAfterAccuse()
    {
        audioSource.clip = afterAccusedClips.Sample();
        audioSource.Play();
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

    public void playIdleTalkingAnimation() {
        animator.Play("Idle Talking");
    }

    public void playTalkingAnimation()
    {
        animator.Play("Talking");
    }

    public void playAccusedAnimation() {
        animator.Play("Accused");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
