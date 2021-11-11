using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodClueScript : MonoBehaviour
{
    public int noClues = 10; // Should store the number of different clues that could be added. 
    public int noAvailableAutopsies = 5; // Should store the maximum number of allowable autopsies. 
    [HideInInspector]
    public int noAutopsiesPerformed = 0;
    [HideInInspector]
    public int noCluesDiscovered = 0;
    [HideInInspector]
    public ClueObject[] discoveredClues;
    [HideInInspector]
    public int roundNumber=1;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        discoveredClues = new ClueObject[noClues];
    }

    public void addClue(ClueScript cs) {
        if (cs.isTriviallyUseless) { }
        else
        {
            discoveredClues[noCluesDiscovered] = new ClueObject(cs, noCluesDiscovered);
            noCluesDiscovered++;
        }
    }

    public int PerformAutopsy(int index) {
        if (noAvailableAutopsies <= noAutopsiesPerformed) {
            return 0;
        }
        discoveredClues[index].isAutopsied = true;
        noAutopsiesPerformed++;
        if (discoveredClues[index].isAutopsySuccess)
        {
            return 2;
        }
        else {
            return 1;
        }
    }

    public void ProgressScene() {
        if (roundNumber == 1)
        {
            roundNumber = 2;
            SceneManager.LoadScene(1); // should be 2 if you have a main menu
        }
        else if (roundNumber == 2) {
            roundNumber = 3;
            SceneManager.LoadScene(3);
        }       
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
