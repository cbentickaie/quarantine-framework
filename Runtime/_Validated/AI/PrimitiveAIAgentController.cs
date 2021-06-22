using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AiStates {Idle, Wander, WaypointPatrol, Investigating, Alert, Chasing, RangedAttack, MeleeAttack, Retreating }

public class PrimitiveAIAgentController : MonoBehaviour
{
    [Header("Runtime Info")]
    
    [SerializeField] Vector3 destination;
    private UnityEngine.AI.NavMeshAgent agent;
    public float DestinationAcceptRadius = 2.0f;
    //public float MeleeAttackRange = 1.0f;
    [SerializeField] AiStates currentState = AiStates.Wander;
    [SerializeField] AiStates previousState = AiStates.Idle;

    [Header("Agent Sensing Parameters")]
    [SerializeField] bool seeHostileTarget = false;
    public float AgentSightDistance = 15.0f;
    //public float AgentHearingDistance = 5.0f;
    [Header("Agent Vision Parameters")]
    //Vision related Parameters
    [SerializeField] float eyeHeight = 1.0f;
    float sightNoticeTime = 0.8f;
    [SerializeField]float defaultsightNoticeTime = 1.2f;
    float sightDecayTime = 1.6f;
    float defaultSightDecay = 1.6f;

    [SerializeField] float currentSight = 0.0f;

    [Header("Waypoints")]
    Vector3 HomeLocation;
    [SerializeField] int CurrentWaypointIndex = 0;
    public float MaxChaseRange = 12.0f;
    float distanceToDest = 9999.0f;

    [Header("Hostile Target Info")]
    //hostile target tracking parameters
    [SerializeField] GameObject HostileTarget;
    [SerializeField] float distanceToTarget = 9999.0f;
    [SerializeField]Vector3 lastSeenPosition;

    [Header("Waypoint Movement Variables")]
    public Transform[] Waypoints;

    [Header("Melee Attack Variables")]
    //Melee Attack Vars
    float meleeAttackDuration = 1.2f;
    public float meleeAttackDistance = 2.5f;
    //MeleeWeaponScript meleeWeapon;
    bool meleeEnabled = false;
    [SerializeField] ParticleSystem meleeDamageFx;
    [SerializeField] float meleeDamage = 8.0f;



    float stateAge;



