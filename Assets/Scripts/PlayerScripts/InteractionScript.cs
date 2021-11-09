using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    public AudioClip bugClip;
    public AudioClip foundClueClip;
    public AudioClip clueBookClip;
    public AudioClip foundSuspectClip;
    public AudioClip interactWithInnocentAccusedClip;
    public AudioClip interactWithGuiltyAccusedClip;
    public AudioClip accuseClip;
    public GameObject interactionButton;
    public GameObject accusationSlider;
    public HandleClueScript handleClueScript;
    public HandleSuspectScript handleSuspectScript;

    GameObject currentClue;
    GameObject currentSuspect;
    AudioSource audioSource;
    int counter = 0;
    int accuseCount = 50;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentClue = null;
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

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "clue") {
            //Debug.Log("Collision");
            currentClue = getCurrentClueFromCollider(collider);
            interactionButton.SetActive(true);
        }
        else if(collider.tag == "suspect"){
            currentSuspect = getCurrentSuspectFromCollider(collider);
            interactionButton.SetActive(true);
            if (!currentSuspect.GetComponent<SuspectScript>().hasBeenAccused) { 
                accusationSlider.SetActive(true); 
            }
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
    }

    void Interact() {
        if (currentClue == null && currentSuspect == null)
        {
            audioSource.clip = bugClip;
            audioSource.Play();
            //Debug.Log("None");
        }
        else if ((currentClue != null && currentSuspect != null) || (currentSuspect != null)) {
            if (!currentSuspect.GetComponent<SuspectScript>().hasBeenAccused)
            {
                StartCoroutine(suspectInteract());
            }
            else {
                StartCoroutine(accusedSuspectInteract());
            }
        }
        else
        {
            if (currentClue.GetComponent<ClueScript>().discovered)
            {
                audioSource.clip = clueBookClip;
            }
            else
            {

                audioSource.clip = foundClueClip;
            }

            audioSource.Play();
            handleClueScript.displayClue(currentClue);
        }
    }

    IEnumerator suspectInteract() {
        audioSource.clip = foundSuspectClip;
        //double waitTime = foundSuspectClip.length; //+ ((double) 2.0);
        audioSource.Play();
        Debug.Log("Interact with suspect");
        yield return new WaitForSeconds(foundSuspectClip.length + 1.0f);
        handleSuspectScript.StartConversation(currentSuspect);
    }

    IEnumerator accusedSuspectInteract() {
        if (currentSuspect.GetComponent<SuspectScript>().isGuilty)
        {
            audioSource.clip = interactWithGuiltyAccusedClip;
        }
        else {
            audioSource.clip = interactWithInnocentAccusedClip;
        }
        //double waitTime = foundSuspectClip.length; //+ ((double) 2.0);
        audioSource.Play();
        Debug.Log("Interact with accused suspect");
        yield return new WaitForSeconds(audioSource.clip.length + 1.0f);
        currentSuspect.GetComponent<SuspectScript>().isSpokenToAfterAccuse();
    }

    IEnumerator suspectAccuse() {
        audioSource.clip = accuseClip;
        //double waitTime = foundSuspectClip.length; //+ ((double) 2.0);
        audioSource.Play();
        Debug.Log("AccuseClip");
        yield return new WaitForSeconds(accuseClip.length + 1.0f);
        currentSuspect.GetComponent<SuspectScript>().isAccused();
        yield return new WaitForSeconds(currentSuspect.GetComponent<SuspectScript>().accusedClip.length+0.2f);
        currentSuspect.GetComponent<SuspectScript>().hasBeenAccused = true;
        handleSuspectScript.Accusation(currentSuspect);

    }

    void Accuse() {
        if (currentSuspect == null)
        {
            audioSource.clip = bugClip;
            audioSource.Play();
            Debug.Log("Null Current Suspect");
        }
        else
        {
            if (!currentSuspect.GetComponent<SuspectScript>().hasBeenAccused) {
                StartCoroutine(suspectAccuse());
                
                
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
        if (Input.GetKey(KeyCode.Return))
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
