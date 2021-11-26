using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    [Space(10)]
    [Header("UI Customization")]
    public GameObject interactionButton;
    public GameObject accusationSlider;

    [Space(10)]
    [Header("Internal Variables (Can ignore)")]
    public HandleClueScript handleClueScript;
    public HandleSuspectScript handleSuspectScript;
    public ClueContentScript clueContentScript;
    public AudioManagementScript audioManagementScript;
    public PlayerPortalScript playerPortalScript;
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public GodScript godScript;
    [HideInInspector]
    public bool hasAccused;

    private AudioGroup bugClips;
    private AudioGroup foundClueClips;
    private AudioGroup clueBookClips;
    private AudioGroup foundSuspectClips;
    private AudioGroup interactWithInnocentAccusedClips;
    private AudioGroup interactWitReasonablyAccusedClips;
    private AudioGroup accuseClips;
    private AudioGroup narratorIntroductionClips;

    GameObject currentClue;
    GameObject currentSuspect;
    bool inPortal;
    TeleporterScript currentTeleporter;
    InternalPortalScript currentInternalPortal;
    int counter = 0;
    int accuseCount = 50;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bugClips = audioManagementScript.bugClips;
        foundClueClips = audioManagementScript.foundClueClips;
        clueBookClips = audioManagementScript.clueBookClips;
        foundSuspectClips = audioManagementScript.foundSuspectClips;
        interactWithInnocentAccusedClips = audioManagementScript.interactWithInnocentAccusedClips;
        interactWitReasonablyAccusedClips = audioManagementScript.interactWitReasonablyAccusedClips;
        accuseClips = audioManagementScript.accuseClips;
        narratorIntroductionClips = audioManagementScript.narratorIntroductionClips;
        audioSource.clip = narratorIntroductionClips.Sample();
        audioSource.loop = false;
        audioSource.Play();
        currentClue = null;
        godScript = GameObject.FindGameObjectsWithTag("God")[0].GetComponent<GodScript>(); // There should be one and only one God in the scene
    }

    GameObject getCurrentClueFromCollider(Collider collider)
    {
        GameObject obj = collider.gameObject.transform.parent.gameObject;
        return obj;
    }

    GameObject getCurrentSuspectFromCollider(Collider collider)
    {
        GameObject obj = collider.gameObject.transform.parent.gameObject;
        return obj.transform.parent.gameObject;
    }

    TeleporterScript getCurrentTeleporterScriptFromCollider(Collider collider) {
        return collider.gameObject.GetComponent<TeleporterScript>();
    }

    InternalPortalScript getCurrentInteralPortalScriptFromCollider(Collider collider) {
        return collider.gameObject.GetComponent<InternalPortalScript>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "clue")
        {
            //Debug.Log("Collision");
            currentClue = getCurrentClueFromCollider(collider);
            interactionButton.SetActive(true);
        }
        else if (collider.tag == "suspect")
        {
            currentSuspect = getCurrentSuspectFromCollider(collider);
            interactionButton.SetActive(true);
            if (!currentSuspect.GetComponent<SuspectScript>().hasBeenAccused && godScript.roundNumber == 2)
            {
                accusationSlider.SetActive(true);
            }
        }
        else if (collider.tag == "Finish") {
            if (godScript.roundNumber == 1)
            {
                interactionButton.SetActive(true);
                inPortal = true;
                playerPortalScript.EnterPortal();
            }
            else if (godScript.roundNumber == 2)
            {
                if (hasAccused) {
                    interactionButton.SetActive(true);
                    inPortal = true;
                    playerPortalScript.EnterPortal();
                }
            }
            else {
                Debug.Log("error");
            }
        }
        else if (collider.tag == "teleporter")
        {
            interactionButton.SetActive(true);
            currentTeleporter = getCurrentTeleporterScriptFromCollider(collider);
        }
        else if (collider.tag == "internal portal")
        {
            interactionButton.SetActive(true);
            currentInternalPortal = getCurrentInteralPortalScriptFromCollider(collider);
        }

    }


    void OnTriggerExit(Collider collider) {
        if (collider.tag == "clue") {
            currentClue = null;
            interactionButton.SetActive(false);
            handleClueScript.closePopup();
        }
        if (collider.tag == "suspect") {
            currentSuspect = null;
            interactionButton.SetActive(false);
            accusationSlider.SetActive(false);
            if (handleSuspectScript.inConversation) {
                handleSuspectScript.LeaveConversation();
            }
        }
        else if (collider.tag == "Finish")
        {
            interactionButton.SetActive(false);
            inPortal = false;
            playerPortalScript.ExitPortal();
        }
        else if (collider.tag == "teleporter")
        {
            interactionButton.SetActive(false);
            currentTeleporter = null;
        }
        else if (collider.tag == "internal portal")
        {
            interactionButton.SetActive(false);
            currentInternalPortal = null;
        }
    }

    void Interact() {
        if (currentClue == null && currentSuspect == null && !inPortal && currentTeleporter == null && currentInternalPortal == null)
        {
            audioSource.clip = bugClips.Sample();
            audioSource.Play();
            //Debug.Log("None");
        }
        else if ((currentClue != null && currentSuspect != null) || (currentSuspect != null))
        {
            SuspectScript ss = currentSuspect.GetComponent<SuspectScript>();
            if (!ss.hasBeenSpokenTo)
            {
                godScript.addSuspect(ss);
            }
            if (!ss.hasBeenAccused)
            {

                StartCoroutine(suspectInteract());
            }
            else
            {
                StartCoroutine(accusedSuspectInteract());
            }
            ss.hasBeenSpokenTo = true;
        }
        else if (currentClue != null)
        {
            if (currentClue.GetComponent<ClueScript>().discovered)
            {
                audioSource.clip = clueBookClips.Sample();
            }
            else
            {

                audioSource.clip = foundClueClips.Sample();
                ClueScript cs = currentClue.GetComponent<ClueScript>();
                int index = godScript.noCluesDiscovered;
                godScript.addClue(cs);
                clueContentScript.CreateClueRecord(cs, index);
            }

            audioSource.Play();
            handleClueScript.displayClue(currentClue);
        }
        else if (inPortal)
        {
            playerPortalScript.ActivatePortal();
        }
        else if (currentTeleporter != null)
        {
            currentTeleporter.Teleport();
        }
        else if (currentInternalPortal != null)
        {
            currentInternalPortal.Activate();
        }
        else {
            Debug.Log("Error");
        }
    }

    IEnumerator suspectInteract() {
        audioSource.clip = foundSuspectClips.Sample();
        //double waitTime = foundSuspectClip.length; //+ ((double) 2.0);
        audioSource.Play();
        handleSuspectScript.playerPauseScript.StartConversation();
        yield return new WaitForSeconds(audioSource.clip.length + 1.0f);
        handleSuspectScript.StartConversation(currentSuspect);
    }

    IEnumerator accusedSuspectInteract() {
        if (currentSuspect.GetComponent<SuspectScript>().isReasonableSuspect)
        {
            audioSource.clip = interactWitReasonablyAccusedClips.Sample();
        }
        else {
            audioSource.clip = interactWithInnocentAccusedClips.Sample();
        }
        //double waitTime = foundSuspectClip.length; //+ ((double) 2.0);
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 1.0f);
        currentSuspect.GetComponent<SuspectScript>().isSpokenToAfterAccuse();
    }

    IEnumerator suspectAccuse() {
        SuspectScript ss = currentSuspect.GetComponent<SuspectScript>();
        audioSource.clip = accuseClips.Sample();
        //double waitTime = foundSuspectClip.length; //+ ((double) 2.0);
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 1.0f);
        float waitTime = ss.isAccused();
        yield return new WaitForSeconds(waitTime+0.2f);
        ss.hasBeenAccused = true;
        hasAccused = true;
        handleSuspectScript.Accusation(currentSuspect);
    }

    void Accuse() {
        if (currentSuspect == null)
        {
            audioSource.clip = bugClips.Sample();
            audioSource.Play();
            Debug.Log("Null Current Suspect");
        }
        else
        {
            if (!currentSuspect.GetComponent<SuspectScript>().hasBeenAccused) {
                //StartCoroutine(suspectAccuse());
                handleSuspectScript.Accusation(currentSuspect);
                hasAccused = true;
            }
        }
    }

    void UpdateSlider() {
        float normalized = ((float) counter) / ((float) accuseCount);
        accusationSlider.GetComponent<Slider>().value = normalized;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Interact();
        }
        if (Input.GetKey(KeyCode.Return) && !hasAccused && accusationSlider.activeSelf)
        {
            counter++;
            if (counter >= accuseCount)
            {
                counter = accuseCount;
                Accuse();
            }
            UpdateSlider();
        }
        if (Input.GetKeyUp(KeyCode.Return)) {
            counter = 0;
            UpdateSlider();
        }

    }
}
