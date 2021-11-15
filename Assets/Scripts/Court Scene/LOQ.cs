using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LOQ
{

    [Header("Clips to play in order before questioning")]
    public SkippableAudioClip playerClip0;
    public SkippableAudioClip judgeClip0;
    public SkippableAudioClip accusedClip0;
    public SkippableAudioClip accusedClip1;
    public SkippableAudioClip judgeClip1;
    public SkippableAudioClip playerClip1;

    [Header("Clips for Questioning")]
    public SkippableAudioClip playerClip2;
    public SkippableAudioClip judgeClip2;
    public SkippableAudioClip accusedClip2;
    public SkippableAudioClip accusedClip3;
    public SkippableAudioClip judgeClip3;
    public SkippableAudioClip playerClip3;

    [Header("Clips for Successful LOQ Conclusion")]
    public SkippableAudioClip playerClip4;
    public SkippableAudioClip judgeClip4;
    public SkippableAudioClip accusedClip4;
    public SkippableAudioClip accusedClip5;
    public SkippableAudioClip judgeClip5;
    public SkippableAudioClip playerClip5;

    [Header("Clips for Failure LOQ Conclusion")]
    public SkippableAudioClip playerClip6;
    public SkippableAudioClip judgeClip6;
    public SkippableAudioClip accusedClip6;
    public SkippableAudioClip accusedClip7;
    public SkippableAudioClip judgeClip7;
    public SkippableAudioClip playerClip7;


    [Header("Question Information")]
    public string question;
    public int maxSelectableCluesAndStatements;
    public HashSet<string> correctClueNames;
    public HashSet<string> correctStatementNames;
    public string[] correctCluenamesArray;
    public string[] correctStatementNamesArray;
    public int minimumCorrectCluesAndStatementsForSuccess;

    SkippableAudioClip[] beforeClips, questioningClips, LOQSuccessClips, LOQFailureClips;

    [HideInInspector]
    public int state = 0;
    [HideInInspector]
    public int ministate = 0;
    [HideInInspector]
    public int last_state = 0;
    [HideInInspector]
    public int last_ministate = 0;
    bool initialized = false;

    public void Initialize() {
        correctClueNames = new HashSet<string>(correctCluenamesArray);
        correctStatementNames = new HashSet<string>(correctStatementNamesArray);
        beforeClips = new SkippableAudioClip[]{playerClip0, judgeClip0,  accusedClip0, accusedClip1, judgeClip1, playerClip1};
        questioningClips = new SkippableAudioClip[] { playerClip2, judgeClip2, accusedClip2, accusedClip3, judgeClip3, playerClip3};
        LOQSuccessClips = new SkippableAudioClip[] { playerClip4, judgeClip4, accusedClip4, accusedClip5, judgeClip5, playerClip5};
        LOQFailureClips = new SkippableAudioClip[] { playerClip6, judgeClip6, accusedClip6, accusedClip7, judgeClip7, playerClip7};
        initialized = true;
    }

    public bool ArrayhasNextClip(SkippableAudioClip[] clips) {
        for (int iii = 0; iii < clips.Length; iii++)
        {
            if (ministate <= iii && !clips[iii].skip)
            {
                return true;
            }
        }
        return false;
    }

    public AudioClip ArrayStep(SkippableAudioClip[] clips) {
        for (int iii = 0; iii < clips.Length; iii++) {
            if (ministate <= iii) {
                if (!clips[iii].skip) {
                    last_ministate = iii;
                    ministate = iii+1;
                    if (ministate == clips.Length)
                    {
                        last_state = state;
                        state++;
                        ministate = 0;
                    }
                    else if (!ArrayhasNextClip(clips))
                    {
                        last_state = state;
                        state++;
                        ministate = 0;
                    }
                    return clips[iii].clip;
                }
            }
        }
        Debug.Log("Error");
        return clips[0].clip;
    }

    public AudioClip Step(bool success = false)
    {
        if (!initialized)
        {
            Initialize();
        }
        if (state == 0) // before
        {
            if (ArrayhasNextClip(beforeClips))
            {
                return ArrayStep(beforeClips);
            }
            else
            {
                Debug.Log("Error");
                return ArrayStep(beforeClips);
            }
        }
        else if (state == 1)
        {
            if (ArrayhasNextClip(questioningClips))
            {
                return ArrayStep(questioningClips);
            }
            else
            {
                Debug.Log("Error");
                return ArrayStep(questioningClips);
            }
        }
        else if (state == 2)
        {
            if (success)
            {
                if (ArrayhasNextClip(LOQSuccessClips))
                {
                    return ArrayStep(LOQSuccessClips);
                }
                else
                {
                    Debug.Log("Error");
                    return ArrayStep(questioningClips);
                }
            }
            else
            {
                if (ArrayhasNextClip(LOQFailureClips))
                {
                    return ArrayStep(LOQFailureClips);
                }
                else
                {
                    Debug.Log("Error");
                    return ArrayStep(questioningClips);
                }

            }
        }
        else if (state == 3)
        {
            Debug.Log("Error");
            return ArrayStep(questioningClips);
        }
        else {
            Debug.Log("Mega Error");
            return ArrayStep(questioningClips);
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
        return getTalkerFromMinistate(last_ministate);
    }

}
