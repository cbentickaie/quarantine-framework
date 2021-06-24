using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTileScript : MonoBehaviour {

    public enum TileType { solid, collapsing, hurt }

    public TileType current_tileType;
    GameObject playerObj;
    public float damageLevel = 10.0f;

    public float CollapseDelay = 1.0f;
    public float HurtDelay = 0.1f;
    public float ResetDelay = 1.0f;

    Vector3 StartLocation;
    Quaternion StartRotation;
    // Use this for initialization
    void Start () 
    {
        StartLocation = transform.position;
        StartRotation = transform.rotation;
    }
	


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            playerObj = collision.gameObject;
            DetermineEffect();
        }
    }

    void DetermineEffect() 
    {
        switch (current_tileType)
        {
            case TileType.solid:
                DoYesEffect();
                break;
            case TileType.collapsing:
                
                Invoke("DoNoEffect", CollapseDelay);
                break;
            case TileType.hurt:
                Invoke("DoMaybeEffect", HurtDelay);
                break;
        }
    }

    void DoYesEffect()
    {
        Debug.Log("YESSSSS!");
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        Invoke("ResetTile", ResetDelay);
    }

    void DoNoEffect()
    {
        Rigidbody tileRB = gameObject.AddComponent<Rigidbody>();
        //tileRB.AddForce(transform.up * -1.0f * 3000, ForceMode.Impulse);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        Invoke("ResetTile", ResetDelay);
    }

    void DoMaybeEffect()
    {
        Rigidbody tileRB = gameObject.AddComponent<Rigidbody>();
        //tileRB.AddForce(transform.up * 3000, ForceMode.Impulse);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        DamageHandler DmgH = playerObj.GetComponent<DamageHandler>();
        if (DmgH) 
        {
            DmgH.ReceiveDamage(damageLevel);
        }
        Invoke("ResetTile", ResetDelay);
    }

    void ResetTile() 
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        transform.position = StartLocation;
        transform.rotation = StartRotation;
    }
}
