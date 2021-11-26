using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodScript : MonoBehaviour
{
    [Header("Game Variables")]
    [Tooltip("Max Number of clues that can be found")]
    public int noClues = 10;
    [Tooltip("Max Number of suspects that could be found")]
    public int noSuspects = 50; // Should store the maximum number of suspects+witnesses+humans you can speak to. 
    [Tooltip("Max Number of Lab Reports available")]
    public int noAvailableAutopsies = 5; // Should store the maximum number of allowable autopsies. 
    [Tooltip("Max Number of Confrontations available per suspect")]
    public int noAvailableConfrontations = 5; // Should store the maximum number of confrontations per suspect.

    [Space(10)]
    [Header("Scene Management Variables")]
    [Tooltip("Scene Name of the first scene part of the Scene 2 set")]
    public string scene2First = "Scene 2";
    [Tooltip("Scene Name of the first scene part of the Scene 3/ Courtroom set")]
    public string scene3First = "Scene 3";
    [Tooltip("Scene Name of the loading scene if any.")]
    public string loadingScene = "null";

    [HideInInspector]
    public int noAutopsiesPerformed = 0;
    [HideInInspector]
    public int noCluesDiscovered = 0;
    [HideInInspector]
    public int noSuspectsDiscovered = 0;
    [HideInInspector]
    public ClueObject[] discoveredClues;
    [HideInInspector]
    public string[] discoveredSuspects;
    [HideInInspector]
    public int roundNumber = 1;
    [HideInInspector]
    public string accusedSuspect;
    [HideInInspector]
    public bool accusedSuspectReasonable;
    [HideInInspector]
    public string nextScene = null;
    CourtGodScript courtGodScript;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        discoveredClues = new ClueObject[noClues];
        discoveredSuspects = new string[noSuspects];
        courtGodScript = GetComponent<CourtGodScript>();
    }

    public void addClue(ClueScript cs)
    {
        if (cs.isTriviallyUseless) { }
        else
        {
            for (int iii = 0; iii < noCluesDiscovered; iii++)
            {
                if (discoveredClues[iii].clueName.Equals(cs.clueName))
                {
                    return;
                }
            }
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
            roundNumber = 2; // Should be 2
            nextScene = scene2First;
            SendToLoadingScene();
        }
        else if (roundNumber == 2)
        {
            if (accusedSuspectReasonable)
            {
                roundNumber = 3;
                nextScene = scene3First;
                SendToLoadingScene();
                SceneManager.sceneLoaded += OnCourtroomSceneLoaded;
            }
            else {
                Debug.Log("Not Yet Implemented");
            }
        }
    }

    public void SceneChangeInternal(string nextScene) {
        this.nextScene = nextScene;
        SendToLoadingScene();
    }

    public void SendToLoadingScene() {
        if (loadingScene == null)
        {
            SceneManager.LoadScene(nextScene);
        }
        else if (loadingScene == "null")
        {
            SceneManager.LoadScene(nextScene);
        }
        else {
            SceneManager.LoadScene(loadingScene);
        }
    }


    public void DeleteNonAccusedObjects() {
        GameObject[] accusedObjects = GameObject.FindGameObjectsWithTag("Accused");
        for (int iii = 0; iii < accusedObjects.Length; iii++) {
            if (accusedObjects[iii].GetComponent<AccusedScript>().suspectName.Equals(accusedSuspect))
            {

            }
            else {
                Destroy(accusedObjects[iii]);
            }
        }
    }

    void OnCourtroomSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (roundNumber == 3) {
            DeleteNonAccusedObjects();
            courtGodScript.Initialize(discoveredClues, noCluesDiscovered, discoveredSuspects, noSuspectsDiscovered);
        }
    }



    // Update is called once per frame
    void Update()
    {
  
    }
}
