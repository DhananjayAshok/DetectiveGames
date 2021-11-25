using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentWandererScript : MonoBehaviour
{
    public AudioGroup playerInteractionClips;
    public AudioGroup atGoalClips;
    public AudioGroup movingClips;
    public GoalSet goalSet;
    public float waitTimeAtWaypoint=5;
    public float speed=1;
    public int noDestinationAnimationClips=3;
    public float randomizationTime=5f;
    AudioSource audioSource;
    Animator animator;
    bool isMoving = false;
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    //float epsilon = 0.1f;
    //bool timeUp = false;
    bool playingPlayerClip;
    bool noGoals;
    float lastRandomized;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (navMeshAgent != null) {
            navMeshAgent.speed = speed;
        }
        if (!goalSet.noGoals() && navMeshAgent != null)
        {
            Move();
        }
        else {
            noGoals = true;
            isMoving = false;
            if (animator != null) {
                animator.SetInteger("RNGInt", Random.Range(0, noDestinationAnimationClips));
                animator.SetBool("isMoving", false);
                lastRandomized = Time.time;
            }
        }
    }

    void Move() {
        isMoving = true;
        navMeshAgent.destination = goalSet.TransitionWaypoint().position;
        if (animator != null) {
            animator.SetBool("isMoving", true);
        }
        if (movingClips != null && movingClips.getNoClips() > 0) {
            audioSource.clip = movingClips.Sample();
            audioSource.Play();
        }
    }

    void ReachDestination() {
        isMoving = false;
        //timeUp = false;
        if (animator != null) {
            animator.SetInteger("RNGInt", Random.Range(0, noDestinationAnimationClips));
            animator.SetBool("isMoving", false);
        }
        if (atGoalClips != null && atGoalClips.getNoClips() > 0 && !playingPlayerClip)
        {
            audioSource.clip = atGoalClips.Sample();
            audioSource.Play();
        }
        StartCoroutine(ReachDestinationCoroutine());
    }


    IEnumerator ReachDestinationCoroutine() {
        yield return new WaitForSeconds(waitTimeAtWaypoint);
        //timeUp = true;
        Move();
    }

    public void OnTriggerEnter(Collider collider) {
        if (!playingPlayerClip) {

            if (collider.tag == "Player" || collider.tag == "InteractionSphere")
            {
                StartCoroutine(PlayerInteractionCoroutine());
            }
        }
    }

    IEnumerator PlayerInteractionCoroutine()
    {
        playingPlayerClip = true;
        if (playerInteractionClips != null && playerInteractionClips.getNoClips() > 0)
        {
            audioSource.clip = playerInteractionClips.Sample();
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length + 2f);
        }
        playingPlayerClip = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (!noGoals && navMeshAgent != null)
        {
            if (isMoving)
            {
                if (!navMeshAgent.pathPending)
                {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            isMoving = false;
                            ReachDestination();
                        }
                    }
                }
            }
        }
        else {
            if (lastRandomized + randomizationTime > Time.time) {
                animator.SetInteger("RNGInt", Random.Range(0, noDestinationAnimationClips));
                animator.SetBool("isMoving", true);
                animator.SetBool("isMoving", false);
            }
            
        }

    }
}
