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
    AudioSource playerAudioSource;
    AudioSource judgeAudioSource;
    AudioSource accusedAudioSource;
    CourtAnimationScript courtAnimationScript;
    CourtSceneCanvasScript courtSceneCanvasScript;
    bool isConcluding;
    bool isInLOQ;
    bool isAwaitingResponse;
    LOQ currLOQ;
    HashSet<string> currClueNames, currStatementNames;
    int score;
    bool overallSuccess;
    bool initialized;
    bool hasConcluded;
    float conversationWaitTime = 1f;

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
        playerAudioSource = player.GetComponent<AudioSource>();
        judgeAudioSource = judge.GetComponent<AudioSource>();
        accusedAudioSource = accused.GetComponent<AudioSource>();
        if (spectators == null)
        {
            AreNoSpectators = true;
        }
        else {
            AreNoSpectators = (spectators[0] == null);
        }
        initialized = true;
        InitializeListeners();
    }




    void InitializeListeners() {
        Debug.Log(courtAnimationScript);
        Debug.Log(player);
        courtAnimationScript.Initialize(player, judge, accused, spectators);
        courtSceneCanvasScript = GameObject.FindGameObjectWithTag("CourtSceneCanvas").GetComponent<CourtSceneCanvasScript>();
        courtSceneCanvasScript.Initialize(this, this.discoveredClues, this.noCluesDiscovered, this.discoveredSuspects, this.noSuspectsDiscovered);
    }

    public void SubmitResponse() {
        isAwaitingResponse = false;
        currClueNames = courtSceneCanvasScript.ReadClueResponses();
        currStatementNames = courtSceneCanvasScript.ReadStatementResponses();
        courtSceneCanvasScript.ToggleState();
        EndLOQ();
    }

    public void StartLOQ()
    {
        isInLOQ = true;
        LOQ nextLOQ = accused.GetComponent<AccusedScript>().getLOQ();
        if (nextLOQ == null)
        {
            isConcluding = true;
            isInLOQ = false;

            return;
        }
        else
        {
            StartCoroutine(StartLOQCoroutine(nextLOQ));
        }
    }

    void Speak(Talker talker, AudioClip clip)
    {
        AudioSource source;
        if (talker == Talker.Player)
        {
            source = playerAudioSource;
        }
        else if (talker == Talker.Judge)
        {
            source = judgeAudioSource;
        }
        else if (talker == Talker.Accused)
        {
            source = accusedAudioSource;
        }
        else
        {
            source = playerAudioSource;
            Debug.Log("Error");
        }
        source.clip = clip;
        source.Play();
    }

    IEnumerator StartLOQCoroutine(LOQ nextLOQ)
    {
        currLOQ = nextLOQ;
        while (nextLOQ.state == 0)
        {
            AudioClip nextClip = nextLOQ.Step();
            Talker nextTalker = nextLOQ.getLastTalker();
            Speak(nextTalker, nextClip);
            courtAnimationScript.AnimateTalking(nextTalker);
            yield return new WaitForSeconds(nextClip.length + conversationWaitTime);
            courtAnimationScript.AnimateIdling();
        }
        courtAnimationScript.AnimateAccusedQuestioning();
        while (nextLOQ.state == 1)
        {
            AudioClip nextClip = nextLOQ.Step();
            Talker nextTalker = nextLOQ.getLastTalker();
            Speak(nextTalker, nextClip);
            yield return new WaitForSeconds(nextClip.length + conversationWaitTime);
        }
        isAwaitingResponse = true;
        courtSceneCanvasScript.UpdateInfo(nextLOQ.question, nextLOQ.maxSelectableCluesAndStatements);
        courtSceneCanvasScript.ToggleState();
    }

    bool JudgeSuccess() {
        currClueNames.IntersectWith(currLOQ.correctClueNames);
        currStatementNames.IntersectWith(currLOQ.correctStatementNames);
        if (currClueNames.Count + currStatementNames.Count >= currLOQ.minimumCorrectCluesAndStatementsForSuccess)
        {
            return true;
        }
        else {
            return false;
        }
    }

    bool JudgeOverallSuccess() {
        if (score > accused.GetComponent<AccusedScript>().minimumScoreForOverallSuccess) {
            overallSuccess = true;
            return true;
        }
        overallSuccess = false;
        return false;
    }

    void EndLOQ()
    {
        bool success = JudgeSuccess();
        if (success) {
            score++;
        }
        StartCoroutine(EndLOQCoroutine(success));
    }

    IEnumerator EndLOQCoroutine(bool success) {
        courtAnimationScript.AnimateLOQConcluding(success);
        while (currLOQ.state == 2)
        {
            AudioClip nextClip = currLOQ.Step(success);
            Talker nextTalker = currLOQ.getLastTalker();
            Speak(nextTalker, nextClip);
            yield return new WaitForSeconds(nextClip.length + conversationWaitTime);
        }
        courtAnimationScript.AnimateIdling();
        isInLOQ = false;
    }

    void Conclude() {
        hasConcluded = true;
        JudgeOverallSuccess();
        playerAudioSource.clip = accused.GetComponent<AccusedScript>().getConclusionClip(overallSuccess);
        playerAudioSource.Play();
        courtAnimationScript.AnimateConcluding(overallSuccess);
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized && !isInLOQ && !isAwaitingResponse && !isConcluding) {
            StartLOQ();
        }
        else if (!hasConcluded && isConcluding) {
            Conclude();
        }
    }
}
