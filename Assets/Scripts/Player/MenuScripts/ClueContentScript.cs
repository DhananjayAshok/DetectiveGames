using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueContentScript : MonoBehaviour
{
    [Header("Internal Variables (Can ignore)")]
    public GameObject clueButtonPrefab;
    public GameObject clueContent;
    [HideInInspector]
    public int noCluesDiscovered = 0;
    private GodScript godScript;

    // Start is called before the first frame update
    void Start()
    {
        godScript = GameObject.FindGameObjectsWithTag("God")[0].GetComponent<GodScript>(); // There should be one and only one God in the scene
        RefreshClues();
    }

    // Update is called once per frame
    void Update()
    {
        if (godScript.noCluesDiscovered != noCluesDiscovered) {
            
        }
    }

    public void CreateClueRecord(ClueScript cs, int index) {
        CreateClueRecord(new ClueObject(cs, index));    
    }

    public void CreateClueRecord(ClueObject co) {
        GameObject button = Instantiate(clueButtonPrefab, clueContent.transform);
        ClueButtonScript clueButtonScript = button.GetComponent<ClueButtonScript>();
        clueButtonScript.UpdateButtonInfo(co);
        noCluesDiscovered++;
    }

    public void RefreshClues() {
        foreach (Transform child in clueContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        noCluesDiscovered = 0;
        int noClues = godScript.noCluesDiscovered;
        for (int iii = 0; iii < noClues; iii++) {
            ClueObject clueObject = godScript.discoveredClues[iii];
            CreateClueRecord(clueObject);
        }
    }
}
