using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccusedScript : MonoBehaviour
{
    public string suspectName;
    public int minimumScoreForOverallSuccess;

    [Header("Clips for Successful Conclusion")]
    public AudioClip successfulConclusionClip;


    [Header("Clips for Failure Conclusion")]
    public AudioClip failureConclusionClip;


    public LOQ[] LOQs;
    [HideInInspector]
    public int noLOQs;
    [HideInInspector]
    public int currLOQ = 0;
    // Start is called before the first frame update
    void Start()
    {
        noLOQs = LOQs.Length;
    }

    public AudioClip getConclusionClip(bool success) {
        if (success) {
            return successfulConclusionClip;
        }
        return failureConclusionClip;
    }


    public LOQ getLOQ() {
        if (currLOQ >= noLOQs) {
            return null;
        }
        else
        {
            currLOQ++;
            return LOQs[currLOQ - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
