using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.vCharacterController;

public class PlayerPauseScript : MonoBehaviour
{
    private Animator animator;
    private vThirdPersonController thirdPersonController;
    private vThirdPersonInput thirdPersonInput;
    private GameObject playerObject;
    private Vector3 prevPosition;
    private Quaternion prevRotation;
    private MenuAudioScript menuAudioScript;
    private MenuAnimationScript menuAnimationScript;

    [HideInInspector]
    public bool isPaused;
    [HideInInspector]
    public bool isMiniPaused;
    [HideInInspector]
    public bool isInConversation;

    Transform playerPausePoint;

    [Space(10)]
    [Header("Internal Variables (Can ignore)")]
    public Camera pauseCam;
    public Canvas mainCanvas;
    public Canvas pauseCanvas;
    public GameObject pauseCanvasBackground;

    void SaveTransform() {
        prevPosition = playerObject.transform.position;
        prevRotation = playerObject.transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerObject = this.transform.parent.gameObject;
        thirdPersonController = playerObject.GetComponent<vThirdPersonController>();
        thirdPersonInput = playerObject.GetComponent<vThirdPersonInput>();
        animator = playerObject.GetComponent<Animator>();
        menuAudioScript = GetComponent<MenuAudioScript>();
        menuAnimationScript = GetComponent<MenuAnimationScript>();
        playerPausePoint = GameObject.FindGameObjectsWithTag("PlayerPausePoint")[0].transform;
    }   

    void Process() {
        if (isPaused)
        {
            isPaused = false;
            UnPauseProcess();

        }
        else {
            isPaused = true;
            PauseProcess();
        }
    }

    public void Freeze() {
        animator.SetFloat("InputMagnitude", 0f);
        thirdPersonController.enabled = false;
        thirdPersonInput.enabled = false;
    }

    public void FreezeInputOnly() { // Unsure if this works
        thirdPersonInput.enabled = false;
    }

    public void UnFreeze() {
        animator.SetFloat("InputMagnitude", 0f);
        thirdPersonController.enabled = true;
        thirdPersonInput.enabled = true;
    }

    void MiniPauseProcess() {
        isMiniPaused = true;
        pauseCanvasBackground.SetActive(false);
        StartCoroutine(CanvasPauseCoroutine(0));
    }

    public void MiniUnpauseProcess() {
        StartCoroutine(CanvasUnpauseCoroutine(0));
        pauseCanvasBackground.SetActive(true);
        isMiniPaused = false;
    }

    public void StartConversation() {
        Freeze();
        isInConversation = true;
    }

    public void LeaveConversation() {
        UnFreeze();
        MiniUnpauseProcess();
        isInConversation = false;
    }

    void PauseProcess() {
        animator.SetTrigger("PauseHit");
        animator.SetBool("isPaused", true);
        Freeze();
        StartCoroutine(CameraPauseCoroutine());
        StartCoroutine(JumpCoroutine());
        StartCoroutine(CanvasPauseCoroutine());
        menuAudioScript.MainToPause();

    }

    void UnPauseProcess() {
        animator.ResetTrigger("PauseHit");
        animator.SetBool("isPaused", false);
        UnFreeze();
        StartCoroutine(CameraUnpauseCoroutine());
        StartCoroutine(JumpBackCoroutine());
        StartCoroutine(CanvasUnpauseCoroutine());
        menuAudioScript.PauseToMain();
    }

    IEnumerator CameraPauseCoroutine()
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        pauseCam.gameObject.SetActive(true);
    }

    IEnumerator CameraUnpauseCoroutine() {
        pauseCam.gameObject.SetActive(false);

        yield return new WaitForSeconds(5);
        thirdPersonController.enabled = true;
        thirdPersonInput.enabled = true;

    }

    IEnumerator JumpCoroutine()
    {
        SaveTransform();
        yield return new WaitForSeconds(2);
        playerObject.transform.position = playerPausePoint.position;
        playerObject.transform.rotation = playerPausePoint.rotation;

    }

    IEnumerator JumpBackCoroutine()
    {

        yield return new WaitForSeconds(3);
        playerObject.transform.position = prevPosition;
        playerObject.transform.rotation = prevRotation;
    }

    IEnumerator CanvasPauseCoroutine(float waitTime=7)
    {
        mainCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        pauseCanvas.gameObject.SetActive(true);
        menuAnimationScript.OpenMenu();
    }

    IEnumerator CanvasUnpauseCoroutine(float waitTime=2)
    {
        menuAnimationScript.CloseMenu();
        pauseCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        mainCanvas.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMiniPaused && !isInConversation)
            {
                if (playerPausePoint != null)
                {
                    Process();
                }
            }
            else if (isMiniPaused && !isInConversation)
            {
                Debug.Log("Error. This state should not be possible");
            }
            else if (isMiniPaused && isInConversation)
            {
                MiniUnpauseProcess();
            }
            else if (isInConversation && !isMiniPaused)
            {
                MiniPauseProcess();
            }
            else {
                Debug.Log("Error. Impossible state");
            }
        }
    }
}
