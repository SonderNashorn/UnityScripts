using UnityEngine;
using StateStuff;
using System.Collections.Generic;
using UnityEngine.AI;

public enum AiStates
{
    Idle,
    Watch
}
public class AI : MonoBehaviour
{
    public StateMachine<AI> stateMachine { get; set; }

    #region Variables
    //Character Transform
    [HideInInspector]
    public Transform character;
    
    [Header("AI PARAMETERS")]
    public List<Transform> aiWayPointList;
    
     //Public
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public AiType aiType;
    public AiStates aiState;   
    //
    public Transform eyes;
    [HideInInspector]
    public bool shouldStare;

    //Private
    private Transform watchTarget;

    //Spotting
    private float sightRange;
    private float sightRadius;

    //Waypoint
    private int nextWaypoint;
    private RaycastHit hit;

    //Timer related
    private float timeToStare;
    private float gameTimer;
    private float timeStaring;
    #endregion

    private void Start()
    {       
        //Ai Nav Mesh
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;        

        //State machine component
        stateMachine = new StateMachine<AI>(this);

        //preset for ai vision
        sightRadius = 6f; 
        sightRange = 10f; 
        
        shouldStare = true;        
        timeToStare = 5f; //how long should they stare
        timeStaring = 0f; //how long have they been staring
    }

    private void Update()
    {
        stateMachine.Update(); // this calls the different states update like any regular update
        CurrentAiState();// runs whichever state we are running
    }

    public void Patrol()
    {
        navMeshAgent.destination = aiWayPointList[nextWaypoint].position;
        Search(); //Boolean to search for tag: Player
        if (Vector3.Distance(gameObject.transform.position, aiWayPointList[nextWaypoint].transform.position) <= 0.5f)
        {
            
            nextWaypoint++;
            if (nextWaypoint == aiWayPointList.Count)
                nextWaypoint = 0;
        }
    } //Walk through preset Waypoints
        
    public bool Search()
    {
        Debug.DrawRay(eyes.position, eyes.forward.normalized * sightRange, Color.green);
        
        if (Physics.SphereCast(eyes.position, sightRadius, eyes.forward, out hit, sightRange)
            && hit.collider.CompareTag("Player") && shouldStare)
        {      
            return false;
        }
        return true;
    }//Boolean to search for tag: Player

    public void Stare()
    {          
        watchTarget = hit.transform;        
        Vector3 direction = (watchTarget.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        transform.LookAt(watchTarget.position);
    }//Follows the "watchTarget" hit by a RayCast.

    public void Timer()
    {
        gameTimer = Time.deltaTime;
        timeStaring += gameTimer ; 

        if (timeStaring >= timeToStare)
        {
            gameTimer = 0f;
            timeStaring = 0f;            
            shouldStare = !shouldStare;            
        }
    } // Counts till preset timer "timeToStare" 

    public void CurrentAiState()
    {

        switch (aiState)
        {
            default:
            case AiStates.Idle:
                {
                    if (stateMachine.currentState != IdleState.Instance)
                    {
                        stateMachine.ChangeState(IdleState.Instance);
                    }
                    break;
                }
            case AiStates.Watch:
                {
                    if (stateMachine.currentState != WatchState.Instance)
                    {
                        stateMachine.ChangeState(WatchState.Instance);
                    }
                    break;
                }
        }
    }//Heart of the Enumerator States are stored here.
}