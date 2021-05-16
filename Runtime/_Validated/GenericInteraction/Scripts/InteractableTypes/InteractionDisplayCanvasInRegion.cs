using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDisplayCanvasInRegion : Interactable
{
    //public GameObject targetGameObject;
    public GameObject TgtObjectCanvas;
    public float maxDistance = 2.0f;
    Vector3 interactionStartLoc;
    [ExecuteInEditMode]
    private void Awake()
    {
        
    }
    private void Start()
    {
       
    }
    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();
        TgtObjectCanvas.SetActive(true);
        interactionStartLoc  = GameObject.FindGameObjectWithTag("Player").transform.position;
        InvokeRepeating("trackInteractorDistance", 0.1f, 0.314f);
        
    }

    void trackInteractorDistance() 
    {
        if ((interactionStartLoc - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude > maxDistance) 
        {
            TgtObjectCanvas.SetActive(false);
            StopAllCoroutines();
        }
        print((interactionStartLoc - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude);
    }
}
