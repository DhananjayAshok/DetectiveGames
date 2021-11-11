using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagementScript : MonoBehaviour
{
    [Header("Clue Interaction Clips")]
    public AudioGroup bugClips;
    public AudioGroup foundClueClips;
    public AudioGroup clueBookClips;

    [Space(10)]
    [Header("Suspect Interaction Clips")]
    public AudioGroup foundSuspectClips;
    public AudioGroup interactWithInnocentAccusedClips;
    public AudioGroup interactWithGuiltyAccusedClips;
    public AudioGroup accuseClips;
    public AudioGroup leaveConversationClips;

    [Space(10)]
    [Header("Narrator Clips")]
    public AudioGroup narratorIntroductionClips;
    public AudioGroup narratorSuccessClips;
    public AudioGroup narratorFailureClips;


    [Space(10)]
    [Header("Menu Background Music Clips")]
    public AudioGroup pauseMenuClips;
    public AudioGroup mainMenuClips;


    [Space(10)]
    [Header("Menu Clicks and Transition Clips")]
    public AudioGroup clueClickClips;
    public AudioGroup backClickClips;
    public AudioGroup transitionClips;
    public AudioGroup transitionBackClips;

    [Space(10)]
    [Header("Lab Request SFX Clips")]
    public AudioGroup autopsyRequestClips;
    public AudioGroup autopsyFailClips;
    public AudioGroup autopsySuccessClips;
    public AudioGroup autopsyErrorClips;

    [Space(10)]
    [Header("Confrontation Clips")]
    public AudioGroup confrontClips;
    public AudioGroup reconfrontClips;
    public AudioGroup leaveSuccessfulConfrontationClips;
    public AudioGroup leaveUnsuccessfulConfrontationClips;

    [Space(10)]
    [Header("Portal Clips")]
    public AudioGroup portalEntryClips;
    public AudioGroup portalExitClips;
    public AudioGroup portalActivateClips;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
