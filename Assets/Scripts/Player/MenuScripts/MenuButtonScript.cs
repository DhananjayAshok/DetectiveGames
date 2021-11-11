using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuButtonScript : MonoBehaviour
{
    [Header("Internal Variables (Can ignore)")]
    public GameObject backButton;
    public GameObject autopsyButton;
    public GameObject confrontationButton;
    public Text autopsyButtonText;
    public GameObject confrontButton;
    public Text clueName;
    public Text clueString;
    public Text clueAutopsy;
    public Image clueImage;
    public ClueContentScript clueContentScript;
    public MenuAnimationScript menuAnimationScript;
    public MenuAudioScript menuAudioScript;
    [HideInInspector]
    public GodClueScript godClueScript;
    [HideInInspector]
    public ClueObject currentClueObject;
    [HideInInspector]
    public float autopsyWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        godClueScript = GameObject.FindGameObjectsWithTag("God")[0].GetComponent<GodClueScript>(); // There should be one and only one God in the scene
        if (godClueScript.roundNumber == 1)
        {
            confrontationButton.SetActive(false);
        }
        else if (godClueScript.roundNumber == 2) {
            confrontationButton.SetActive(true);
        }
        UpdateAutopsyButtonText();
        autopsyWaitTime = menuAudioScript.getAutopsyWaitTime();
    }

    public void SetClueDisplay(ClueObject clueObject) {
        clueName.text = clueObject.clueName;
        clueString.text = clueObject.clueString;
        clueImage.sprite = clueObject.clueSprite;
        if (clueObject.isAutopsied) {
            ShowAutopsy();
        } else { 
            clueAutopsy.text = "";
            autopsyButton.SetActive(true);
        }
        currentClueObject = clueObject;
    }

    public void Activate(ClueObject clueObject)
    {
        SetClueDisplay(clueObject);
        menuAudioScript.ClueClick();
        menuAnimationScript.SelectClueAnimation();
    }

    public void Deactivate() {
        menuAudioScript.BackClick();
        menuAnimationScript.DeselectClueAnimation();
    }
    
    public void RequestAutopsy() {
        autopsyButton.SetActive(false);
        if (currentClueObject.isAutopsied) {
            Debug.Log("Requesting Lab when it shouldn't be reachable");
        } else {
            int status = godClueScript.PerformAutopsy(currentClueObject.index);
            menuAudioScript.AutopsyRequest();

            if (status != 0)
            {
                clueContentScript.RefreshClues();
                if (status == 1)
                {
                    StartCoroutine(AutopsyFailure());
                }
                else {
                    StartCoroutine(AutopsySuccess());
                }

            }
            else {
                StartCoroutine(AutopsyError());
            }
        }
    }

    IEnumerator AutopsySuccess()
    {
        yield return new WaitForSeconds(autopsyWaitTime);
        menuAudioScript.AutopsySuccess();
        ShowAutopsy();
        UpdateAutopsyButtonText();
    }

    IEnumerator AutopsyFailure()
    {
        yield return new WaitForSeconds(autopsyWaitTime);
        menuAudioScript.AutopsyFailure();
        ShowAutopsy();
        UpdateAutopsyButtonText();
    }

    IEnumerator AutopsyError()
    {
        yield return new WaitForSeconds(autopsyWaitTime);
        menuAudioScript.AutopsyError();
        ShowOutOfAutopsiesError();
    }

    void UpdateAutopsyButtonText() {
        autopsyButtonText.text = "Send Evidence to Lab: " + (godClueScript.noAvailableAutopsies - godClueScript.noAutopsiesPerformed) + " requests left.";
    }

    void ShowAutopsy() {
        autopsyButton.SetActive(false);
        clueAutopsy.text = "Lab Report: " + currentClueObject.clueAutopsy;
    }

    void ShowOutOfAutopsiesError() {
        autopsyButton.SetActive(false);
        clueAutopsy.text = "Sorry, you have contacted The Lab too many times. The Lab is no longer offering its services to you. ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
