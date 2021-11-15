using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSuspectScript : MonoBehaviour
{ 

    [Header("Internal Variables (Can ignore)")]
    public AudioSource audioSource;
    public Text clueNameText;
    public InteractionScript interactionScript;
    public Button[] buttons;
    public GameObject conversationButtons;
    [HideInInspector]
    public bool inConversation = false;
    [HideInInspector]
    public bool isAccusing = false;

    [HideInInspector]
    public PlayerPauseScript playerPauseScript;

    Animator playerAnimator;

    AudioManagementScript audioManagementScript;
    MenuAnimationScript menuAnimationScript;
    AudioGroup narratorSuccessClips;
    AudioGroup narratorFailureClips;
    AudioGroup leaveConversationClips;
    AudioGroup accuseClips;
    AudioGroup bugClips;
    AudioGroup reconfrontClips;
    AudioGroup confrontClips;
    AudioGroup leaveSuccessfulConfrontationClips;
    AudioGroup leaveUnsuccessfulConfrontationClips;
    private ConversationTree currentTree;
    private GameObject currentSuspect;
    GodScript godScript;


void Start()
    {
        audioManagementScript = GetComponent<AudioManagementScript>();
        menuAnimationScript = GetComponent<MenuAnimationScript>();
        narratorSuccessClips = audioManagementScript.narratorSuccessClips;
        narratorFailureClips = audioManagementScript.narratorFailureClips;
        leaveConversationClips = audioManagementScript.leaveConversationClips;
        reconfrontClips = audioManagementScript.reconfrontClips;
        confrontClips = audioManagementScript.confrontClips;
        bugClips = audioManagementScript.bugClips;
        accuseClips = audioManagementScript.accuseClips;
        leaveSuccessfulConfrontationClips = audioManagementScript.leaveSuccessfulConfrontationClips;
        leaveUnsuccessfulConfrontationClips = audioManagementScript.leaveUnsuccessfulConfrontationClips;
        playerAnimator = transform.parent.gameObject.GetComponent<Animator>();
        playerPauseScript = GetComponent<PlayerPauseScript>();
        godScript = GameObject.FindGameObjectsWithTag("God")[0].GetComponent<GodScript>(); // There should be one and only one God in the scene
    }

    public void RegisterAccusation(SuspectScript suspectScript)
    {
        godScript.accusedSuspect = suspectScript.suspectName;
        godScript.accusedSuspectReasonable = suspectScript.isReasonableSuspect;
        suspectScript.hasBeenAccused = true;
    }

    public void EndAccusation(SuspectScript suspectScript) {
        if (suspectScript.isReasonableSuspect)
        {
            Success();
        }
        else
        {
            Failure();
        }
    }

    void Success()
    {
        audioSource.clip = narratorSuccessClips.Sample();
        audioSource.Play();
    }

    void Failure()
    {
        audioSource.clip = narratorFailureClips.Sample();
        audioSource.Play();
    }
    
    
    public void StartConversation(GameObject suspect)
    {
        playerPauseScript.StartConversation();
        currentSuspect = suspect;
        SuspectScript script = suspect.GetComponent<SuspectScript>();
        script.playIdleTalkingAnimation();
        currentTree = script.baseTree;
        inConversation = true;
        TraverseToNextConversationTree(suspect, currentTree);
        //Debug.Log("Reached After Traversal");
    }

    public void Accusation(GameObject suspect) {
        SuspectScript ss = suspect.GetComponent<SuspectScript>();
        currentSuspect = suspect;
        StartCoroutine(PerformAccusation(ss));
    }

    IEnumerator PerformAccusation(SuspectScript ss)
    {
        isAccusing = true;
        playerPauseScript.StartAccusation();
        audioSource.clip = accuseClips.Sample();
        //double waitTime = foundSuspectClip.length; //+ ((double) 2.0);
        audioSource.Play();
        ss.playAccusedAnimation();
        yield return new WaitForSeconds(audioSource.clip.length + 1.0f);
        RegisterAccusation(ss);
        float waitTime = ss.isAccused();
        yield return new WaitForSeconds(waitTime + 0.2f);
        EndAccusation(ss);
        currentSuspect = null;
        isAccusing = false;
        playerPauseScript.EndAccusation();
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
        suspect.GetComponent<SuspectScript>().playTalkingAnimation();
        yield return new WaitForSeconds(suspectAudioSource.clip.length + 1.0f);
        suspect.GetComponent<SuspectScript>().playIdleTalkingAnimation();
        HandleButtonActivation(tree);

    }

    public void LeaveConversationArgless() {
        LeaveConversation(false, false);
    }

    public void LeaveConversation(bool afterConfrontation=false, bool succesfulConfrontation=false)
    {
        inConversation = false;
        deactivateButtons();
        currentSuspect.GetComponent<SuspectScript>().playIdleAnimation();
        currentSuspect = null;
        currentTree = null;
        if (afterConfrontation)
        {
            if (succesfulConfrontation)
            {
                audioSource.clip = leaveSuccessfulConfrontationClips.Sample();
            }
            else {
                audioSource.clip = leaveUnsuccessfulConfrontationClips.Sample();
            }
        }
        else {
            audioSource.clip = leaveConversationClips.Sample();
        }
        audioSource.Play();
        playerPauseScript.LeaveConversation();
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

    public void RequestConfrontation() {
        if (currentSuspect == null) {
            audioSource.clip = bugClips.Sample();
            audioSource.Play();
            return;
        }
        SuspectScript suspectScript = currentSuspect.GetComponent<SuspectScript>();
        if (suspectScript.noConfrontations>godScript.noAvailableConfrontations)
        {
            audioSource.clip = reconfrontClips.Sample();
            audioSource.Play();
            return;
        }
        else {
            Confront();
        }

    }

    public void Confront() {
        audioSource.clip = confrontClips.Sample();
        audioSource.Play();
        SuspectScript suspectScript = currentSuspect.GetComponent<SuspectScript>();
        string clueName = clueNameText.text;
        Confrontation confrontation = suspectScript.getConfrontation(clueName);
        AudioGroup responses = confrontation.responses;
        bool isSuccessfulConfrontation = confrontation.isSuccessfulConfrontation;
        AudioSource suspectAudioSource = currentSuspect.GetComponent<AudioSource>();
        suspectAudioSource.clip = responses.Sample();
        float waitTimePlayer = audioSource.clip.length + 1.0f;
        float waitTimeSuspect = suspectAudioSource.clip.length + 1.0f;
        StartCoroutine(ConfrontAnimation(isSuccessfulConfrontation, waitTimePlayer, waitTimeSuspect));
        StartCoroutine(ConfrontAudio(suspectAudioSource, waitTimePlayer, waitTimeSuspect));
        StartCoroutine(ConfrontCanvas(isSuccessfulConfrontation, waitTimePlayer, waitTimeSuspect));
        suspectScript.noConfrontations++;

    }

    public IEnumerator ConfrontAnimation(bool successfulConfrontation, float waitTimePlayer, float waitTimeSuspect) {
        playerAnimator.SetBool("isConfronting", true);
        playerAnimator.Play("Confrontation");
        yield return new WaitForSeconds(waitTimePlayer);
        playerAnimator.SetBool("isConfronting", false);
        if (successfulConfrontation) {
            currentSuspect.GetComponent<SuspectScript>().playSuccessfulConfrontationAnimation();
        } else {
        }
        currentSuspect.GetComponent<SuspectScript>().playUnsuccessfulConfrontationAnimation();
        yield return new WaitForSeconds(waitTimeSuspect);
        currentSuspect.GetComponent<SuspectScript>().playIdleAnimation();
    }

    public IEnumerator ConfrontAudio(AudioSource suspectAudioSource, float waitTimePlayer, float waitTimeSuspect) {
        audioSource.Play();
        yield return new WaitForSeconds(waitTimePlayer);
        suspectAudioSource.Play();
    }

    public IEnumerator ConfrontCanvas(bool successfulConfrontation, float waitTimePlayer, float waitTimeSuspect) {
        interactionScript.interactionButton.SetActive(false);
        interactionScript.accusationSlider.SetActive(false);
        deactivateButtons();
        playerPauseScript.MiniUnpauseProcess();
        yield return new WaitForSeconds(waitTimePlayer);
        yield return new WaitForSeconds(waitTimeSuspect);
        LeaveConversation(true, successfulConfrontation);
    }



    void Update()
    {

    }
}
    


    // Update is called once per frame
    
