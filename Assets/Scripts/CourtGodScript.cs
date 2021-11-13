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

    // Start is called before the first frame update
    void Start()
    {
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

    }

    // Update is called once per frame
    void Update()
    {
                
    }
}
