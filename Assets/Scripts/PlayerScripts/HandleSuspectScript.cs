using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSuspectScript : MonoBehaviour
{
    public GameObject conversationButtons;


    public AudioSource audioSource;
    public AudioClip succesClip;
    public AudioClip failureClip;
    public AudioClip leaveConversationClip;
    public bool inConversation = false;


    public Button[] buttons;
    private ConversationTree currentTree;
    private GameObject currentSuspect;
    
    
    void Start()
    {
        //buttons = {button0, button1, button2, button3};
    }

    public void Accusation(GameObject suspectObject)
    {
        //Debug.Log("Gets here");
        SuspectScript suspectScript = suspectObject.GetComponent<SuspectScript>();
        if (suspectScript.isGuilty)
        {
            Success();
        }
        else
        {
            Failure();
        }
        suspectScript.hasBeenAccused = true; // Should already be set but just in case

    }

    void Success()
    {
        audioSource.clip = succesClip;
        audioSource.Play();
        Debug.Log("SuccessClip");
    }

    void Failure()
    {
        audioSource.clip = failureClip;
        audioSource.Play();
        Debug.Log("FailureClip");
    }
    
    
    public void StartConversation(GameObject suspect)
    {
        currentSuspect = suspect;
        SuspectScript script = suspect.GetComponent<SuspectScript>();
        currentTree = script.baseTree;
        inConversation = true;
        TraverseToNextConversationTree(suspect, currentTree);
        //Debug.Log("Reached After Traversal");
    }
    

    public void TraverseToNextConversationTree(GameObject suspect, ConversationTree targetTree) {
        StartCoroutine(playTreeAnswer(suspect, targetTree));
    }

    public void HandleButtonActivation(ConversationTree tree) {
        if (tree.isLeaf())
        {
            LeaveConversation();
        }
        else
        {
            Debug.Log("Activating Buttons and updating tree");
            updateButtons(tree);
            activateButtons();
        }
    }
    
    public void SelectConversationSubTree(int index) {
        if (currentTree == null || currentSuspect == null)
        {
            Debug.Log("Something is wrong current tree is null");
            return;
        }
        else {
            if (currentTree.getNoChildren() <= index) {
                Debug.Log("Something is wrong current tree is null");
                return;
            }
            currentTree = (ConversationTree) currentTree.children[index];
            TraverseToNextConversationTree(currentSuspect, currentTree);
        }
    }

    
    public IEnumerator playTreeAnswer(GameObject suspect, ConversationTree tree) {
        deactivateButtons();
        AudioSource suspectAudioSource = suspect.GetComponent<AudioSource>();
        suspectAudioSource.clip = tree.answer;
        suspectAudioSource.Play();
        yield return new WaitForSeconds(suspectAudioSource.clip.length + 1.0f);
        HandleButtonActivation(tree);

    }

    public void LeaveConversation()
    {
        inConversation = false;
        deactivateButtons();
        currentSuspect = null;
        currentTree = null;
        audioSource.clip = leaveConversationClip;
        audioSource.Play();
    }

    public void activateButtons() {
        conversationButtons.SetActive(true);
    }

    public void deactivateButtons() {
        conversationButtons.SetActive(false);
    }
    
    public void updateButtons(ConversationTree tree) {
        int noChildren = tree.getNoChildren();
        for(int i = 0; i < noChildren; i++)
        {
            Button button = (Button) buttons.GetValue(i);
            string text = ((ConversationTree)tree.children[i]).question;
            button.GetComponentInChildren<Text>().text = text;
            button.gameObject.SetActive(true);

        }
        for (int i = noChildren; i < 4; i++) {
            Button button = (Button) buttons.GetValue(i);
            button.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {

    }
}
    


    // Update is called once per frame
    
