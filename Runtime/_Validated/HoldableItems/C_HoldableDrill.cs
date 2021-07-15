using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_HoldableDrill : C_HoldableItem
{
    float DamageAmount = 1.0f;
    [SerializeField]DamageTypes dType = DamageTypes.Drilling;
    
    [SerializeField] string idleAnimationTrigger = "Idle";
    [SerializeField] string activeAnimationTrigger = "Spin";

    public override void startUseItem()
    {
        base.startUseItem();
        StartCoroutine(traceForDrillingTarget());
        if (animationEnabled) 
        {
            AnimCon.SetTrigger(activeAnimationTrigger);
        }
        
    }

    public override void stopUseItem()
    {
        
        StopCoroutine(traceForDrillingTarget());
        StopAllCoroutines();
        base.stopUseItem();
        AnimCon.SetTrigger(idleAnimationTrigger);
    }

    IEnumerator traceForDrillingTarget()
    {
        RaycastHit InvHit;
        //print("DRILLING");
        
        //Debug.DrawRay(transform.position, (transform.forward * 3.14f), Color.blue, 1.314f);
        //if (Physics.Raycast(transform.position, transform.forward, out InvHit, 3.14f))
        //{
        //    Debug.DrawRay(transform.position, (transform.forward * 3.14f), Color.magenta, 1.314f);
        //    //Gizmos.DrawCube(InvHit.point, Vector3.one);
        //    assessDamage(InvHit);
        //}

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out InvHit, 3.14f))
        {
            Debug.DrawRay(transform.position, (transform.forward * 3.14f), Color.magenta, 1.314f);
            //Gizmos.DrawCube(InvHit.point, Vector3.one);
            assessDamage(InvHit);
        }


        yield return new WaitForSeconds(0.314f);
        StartCoroutine(traceForDrillingTarget());
    }

    void assessDamage(RaycastHit hit) 
    {
        DamageHandler dh;
        if (hit.collider.gameObject.GetComponent<DamageHandler>() != null)
        {
            dh = hit.collider.gameObject.GetComponent<DamageHandler>();
            dh.ReceiveDamage(DamageAmount, dType);
            print("Applying " + DamageAmount + "Damage to " + hit.collider.gameObject.name);
            //ApplyDamageToTarget(dh, DamageTypeToApply);
        }
    }
}
