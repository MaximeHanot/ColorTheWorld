using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{

    [Header("Character Movement")]
    public List<GameObject> targets;
    private int destPoint = 0;
    NavMeshAgent agent;

    [SerializeField] private float detectionDistance = 8f;
    [SerializeField] private float outDetectionDistance = 15f;
    [SerializeField] private LayerMask detectionLayerMask;

    public float findTime = 3.0f;
    private float findingTimer = 0f;
    public float standTime = 3.0f;
    private float standingTimer = 0f;

    GameObject player;

    public static State currentState = State.isWalking;
    public static State previousState;
    public enum State
    {
        isWalking,
        isDetecting,
        isFollowing,
        isStanding
    }

    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        GotoNextPoint();
    }

    private void Update()
    {
        UpdateState();

        //Debug.Log(currentState);
    }

    void GotoNextPoint()
    {
        if (targets.Count == 0)
            return;

        agent.destination = targets[destPoint].transform.position;
        destPoint = (destPoint + 1) % targets.Count;
    }

    void StopAgent() { agent.isStopped = true; }
    void StartAgent()  { agent.isStopped = false; }

    public float PlayerDistance()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        Vector3 startRaycastPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= detectionDistance)
            Debug.DrawRay(startRaycastPosition, playerDirection, Color.green); // DEPART DE LA DETECTION DETECTION
        else if (distance > detectionDistance && distance <= outDetectionDistance)
            Debug.DrawRay(startRaycastPosition, playerDirection, Color.blue); // POURSUITE
        else
            Debug.DrawRay(startRaycastPosition, playerDirection, Color.red); // TROP LOIN

        return distance;            
    }

    void FollowPlayer(float stopDistance)
    {
        Vector3 positionAroundPlayer = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        agent.destination = positionAroundPlayer;

        if (PlayerDistance() < stopDistance)
            StopAgent();
        else
            StartAgent();
    }

    private void ChangeState(State newState, bool forced = false)
    {
        if (newState != currentState || forced)
        {
            OnExitState();
            previousState = currentState;
            currentState = newState;
            OnEnterState();
        }
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case State.isWalking:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GotoNextPoint();

                if (PlayerDistance() <= detectionDistance)
                    ChangeState(State.isDetecting);

                    break;
            case State.isDetecting:
                FollowPlayer(rdmDistance);

                if ( PlayerDistance() > detectionDistance && PlayerDistance() <= outDetectionDistance)
                    ChangeState(State.isFollowing);
                if (PlayerDistance() > outDetectionDistance)
                    ChangeState(State.isStanding);

                break;
            case State.isFollowing:
                FollowPlayer(rdmDistance);
                findingTimer += Time.deltaTime;

                if (findingTimer >= findTime)
                    ChangeState(State.isStanding);

                break;
            case State.isStanding:
                standingTimer += Time.deltaTime;

                if (standingTimer >= standTime)
                    ChangeState(State.isWalking);

                 break;
        }
    }

    float rdmDistance = 0f;
    private void OnEnterState()
    {
        switch (currentState)
        {
            case State.isWalking:
                StartAgent();
                GotoNextPoint(); 
                break;
            case State.isDetecting:
                rdmDistance = Random.Range(2f, 4f);
                StartAgent();
                break;
            case State.isFollowing:
                rdmDistance = Random.Range(2f, 4f);
                StartAgent();
                break;
            case State.isStanding:
                StopAgent();
                break;
        }
    }
    private void OnExitState()
    {
        switch (currentState)
        {
            case State.isWalking:
                break;
            case State.isDetecting:
                break;
            case State.isFollowing:
                findingTimer = 0f;
                break;
            case State.isStanding:
                standingTimer = 0f;
                break;
        }
    }
}
