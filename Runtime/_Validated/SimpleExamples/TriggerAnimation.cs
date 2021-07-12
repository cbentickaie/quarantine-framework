using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] Animator AnimTarget;
    public string TriggerName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && TriggerName != null) 
        {
            AnimTarget.SetTrigger(TriggerName);
            Debug.Log("Animation State Triggered for: " + AnimTarget.name + ": " + TriggerName);
        }
    }

    public void triggerAnimationDirectly() 
    {
        AnimTarget.SetTrigger(TriggerName);
        Debug.Log("Animation State Triggered for: " + AnimTarget.name + ": " + TriggerName);
    }
}
