using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{

    public AudioGroup teleportClips;
    [Header("Internal Variables (Can ignore)")]
    public float coolDownTime = 3f;
    public Transform pOther;
    AudioSource audioSource;
    bool noClips;
    float nextActivate;
    TeleporterScript tOther;
    GameObject player;
    GameObject playerAttachment;
    PlayerPauseScript playerPauseScript;

    // Start is called before the first frame update
    void Start()
    {
        noClips = teleportClips.getNoClips() == 0;
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttachment = GameObject.FindGameObjectWithTag("Player Attachment");
        playerPauseScript = playerAttachment.GetComponent<PlayerPauseScript>();
        tOther = pOther.gameObject.GetComponent<TeleporterScript>();
        UpdateCooldown();
    }

    public void Teleport() {
        if (Time.time > nextActivate) {
            PerformTeleport();
            UpdateCooldown();
        }
        tOther.UpdateCooldown();
    }

    public void PerformTeleport() {
        if (noClips)
        {
        }
        else
        {
            audioSource.clip = teleportClips.Sample();
            audioSource.Play();
        }
        StartCoroutine(Teleportation());
    }

    IEnumerator Teleportation() {
        playerPauseScript.Freeze();
        player.transform.position = pOther.position;
        yield return new WaitForSeconds(0.5f);
        playerPauseScript.UnFreeze();
    }

    public void UpdateCooldown() {
        nextActivate = Time.time + coolDownTime;
    }

    public void Update() {
    }
}