    // Start is called before the first frame update
    void Start()
    {
        initAI();
        HostileTarget = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SwapAiState(currentState, true));
    }

    void initAI()
    {
        //Store Agent's initial location as 'Home'
        HomeLocation = transform.position;      

        // Cache agent component and destination
        agent = GetComponent<NavMeshAgent>();
        if (!agent)
        {
            print("agent is missing, please add a Nav Mesh Agent component to this GameObject");
            this.enabled = false;
        }
    }

    IEnumerator SwapAiState(AiStates newstate, bool forceState = false)
    {
        if (newstate != currentState || forceState)
        {
            previousState = currentState;
            switch (newstate)
            {
                case AiStates.Idle:
                    yield return new WaitForSeconds(1.6f);
                    print("Idle loopin");
                    break;

                case AiStates.Wander:
                    //yield return new WaitForSeconds(0.5f);
                    FindNewRandomDestination();
                    print("Wandering");
                    break;

                case AiStates.Chasing:
                    chaseTarget();
                    print("Pursuing Target");
                    break;

                case AiStates.Retreating:
                    FindRetreatPosition();
                    break;

                case AiStates.Investigating:
                    agent.SetDestination(lastSeenPosition);
                    break;
                case AiStates.MeleeAttack:
                    StartCoroutine(doMeleeAttack());
                    break;
                case AiStates.WaypointPatrol:
                    nextWaypoint();
                    break;

            }
            currentState = newstate;
        }
    }

    IEnumerator waitAtLocation(float duration)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        //update Hostile Target info
        if (!HostileTarget)
        {
            StartCoroutine(SwapAiState(AiStates.Idle));
        }

        // Update destination if the target is beyond 1 unit from the agent
        if (Vector3.Distance(destination, agent.transform.position) > DestinationAcceptRadius)
        {
            //destination = target.position;
            //agent.destination = destination;
            //SwapAiState(currentState);
        }

        else
        {
            // Invoke("FindNewRandomDestination", Random.Range(1.5f, 2.0f)); 
           // StartCoroutine(SwapAiState(currentState, true));
        }



        switch (currentState)
        {
            case AiStates.Idle:                
                print("Idle loopin");
                break;

            case AiStates.Wander:
                sphereLineOfSight();
                //yield return new WaitForSeconds(0.5f);
                if (hasReachedDestination())
                {
                    FindNewRandomDestination();
                    StartCoroutine(waitAtLocation(Random.Range(1.0f, 3.14f)));
                }
                else
                {
                    //print(distanceToDest);
                }
                //FindNewRandomDestination();
                //print("Wandering");
                break;

            case AiStates.Chasing:
                agent.updateRotation = true;
                sphereLineOfSight();
                chaseTarget();
                print("Pursuing Target");                               
                break;

            case AiStates.Retreating:
                FindRetreatPosition();
                break;

            case AiStates.Alert:
                sphereLineOfSight();
                break;

            case AiStates.Investigating:
                sphereLineOfSight();
                break;

            case AiStates.MeleeAttack:
                agent.updateRotation = false;
                Vector3 hostileLoc = new Vector3(HostileTarget.transform.position.x, agent.GetComponent<Collider>().bounds.center.y, HostileTarget.transform.position.z);

                //This line forces the actor to rotate and face the player on the xz plane
                agent.transform.rotation = Quaternion.LookRotation(hostileLoc - agent.GetComponent<Collider>().bounds.center, Vector3.up);
                break;

            case AiStates.WaypointPatrol:
                sphereLineOfSight();
                if (hasReachedDestination())
                {
                    nextWaypoint();
                }
                break;
        }
    }


    void FindNewRandomDestination()
    {
        print("New Random Dest Requested");
        if (RandomPoint(agent.transform.position, 10.0f, out destination))
        {
            agent.SetDestination(destination);
        }
        
    }

    void FindPositionNearTarget(GameObject tgtObject)
    {
        print("New Random Dest Requested");
        if (RandomPoint(HostileTarget.transform.position, 10.0f, out destination))
        {
            agent.SetDestination(destination);            
        }
    }

    void FindRetreatPosition()
    {
        RandomPoint(agent.transform.position, 10.0f, out destination);
    }

    void nextWaypoint()
    {
        if (CurrentWaypointIndex + 1 < Waypoints.Length)
        {
            CurrentWaypointIndex++;
        }
        else
        {
            CurrentWaypointIndex = 0;
        }

        StartCoroutine(waitAtLocation(1.4f));
        destination = (Waypoints[CurrentWaypointIndex].position);
        agent.SetDestination(destination);
    }
    /// This method generates a number of randomised positions in a sphere around the agent, then samples that position for vailidity on the navmesh using the SamplePosition method.
    bool RandomPoint(Vector3 center, float range, out Vector3 result)

    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * Random.Range(2.0f, range); //range;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                result = hit.position;
                Debug.DrawLine(hit.position, hit.position + new Vector3(0, 2, 0));
                return true;                
            }
        }
        result = Vector3.zero;
        return false;
    }

    [SerializeField] LayerMask visibilityLayerMask;
    //This method uses a spherecast between the agent and the HostileTarget's position (with an added offset)
    void sphereLineOfSight()
    {
        RaycastHit hit;

        Vector3 p1 = agent.transform.position + (transform.up * eyeHeight);
        float distanceToObstacle = 0;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        if (HostileTarget)
        {
            Debug.DrawRay(p1, ((HostileTarget.GetComponent<Collider>().bounds.center - p1).normalized * 150.0f), Color.green);
            if (Physics.SphereCast(p1, 0.5f, (HostileTarget.GetComponent<Collider>().bounds.center - p1), out hit, AgentSightDistance))
            {
                distanceToObstacle = hit.distance;
                if (hit.collider.gameObject == HostileTarget.gameObject)
                {
                    if (!seeHostileTarget) 
                    {
                        seeHostileTarget = true;
                        lastSeenPosition = hit.point;
                        StartCoroutine(SwapAiState(AiStates.Chasing));
                    }

                    currentSight += Time.fixedDeltaTime;

                    if (currentSight >= defaultsightNoticeTime)
                    {
                        currentSight = defaultsightNoticeTime;
                        
                    }


                    Debug.DrawLine(p1, hit.point, Color.red, Time.deltaTime);
                    print(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject != HostileTarget || !hit.collider)
                {
                    if (seeHostileTarget)
                    {
                        seeHostileTarget = false;
                        lastSeenPosition = HostileTarget.transform.position;
                        SwapAiState(AiStates.Investigating);

                    }
                    else if (!seeHostileTarget) 
                    {
                        //hold off on losing sight until timer expired
                        

                        Debug.DrawRay(p1, ((HostileTarget.GetComponent<Collider>().bounds.center - p1).normalized * 150.0f), Color.yellow);
                        if ((currentSight - Time.deltaTime) <= 0)
                        {
                            currentSight = 0;
                            seeHostileTarget = false;
                            StartCoroutine(SwapAiState(previousState));
                        }
                        else if (currentSight > 0)
                        {
                            currentSight -= Time.deltaTime;
                        }

                    }

                    
                    //lastSeenPosition =

                }
            }
        }
    }

    void chaseTarget()
    {
        tickDistanceToTarget();
        destination = HostileTarget.transform.position + (HostileTarget.transform.forward * (meleeAttackDistance * 0.5f));

        //Offset Hostile Position from player to prevent 'crowding'
        //destination = HostileTarget.transform.position + (HostileTarget.transform.forward * (meleeAttackDistance * 0.5f));

        //offset destination based upon approach direction
        //destination = HostileTarget.transform.position + ( HostileTarget.transform.position + (HostileTarget.transform.position - transform.position).normalized * (meleeAttackDistance * 0.5f));
        
        //FindPositionNearTarget(HostileTarget);
        agent.SetDestination(destination);
        //agent.
    }



    void tickDistanceToTarget()
    {
        distanceToTarget = Vector3.Distance(HostileTarget.transform.position, agent.transform.position);

        //test whether the current HostileTarget is within X units (EnemySightDistance) of the agent
        if (distanceToTarget < meleeAttackDistance)
        {
            StartCoroutine(SwapAiState(AiStates.MeleeAttack));
        }
    }

    bool hasReachedDestination()
    {
        distanceToDest = Vector3.Distance(destination, agent.transform.position);

        //test whether the current Destination is within X units (DestinationAcceptRadius) of the agent
        if (distanceToDest < DestinationAcceptRadius)
        {
            //agent.isStopped = true;
            print("Agent Reached Destination");
            
            return true;
            
            //chase the HostileTarget by setting our destination to the HostileTarget location evey frame
            //  destination = GameObject.FindGameObjectWithTag("HostileTarget").transform.position;
        }
        else
        {
            return false;
        }
    }

    

    IEnumerator doMeleeAttack()
    {

        print("Melee Attacking");
        RaycastHit meleeHit;
        if (Physics.BoxCast(GetComponent<Collider>().bounds.center, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out meleeHit, transform.rotation, meleeAttackDistance))
        {
            Debug.DrawRay(GetComponent<Collider>().bounds.center, transform.forward, Color.cyan);
            print("DAMAGIN!!");
            DamageHandler dh;
            if (dh = meleeHit.collider.gameObject.GetComponent<DamageHandler>())
            {
                dh.ApplyDamage(meleeDamage);
                if (meleeDamageFx)
                {
                    ParticleSystem DamageParticles = (ParticleSystem)Instantiate(meleeDamageFx, gameObject.GetComponent<Collider>().bounds.center, transform.rotation) as ParticleSystem;
                }
            }
            
        }
        yield return new WaitForSeconds(meleeAttackDuration);
        StartCoroutine(SwapAiState(previousState));

        /*if (distanceToTarget < meleeAttackDistance)
        {
            StartCoroutine(SwapAiState(AiStates.MeleeAttack));
        }
        else if (distanceToTarget > meleeAttackDistance)
        {
            StartCoroutine(SwapAiState(AiStates.Chasing));
        }
        else
        {
            StartCoroutine(SwapAiState(previousState));
        } */

        //if (meleeWeapon)
        //{
        //    meleeWeapon.AIMeleeAttack();
        //}
        //yield return new WaitForSeconds(meleeAttackDuration);

    }

    IEnumerator doRangeAttack()
    {
        yield return new WaitForSeconds(2.4f);

    }
}
