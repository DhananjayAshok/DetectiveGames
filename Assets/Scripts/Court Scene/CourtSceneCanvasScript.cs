using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourtSceneCanvasScript : MonoBehaviour
{
    public GameObject questionText, limitText, resetCluesButton, resetStatementsButton, submitButton, cluesContent, statementsContent;
    public GameObject clueTogglePrefab, statementTogglePrefab;
    [HideInInspector]
    public GameObject[] clueToggles;
    [HideInInspector]
    public GameObject[] statementToggles;
    int limit=1;
    int noItemsSelected = 0;
    Animator animator;
    bool isRisen = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(CourtGodScript courtGodScript, ClueObject[] clueObjects, int noCluesDiscovered, string[] suspectNames, int noSuspectsDiscovered) {

        CreateToggles(clueObjects, noCluesDiscovered, suspectNames, noSuspectsDiscovered);
        submitButton.GetComponent<Button>().onClick.AddListener(courtGodScript.SubmitResponse);
    }


    public void CreateToggles(ClueObject[] clueObjects, int noCluesDiscovered,  string[] suspectNames, int noSuspectsDiscovered) 
    {
        clueToggles = new GameObject[noCluesDiscovered];
        statementToggles = new GameObject[noSuspectsDiscovered];
        for (int iii = 0; iii < noCluesDiscovered; iii++) {
            GameObject clueToggle = Instantiate(clueTogglePrefab, cluesContent.transform);
            clueToggle.GetComponent<CourtClueToggleScript>().UpdateToggleInfo(clueObjects[iii].clueName);
            clueToggle.GetComponent<Toggle>().onValueChanged.AddListener(ToggleSwitched);
            clueToggles[iii] = clueToggle;
        }

        for (int iii = 0; iii < noSuspectsDiscovered; iii++)
        {
            GameObject statementToggle = Instantiate(statementTogglePrefab, statementsContent.transform);
            statementToggle.GetComponent<CourtStatementToggleScript>().UpdateToggleInfo(suspectNames[iii]);
            statementToggle.GetComponent<Toggle>().onValueChanged.AddListener(ToggleSwitched);
            statementToggles[iii] = statementToggle;
        }


    }

    public void UpdateInfo(string question, int limit) {
        UpdateQuestion(question);
        UpdateLimit(limit);
    }

    public void UpdateLimit(int limit) {
        this.limit = limit;
        limitText.GetComponent<Text>().text = "Item Selected: " + noItemsSelected + "/" + limit; 
    }

    public void UpdateQuestion(string question) {
        questionText.GetComponent<Text>().text = question;
    }

    public void DeactivateToggles() {
        for (int iii = 0; iii < clueToggles.Length + statementToggles.Length; iii++) {
            int index = 0;
            GameObject toggle;
            if (iii < clueToggles.Length)
            {
                index = iii;
                toggle = clueToggles[index];
            }
            else {
                index = iii - clueToggles.Length;
                toggle = statementToggles[index];
            }
            Toggle t = toggle.GetComponent<Toggle>();
            if (!t.isOn)
            {
                t.interactable = false;
            }
        }
    }

    public void ActivateToggles() {
        for (int iii = 0; iii < clueToggles.Length + statementToggles.Length; iii++)
        {
            int index = 0;
            GameObject toggle;
            if (iii < clueToggles.Length)
            {
                index = iii;
                toggle = clueToggles[index];
            }
            else
            {
                index = iii - clueToggles.Length;
                toggle = statementToggles[index];
            }
            Toggle t = toggle.GetComponent<Toggle>();
            t.interactable = true;
        }
    }

    void HandleActivation() {
        if (noItemsSelected >= limit)
        {
            DeactivateToggles();
        }
        else
        {
            ActivateToggles();
        }
        if (noItemsSelected == 0)
        {
            submitButton.GetComponent<Button>().interactable = false;
        }
        else {
            submitButton.GetComponent<Button>().interactable = true;
        }
        UpdateLimit(limit);
    }

    public void ToggleSwitched(bool isOn) {
        int additive = 0;
        if (isOn)
        {
            additive = 1;
        }
        else {
            additive = -1;
        }
        noItemsSelected = noItemsSelected + additive;
        HandleActivation();
    }

    public void ResetClues() {
        for (int iii = 0; iii < clueToggles.Length; iii++) {
            clueToggles[iii].GetComponent<Toggle>().isOn = false;
        }
        HandleActivation();
    }

    public void ResetStatements()
    {
        for (int iii = 0; iii < statementToggles.Length; iii++)
        {
            statementToggles[iii].GetComponent<Toggle>().isOn = false;
        }
        HandleActivation();
    }

    public void ResetAll() {
        ResetClues();
        ResetStatements();
    }

    public HashSet<string> ReadClueResponses() {
        HashSet<string> clueNames = new HashSet<string>();
        for (int iii = 0; iii < clueToggles.Length; iii++)
        {
            if (clueToggles[iii].GetComponent<Toggle>().isOn)
            {
                clueNames.Add(clueToggles[iii].GetComponent<CourtClueToggleScript>().clueNameText.text);
            }
        }
        return clueNames;
    }

    public HashSet<string> ReadStatementResponses()
    {
        HashSet<string> statementNames = new HashSet<string>();
        for (int iii = 0; iii < statementToggles.Length; iii++)
        {
            if (statementToggles[iii].GetComponent<Toggle>().isOn)
            {
                statementNames.Add(statementToggles[iii].GetComponent<CourtStatementToggleScript>().statementNameText.text);
            }
        }
        return statementNames;
    }

    public void ToggleState() {
        isRisen = !isRisen;
        animator.SetBool("isRisen", isRisen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
