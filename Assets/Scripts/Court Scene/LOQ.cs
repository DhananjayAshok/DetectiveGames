using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptList {
    public AudioClip[] clipList;
    public Talker[] talkerList;

    public ScriptList(int length) {
        clipList = new AudioClip[length];
        talkerList = new Talker[length];
    }

    public void assign(int index, AudioClip clip, Talker talker) {
        clipList[index] = clip;
        talkerList[index] = talker;
    }

    public AudioClip getClip(int index) {
        return clipList[index];
    }

    public Talker getTalker(int index) {
        return talkerList[index];
    }

}

[System.Serializable]
public class LOQ
{
    public ScriptList preQuestioning;
    public ScriptList questioning;

    [Header("Question Information")]
    public string question;
    public int maxSelectableCluesAndStatements;
    public string[] correctCluenamesArray;
    public string[] correctStatementNamesArray;
    public int minimumCorrectCluesAndStatementsForSuccess;


    public ScriptList loqSuccess;
    public ScriptList loqFailure;

    ScriptList successScriptSequence;
    ScriptList failureScriptSequence;
    Talker lastTalker;
    [HideInInspector]
    public int state = 0;
    int noPreQuestioningClips;
    int noQuestioningClips;
    int noLOQSuccessClips;
    int noLOQFailureClips;
    int basic;
    int successLength;
    int failureLength;
    bool initialized = false;

    public void Initialize() {
        noPreQuestioningClips = preQuestioning.clipList.Length;
        noQuestioningClips = questioning.clipList.Length;
        noLOQSuccessClips = loqSuccess.clipList.Length;
        noLOQFailureClips = loqFailure.clipList.Length;
        basic = noPreQuestioningClips + noQuestioningClips;
        successLength = basic + noLOQSuccessClips;
        failureLength = basic + noLOQFailureClips;
        successScriptSequence = new ScriptList(successLength);
        failureScriptSequence = new ScriptList(failureLength);
        int iii = 0;
        int jjj = 0;
        for (int i = 0; i < noPreQuestioningClips; i++) {
            successScriptSequence.assign(iii, preQuestioning.getClip(i), preQuestioning.getTalker(i));
            failureScriptSequence.assign(jjj, preQuestioning.getClip(i), preQuestioning.getTalker(i));
            iii++;
            jjj++;
        }

        for (int i = 0; i < noQuestioningClips; i++)
        {
            successScriptSequence.assign(iii, questioning.getClip(i), questioning.getTalker(i));
            failureScriptSequence.assign(jjj, questioning.getClip(i), questioning.getTalker(i));
            iii++;
            jjj++;
        }
        for (int i = 0; i < noLOQSuccessClips; i++)
        {
            successScriptSequence.assign(iii, loqSuccess.getClip(i), loqSuccess.getTalker(i));
            iii++;
        }
        for (int i = 0; i < noLOQFailureClips; i++)
        {
            failureScriptSequence.assign(jjj, loqFailure.getClip(i), loqFailure.getTalker(i));
            jjj++;
        }
        initialized = true;
    }

    public bool isPreQuestioning() {
         return (state < noPreQuestioningClips);
    }

    public bool isQuestioning()
    {
        if (isPreQuestioning()) {
            return false;
        }
        return (state < basic);
    }

    public bool isFinished(bool success = false)
    {
        Debug.Log("State is: " + state.ToString() + ". SuccessLength is: " + successLength.ToString() + ". Failure Length is: " + failureLength.ToString());
        if (success)
        {
            return (state >= successLength);
        }
        else
        {
            return state >= failureLength;
        }
    }

    public bool isLOQConcluding(bool success = false)
    {
        if (isPreQuestioning())
        {
            return false;
        }
        if (isQuestioning()) {
            return false;
        }
        return !(isFinished(success));
    }



    public AudioClip Step(bool success = false) {
        if (!initialized || state == 0)
        {
            Initialize();
        }
        if (success)
        {
            AudioClip clip = successScriptSequence.getClip(state);
            lastTalker = successScriptSequence.getTalker(state);
            state++;
            return clip;
        }
        else {
            AudioClip clip = failureScriptSequence.getClip(state);
            lastTalker = failureScriptSequence.getTalker(state);
            state++;
            return clip;
        }
    }


    public Talker getTalkerFromMinistate(int ministate) {        
        if (ministate == 0 || ministate == 5) {
            return Talker.Player;
        }
        if (ministate == 1 || ministate == 4)
        {
            return Talker.Judge;
        }
        else {
            return Talker.Accused;
        }
    }

    public Talker getLastTalker() {
        return lastTalker;
    }

}
