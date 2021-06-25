using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class C_StatusEffect : MonoBehaviour
{
    public NavMeshAgent Agent;
    public PrimitiveAIAgentController PrimAiCon;

    //Parameters on the Agent or Controller we wish to cache whenever attaching to an Agent
    public float defaultSpeed;


    // Start is called before the first frame update
    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        PrimAiCon = GetComponent<PrimitiveAIAgentController>();

        defaultSpeed = Agent.speed;
        ApplyNewStatus();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void ApplyNewStatus() 
    {
        print("APPLYING STATUS");
    }
}
