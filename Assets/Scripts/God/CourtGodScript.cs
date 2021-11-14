using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtGodScript : MonoBehaviour
{
    [HideInInspector]
    public ClueObject[] discoveredClues;
    [HideInInspector]
    public string[] discoveredSuspects;
    [HideInInspector]
    public int noCluesDiscovered;
    [HideInInspector]
    public int noSuspectsDiscovered;
    [HideInInspector]
    public bool AreNoSpectators;
    GameObject player;
    GameObject judge;
    GameObject accused;
    GameObject[] spectators;
    CourtAnimationScript courtAnimationScript;

    // Start is called before the first frame update
    void Start()
    {
        courtAnimationScript = GetComponent<CourtAnimationScript>();
    }

    public void Initialize(ClueObject[] discoveredClues, int noCluesDiscovered, string[] discoveredSuspects, int noSuspectsDiscovered) {
        this.discoveredClues = discoveredClues;
        this.noCluesDiscovered = noCluesDiscovered;
        this.discoveredSuspects = discoveredSuspects;
        this.noSuspectsDiscovered = noSuspectsDiscovered;
        player = GameObject.FindGameObjectWithTag("Player");
        judge = GameObject.FindGameObjectWithTag("Judge");
        accused = GameObject.FindGameObjectWithTag("Accused");
        spectators = GameObject.FindGameObjectsWithTag("spectator");
        if (spectators == null)
        {
            AreNoSpectators = true;
        }
        else {
            AreNoSpectators = (spectators[0] == null);
        }
        InitializeListeners();
    }

    void InitializeListeners() {
        courtAnimationScript.Initialize(player, judge, accused, spectators);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            courtAnimationScript.AnimatePlayerTalking();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            courtAnimationScript.AnimateJudgeTalking();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            courtAnimationScript.AnimateAccusedTalking();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            courtAnimationScript.AnimateAccusedQuestioning();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            courtAnimationScript.AnimatePlayerResponding();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            courtAnimationScript.AnimateLOQConcluding(true);
        }
    }
}
