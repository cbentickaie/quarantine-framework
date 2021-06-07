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
            Debug.Log("WE DETECTED THE PLAYER");
        }
    }
}
