using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ShockedStatus : C_StatusEffect
{
    [SerializeField] float ShockDuration = 4.4f;
    Rigidbody RB;
    public override void Start() 
    {
        base.Start();
        RB = Agent.gameObject.GetComponent<Rigidbody>();
    }

    public override void ApplyNewStatus()
    {
        base.ApplyNewStatus();
        StartCoroutine(ApplyShock());
    }

    IEnumerator ApplyShock()
    {
        Agent.speed = 0f;
        if (RB) 
        {
            StartCoroutine(Shockimpulse());
        }        
        yield return new WaitForSeconds(ShockDuration);
        Agent.speed = defaultSpeed;
        Destroy(this);
    }

    IEnumerator Shockimpulse() 
    {
        print("Shocking");
        RB.AddForce(Random.onUnitSphere, ForceMode.Impulse);
        yield return new WaitForSeconds(Random.Range(.05f, .5f));
        StartCoroutine(Shockimpulse());
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawIcon(this.transform.position, "")
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
