using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueContentScript : MonoBehaviour
{
    public GameObject clueButtonPrefab;
    public GameObject clueContent;
    public int noCluesDiscovered = 0;
    private GodClueScript godClueScript;

    // Start is called before the first frame update
    void Start()
    {
        godClueScript = GameObject.FindGameObjectsWithTag("God")[0].GetComponent<GodClueScript>(); // There should be one and only one God in the scene
    }

    // Update is called once per frame
    void Update()
    {
        if (godClueScript.noCluesDiscovered != noCluesDiscovered) {
            
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
        int noClues = godClueScript.noCluesDiscovered;
        for (int iii = 0; iii < noClues; iii++) {
            ClueObject clueObject = godClueScript.discoveredClues[iii];
            CreateClueRecord(clueObject);
        }
    }
}
