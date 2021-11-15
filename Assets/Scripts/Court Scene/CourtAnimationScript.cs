using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Talker
{
    Player,
    Judge,
    Accused
}

public class CourtAnimationScript : MonoBehaviour
{
    
    [Header("Player Animation Info")]

    [Header("From the Animation Controllers in Scene 3")]
    public int noPlayerTalkingClips = 2;
    public int noPlayerQuestionedClips = 2;
    public int noPlayerRespondingClips = 2;
    public int noPlayerLOQConclusionSuccessClips = 2;
    public int noPlayerLOQConclusionFailureClips = 2;

    [Space(10)]
    [Header("Judge Animation Info")]
    public int noJudgeTalkingClips = 2;
    public int noJudgeThinkingClips = 2;
    public int noJudgeLOQConclusionSuccessClips = 2;
    public int noJudgeLOQConclusionFailureClips = 2;

    [Space(10)]
    [Header("Accused Animation Info")]
    public int noAccusedTalkingClips = 2;
    public int noAccusedQuestioningClips = 2;
    public int noAccusedRespondedClips = 2;
    public int noAccusedLOQConclusionSuccessClips = 2;
    public int noAccusedLOQConclusionFailureClips = 2;

    [Space(10)]
    [Header("Spectator Animation Info")]
    public int noSpectatorQuestioningClips = 2;
    public int noSpectatorRespondingClips = 2;
    public int noSpectatorLOQConclusionSuccessClips = 2;
    public int noSpectatorLOQConclusionFailureClips = 2;
    public int noSpectatorConclusionSuccessClips = 2;
    public int noSpectatorConclusionFailureClips = 2;


    bool isPlayerIdling;
    bool isJudgeIdling;
    bool isAccusedIdling;
    bool playerTalking;
    bool judgeTalking;
    bool accusedTalking;
    bool accusedQuestioning = false;
    bool playerResponding = false;
    bool isLOQConcluding = false;
    bool isLOQSuccess = false;
    bool isConcluding = false;
    bool isConclusionSuccess = false;
    


    [HideInInspector]
    public Animator playerAnimator, judgeAnimator, accusedAnimator;
    [HideInInspector]
    public Animator[] spectatorAnimators;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void Initialize(GameObject player, GameObject judge, GameObject accused, GameObject[] spectators) {
        playerAnimator = player.GetComponent<Animator>();
        judgeAnimator = judge.GetComponent<Animator>();
        accusedAnimator = accused.GetComponent<Animator>();
        spectatorAnimators = new Animator[spectators.Length];
        for (int iii = 0; iii < spectators.Length; iii++) {
            spectatorAnimators[iii] = spectators[iii].GetComponent<Animator>();
        }
        Randomize();
    }


    public void AnimateIdling() {
        SetAllIdlingState();
        SetAnimatorVars();
    }

    public void AnimateTalking(Talker talker) {
        SetTalkingState(talker);
        SetAnimatorVars();
    }

    public void AnimatePlayerTalking() {
        SetTalkingState(Talker.Player);
        SetAnimatorVars();
    }

    public void AnimateJudgeTalking() {
        SetTalkingState(Talker.Judge);
        SetAnimatorVars();
    }

    public void AnimateAccusedTalking()
    {
        SetTalkingState(Talker.Accused);
        SetAnimatorVars();
    }

    public void AnimateAccusedQuestioning() {
        SetAccusedQuestioningState();
        SetAnimatorVars();
    }

    public void AnimatePlayerResponding()
    {
        SetPlayerRespondingState();
        SetAnimatorVars();
    }

    public void AnimateLOQConcluding(bool success)
    {
        SetLOQConcludingState(success);
        SetAnimatorVars();
    }

    public void AnimateConcluding(bool success) {
        SetConcludingState(success);
        SetAnimatorVars();
    }

    #region State Setters

    void SetAllIdlingState() {
        ResetAll();
        isPlayerIdling = true;
        isJudgeIdling = true;
        isAccusedIdling = true;
    }
    void SetAccusedQuestioningState() {
        ResetAll();
        accusedQuestioning = true;
    }

    void SetPlayerRespondingState()
    {
        ResetAll();
        playerResponding = true;
    }

