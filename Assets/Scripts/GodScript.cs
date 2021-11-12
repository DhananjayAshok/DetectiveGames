using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodScript : MonoBehaviour
{
    public int noClues = 10; // Should store the number of different clues that could be added. 
    public int noSuspects = 50; // Should store the maximum number of suspects+witnesses+humans you can speak to. 
    public int noAvailableAutopsies = 5; // Should store the maximum number of allowable autopsies. 
    public int noAvailableConfrontations = 5; // Should store the maximum number of confrontations per suspect.
    [HideInInspector]
    public int noAutopsiesPerformed = 0;
    [HideInInspector]
    public int noCluesDiscovered = 0;
    public int noSuspectsDiscovered = 0;
    [HideInInspector]
    public ClueObject[] discoveredClues;
    public string[] discoveredSuspects;
    [HideInInspector]
    public int roundNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        discoveredClues = new ClueObject[noClues];
        discoveredSuspects = new string[noSuspects];
    }

    public void addClue(ClueScript cs)
    {
        if (cs.isTriviallyUseless) { }
        else
        {
            discoveredClues[noCluesDiscovered] = new ClueObject(cs, noCluesDiscovered);
            noCluesDiscovered++;
        }
    }

    public void addSuspect(SuspectScript ss) {
        if (noSuspectsDiscovered >= noSuspects) {
            Debug.Log("Too many suspects");
            return;
        }
        for (int iii = 0; iii < noSuspectsDiscovered; iii++) {
            if (discoveredSuspects[iii].Equals(ss.suspectName)) {
                return;
            }
        }
        discoveredSuspects[noSuspectsDiscovered] = ss.suspectName;
        noSuspectsDiscovered++;
    }

    public int PerformAutopsy(int index)
    {
        if (noAvailableAutopsies <= noAutopsiesPerformed)
        {
            return 0;
        }
        discoveredClues[index].isAutopsied = true;
        noAutopsiesPerformed++;
        if (discoveredClues[index].isAutopsySuccess)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    public void ProgressScene()
    {
        if (roundNumber == 1)
        {
            roundNumber = 2;
            SceneManager.LoadScene(1); // should be 2 if you have a main menu
        }
        else if (roundNumber == 2)
        {
            roundNumber = 3;
            SceneManager.LoadScene(3);
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int iii = 0; iii < noCluesDiscovered; iii++) {
            Debug.Log(iii);
            Debug.Log(discoveredClues[iii]);
        }
    }
}
