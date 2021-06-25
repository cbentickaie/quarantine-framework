using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_StunnedStatus : C_StatusEffect
{
    [SerializeField] float StunDuration = 2.4f;
    public override void ApplyNewStatus()
    {
        base.ApplyNewStatus();
        StartCoroutine(ApplyStun());
    }

    IEnumerator ApplyStun() 
    {
        Agent.speed = 0f;
        
        yield return new WaitForSeconds(StunDuration);
        Agent.speed = defaultSpeed;
        Destroy(this);
    }

    void OnDrawGizmos() 
    {
        //Gizmos.DrawIcon(this.transform.position, "")
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