    void SetLOQConcludingState(bool success)
    {
        ResetAll();
        isLOQConcluding = true;
        isLOQSuccess = success;
    }

    void SetConcludingState(bool success)
    {
        ResetAll();
        isConcluding = true;
        isConclusionSuccess = success;
    }

    void SetTalkingState(Talker talker) {
        ResetAll();
        isPlayerIdling = true;
        isJudgeIdling = true;
        isAccusedIdling = true;

        if (talker == Talker.Player)
        {
            playerTalking = true;
            isPlayerIdling = false;
        }
        else if (talker == Talker.Judge)
        {
            judgeTalking = true;
            isJudgeIdling = false;

        }
        else if (talker == Talker.Accused)
        {
            accusedTalking = true;
            isAccusedIdling = false;
        }
        else
        {
            Debug.Log("error");
        }
    }

    void ResetAll() {
        playerTalking = false;
        judgeTalking = false;
        accusedTalking = false;
        isPlayerIdling = false;
        isJudgeIdling = false;
        isAccusedIdling = false;
        accusedQuestioning = false;
        playerResponding = false;
        isLOQConcluding = false;
        isLOQSuccess = false;
        isConcluding = false;
        isConclusionSuccess = false;
    }
    #endregion

    #region AnimationRandomization Setters

    public void Randomize() {
        RandomizeInts();
        RandomizeFloats();
    }

    public void RandomizeInts() {
        RandomizePlayerInts();
        RandomizeJudgeInts();
        RandomizeAccusedInts();
        RandomizeSpectatorInts();
    }

    public void RandomizePlayerInts() {
        if (judgeTalking || accusedTalking)
        {
            return; // No state change based on random ints
        }
        else if (playerTalking) {
            SetRandInt(playerAnimator, noPlayerTalkingClips);
            return;
        }
        if (accusedQuestioning) {
            SetRandInt(playerAnimator, noPlayerQuestionedClips);
            return;
        }
        if (playerResponding) {
            SetRandInt(playerAnimator, noPlayerRespondingClips);
            return;
        }
        if (isLOQConcluding) {
            if (isLOQSuccess)
            {
                SetRandInt(playerAnimator, noPlayerLOQConclusionSuccessClips);
                return;
            }
            else {
                SetRandInt(playerAnimator, noPlayerLOQConclusionFailureClips);
                return;
            }
        }
        if (isConcluding) {
            return;
        }
    }

    public void RandomizeJudgeInts()
    {
        if (playerTalking || accusedTalking)
        {
            return; // No state change based on random ints
        }
        else if (judgeTalking)
        {
            SetRandInt(judgeAnimator, noJudgeTalkingClips);
            return;
        }
        if (accusedQuestioning || playerResponding)
        {
            SetRandInt(judgeAnimator, noJudgeThinkingClips);
            return;
        }
        if (isLOQConcluding)
        {
            if (isLOQSuccess)
            {
                SetRandInt(judgeAnimator, noJudgeLOQConclusionSuccessClips);
                return;
            }
            else
            {
                SetRandInt(judgeAnimator, noJudgeLOQConclusionFailureClips);
                return;
            }
        }
        if (isConcluding)
        {
            return;
        }
    }

    public void RandomizeAccusedInts()
    {
        if (judgeTalking || playerTalking)
        {
            return; // No state change based on random ints
        }
        else if (accusedTalking)
        {
            SetRandInt(accusedAnimator, noAccusedTalkingClips);
            return;
        }
        if (accusedQuestioning)
        {
            SetRandInt(accusedAnimator, noAccusedQuestioningClips);
            return;
        }
        if (playerResponding)
        {
            SetRandInt(accusedAnimator, noAccusedRespondedClips);
            return;
        }
        if (isLOQConcluding)
        {
            if (isLOQSuccess)
            {
                SetRandInt(accusedAnimator, noAccusedLOQConclusionSuccessClips);
                return;
            }
            else
            {
                SetRandInt(accusedAnimator, noAccusedLOQConclusionFailureClips);
                return;
            }
        }
        if (isConcluding)
        {
            return;
        }
    }

