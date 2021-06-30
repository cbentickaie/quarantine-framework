using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_HoldableMagnifyingGlass : C_HoldableItem
{

    public override void startUseItem()
    {
        base.startUseItem();
        StartCoroutine(traceForInvestigation());
    }

    public override void stopUseItem()
    {
        StopCoroutine(traceForInvestigation());
        StopAllCoroutines();
        base.stopUseItem();
        
        
    }

    IEnumerator traceForInvestigation()
    {
        RaycastHit InvHit;
        
        
        Debug.DrawRay(transform.position, (transform.forward * 3.14f), Color.blue, 1.314f);
        if (Physics.Raycast(transform.position, transform.forward * 3.14f, out InvHit, 3.14f))
        {
            Debug.DrawRay(transform.position, (transform.position + transform.forward * 3.14f), Color.magenta, 1.314f);
        }
        yield return new WaitForSeconds(0.314f);
        StartCoroutine(traceForInvestigation());
    }
}
