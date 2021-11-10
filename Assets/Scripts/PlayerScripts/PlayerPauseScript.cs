using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;
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
    public bool isPaused;
    public bool isMiniPaused;
    public Canvas mainCanvas;
    public Canvas pauseCanvas;
    public GameObject pauseCanvasBackground;

    public Transform playerPausePoint;
    public Camera pauseCam;

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

    void MiniPauseProcess() {
        isMiniPaused = true;
        thirdPersonController.enabled = false;
        thirdPersonInput.enabled = false;
        pauseCanvas.gameObject.SetActive(true);
        pauseCanvasBackground.SetActive(false);
    }

    void MiniUnpauseProcess() {
        thirdPersonController.enabled = true;
        thirdPersonInput.enabled = true;
        pauseCanvas.gameObject.SetActive(false);
        pauseCanvasBackground.SetActive(true);
        isMiniPaused = false;
    }

    void PauseProcess() {
        animator.SetTrigger("PauseHit");
        animator.SetBool("isPaused", true);
        thirdPersonController.enabled = false;
        thirdPersonInput.enabled = false;
        StartCoroutine(CameraPauseCoroutine());
        StartCoroutine(JumpCoroutine());
        StartCoroutine(CanvasPauseCoroutine());
        menuAudioScript.MainToPause();

    }

    void UnPauseProcess() {
        animator.ResetTrigger("PauseHit");
        animator.SetBool("isPaused", false);
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

    IEnumerator CanvasPauseCoroutine()
    {
        mainCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(7);

        pauseCanvas.gameObject.SetActive(true);
    }

    IEnumerator CanvasUnpauseCoroutine()
    {
        pauseCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        mainCanvas.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMiniPaused)
        {
            Process();
        }
    }
}
