using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class C_SecurityNavBehaviours : MonoBehaviour
{
    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //nav.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    public void PursueTargetActor() 
    {
        nav.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    }
}
