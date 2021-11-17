using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalScript : MonoBehaviour
{
    [Space(10)]
    [Header("Hyperparameters")]
    public float speed = 5;
    public float stayAnimationLength = 2.36f;
    public float jumpAnimationLength = 1f;
    GodScript godScript;
    PlayerPauseScript playerPauseScript;
    AudioManagementScript audioManagementScript;
    AudioSource audioSource;
    AudioGroup portalEntryClips;
    AudioGroup portalExitClips;
    AudioGroup portalActivateClips;
    AudioGroup round2SuccessPortalActivateClips;
    AudioGroup round2FailurePortalActivateClips;
    Animator animator;
    GameObject mainCamera;
    vThirdPersonCamera camScript;
    GameObject player;
    bool movingForward=false;

    // Start is called before the first frame update
    void Start()
    {
        playerPauseScript = GetComponent<PlayerPauseScript>();
        audioManagementScript = GetComponent<AudioManagementScript>();
        mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        camScript = mainCamera.GetComponent<vThirdPersonCamera>();
        portalEntryClips = audioManagementScript.portalEntryClips;
        portalExitClips = audioManagementScript.portalExitClips;
        portalActivateClips = audioManagementScript.portalActivateClips;
        round2SuccessPortalActivateClips = audioManagementScript.round2SuccessPortalActivateClips;
        round2FailurePortalActivateClips = audioManagementScript.round2FailurePortalActivateClips;
        godScript = GameObject.FindGameObjectsWithTag("God")[0].GetComponent<GodScript>(); // There should be one and only one God in the scene
        animator = this.transform.parent.gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        audioSource = GameObject.FindGameObjectsWithTag("InteractionSphere")[0].GetComponent<AudioSource>();
    }

    public void EnterPortal() {
        audioSource.clip = portalEntryClips.Sample();
        audioSource.Play();
    }

    public void ExitPortal() {
        audioSource.clip = portalExitClips.Sample();
        audioSource.Play();
    }

    public void ActivatePortal() {
        playerPauseScript.Freeze();
        camScript.enabled = false;
        if (godScript.roundNumber == 2)
        {
            if (godScript.accusedSuspectReasonable)
            {
                round2SuccessActivatePortal();
            }
            else {
                round2FailureActivatePortal();
            }
        }
        else {
            audioSource.clip = portalActivateClips.Sample();
            audioSource.Play();
            StartCoroutine(portalActivateAnimation());
        }
    }

    public void round2SuccessActivatePortal() {
        audioSource.clip = round2SuccessPortalActivateClips.Sample();
        audioSource.Play();
        StartCoroutine(round2SuccessPortalActivateAnimation());
    }

    IEnumerator round2SuccessPortalActivateAnimation()
    {
        animator.SetBool("isPortalActivated", true);
        animator.SetTrigger("PortalActivated");
        yield return new WaitForSeconds(stayAnimationLength);
        movingForward = true;
        yield return new WaitForSeconds(jumpAnimationLength);
        godScript.ProgressScene();
    }

    IEnumerator round2FailurePortalActivateAnimation()
    {
        animator.SetBool("isPortalActivated", true);
        animator.SetTrigger("PortalActivated");
        yield return new WaitForSeconds(stayAnimationLength);
        movingForward = true;
        yield return new WaitForSeconds(jumpAnimationLength);
        godScript.ProgressScene();
    }

    public void round2FailureActivatePortal() {
        audioSource.clip = portalActivateClips.Sample();
        audioSource.Play();
        StartCoroutine(round2FailurePortalActivateAnimation());
    }

    IEnumerator portalActivateAnimation() {
        animator.SetBool("isPortalActivated", true);
        animator.SetTrigger("PortalActivated");
        yield return new WaitForSeconds(stayAnimationLength);
        movingForward = true;
        yield return new WaitForSeconds(jumpAnimationLength);
        godScript.ProgressScene();
    }


    void MovePlayerForward() {
        player.transform.position += player.transform.forward * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingForward) {
            MovePlayerForward();
        }
    }
}
