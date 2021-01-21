using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public AudioClip bugClip;
    public AudioClip foundClueClip;
    public AudioClip clueBookClip;
    public GameObject interactionText;
    public HandleClueScript handleClueScript;

    GameObject currentClue;
    GameObject currentSuspect;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentClue = null;
    }

    GameObject getCurrentClueFromCollider(Collider collider)
    {
        GameObject obj = collider.gameObject.transform.parent.gameObject;
        return obj.transform.parent.gameObject;
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
            interactionText.SetActive(true);
        }
        else if(collider.tag == "suspect"){
            currentSuspect = getCurrentSuspectFromCollider(collider);
            interactionText.SetActive(true);
        }
        
    }


    void OnTriggerExit(Collider collider) {

        if (collider.tag == "clue") {
            currentClue = null;
            interactionText.SetActive(false);
            handleClueScript.closePopup();
        }
        if (collider.tag == "suspect") {
            currentSuspect = null;
            interactionText.SetActive(false);
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
            currentSuspect.GetComponent<SuspectScript>().isSpokenTo();
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

    void Accuse() {
        
    }

    void StartAccusation() {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Interact();
        }
    }
}