    public void RandomizeSpectatorInts()
    {
        if (judgeTalking || playerTalking || accusedTalking)
        {
            return; // No state change based on random ints
        }
        if (accusedQuestioning)
        {
            SetRandIntSpectator(noSpectatorQuestioningClips);
            return;
        }
        if (playerResponding)
        {
            SetRandIntSpectator(noSpectatorRespondingClips);
            return;
        }
        if (isLOQConcluding)
        {
            if (isLOQSuccess)
            {
                SetRandIntSpectator(noSpectatorLOQConclusionSuccessClips);
                return;
            }
            else
            {
                SetRandIntSpectator(noSpectatorLOQConclusionFailureClips);
                return;
            }
        }
        if (isConcluding)
        {
            return;
        }
    }

    public void RandomizeFloats()
    {
        float value = Random.Range(0f, 1f);
        playerAnimator.SetFloat("RNGFloat", value);
        judgeAnimator.SetFloat("RNGFloat", value);
        accusedAnimator.SetFloat("RNGFloat", value);
        RandomizeSpectatorFloats();
    }

    public void RandomizeSpectatorFloats() {
        for (int iii = 0; iii < spectatorAnimators.Length; iii++)
        {
            float value = Random.Range(0f, 1f);
            spectatorAnimators[iii].SetFloat("RNGFloat", value);
        }
    }
    #endregion

    #region Specific Animation Setter Functions

    void SetAnimatorVars()
    {
        // Must be called only after state space is fully updated. 
        Randomize();
        SetTalking();
        SetAccusedQuestioning();
        SetPlayerResponding();
        SetLOQConcluding();
        SetLOQSuccess();
        SetConcluding();
        SetConclusionSuccess();
    }

    void SetTalking() {
        // Assumes the current state is updated in bool information of this script. 
        playerAnimator.SetBool("isTalking", playerTalking);
        judgeAnimator.SetBool("isTalking", judgeTalking);
        accusedAnimator.SetBool("isTalking", accusedTalking);
        playerAnimator.SetBool("isIdling", isPlayerIdling);
        judgeAnimator.SetBool("isIdling", isJudgeIdling);
        accusedAnimator.SetBool("isIdling", isAccusedIdling);
        SetBoolSpectators("isIdling", (isPlayerIdling || isJudgeIdling || isAccusedIdling));
    }

    void SetAccusedQuestioning() {
        SetBoolAll("isAccusedQuestioning", accusedQuestioning);
    }

    void SetPlayerResponding() {
        SetBoolAll("isPlayerResponding", playerResponding);
    }

    void SetLOQConcluding()
    {
        SetBoolAll("isLOQConcluding", isLOQConcluding);
    }

    void SetLOQSuccess()
    {
        SetBoolAll("isLOQSuccess", isLOQSuccess);
    }

    void SetConcluding()
    {
        SetBoolAll("isConcluding", isConcluding);
    }

    void SetConclusionSuccess()
    {
        SetBoolAll("isConclusionSuccess", isConclusionSuccess);
    }
    #endregion

    #region Generic Animation Setter Functions
    void SetRandInt(Animator animator, int bound)
    {

        animator.SetInteger("RNGInt", Random.Range(0, bound));
    }

    void SetRandIntSpectator(int bound) {
        for (int iii = 0; iii < spectatorAnimators.Length; iii++)
        {
            int value = Random.Range(0, bound);
            spectatorAnimators[iii].SetInteger("RNGInt", value);
        }
    }

    void SetBoolSpectators(string varName, bool value) {
        for (int iii = 0; iii < spectatorAnimators.Length; iii++)
        {
            spectatorAnimators[iii].SetBool(varName, value);
        }
    }

    void SetFloatSpectators(string varName, float value)
    {
        for (int iii = 0; iii < spectatorAnimators.Length; iii++)
        {
            spectatorAnimators[iii].SetFloat(varName, value);
        }
    }

    void SetIntegerSpectators(string varName, int value)
    {
        for (int iii = 0; iii < spectatorAnimators.Length; iii++)
        {
            spectatorAnimators[iii].SetInteger(varName, value);
        }
    }

    void SetBoolAll(string state, bool value) {
        playerAnimator.SetBool(state, value);
        judgeAnimator.SetBool(state, value);
        accusedAnimator.SetBool(state, value);
        SetBoolSpectators(state, value);
    }


    #endregion


    // Update is called once per frame
    void Update()
    {
        
    }
}
