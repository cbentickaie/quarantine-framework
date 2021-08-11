using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_RegionalInputInteractor : MonoBehaviour
{
    [SerializeField]bool isPlayerInRegion = false;

    public UnityEvent InteractionEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            isPlayerInRegion = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPlayerInRegion) 
        {
            if (Input.GetKeyDown("j")) 
            {
                //do action we ask for
                print("Player Interacted");
                InteractionEvent.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRegion = false;
        }
    }
}
