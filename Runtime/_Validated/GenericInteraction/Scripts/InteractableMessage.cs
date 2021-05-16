using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractableMessage : MonoBehaviour
{
    
    public string message = "Interactable Message";
    Interactable myInteractable;
    [ExecuteInEditMode]
    private void Awake()
    {
        if (myInteractable = GetComponent<Interactable>())
        {
            Debug.Log(message);
        }
        else 
        {
            enabled = false;
        }
    }

}
