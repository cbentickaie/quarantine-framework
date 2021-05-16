using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDepositPickup : MonoBehaviour
{
    PlayerController Pcon;
    public PickupTypes pitstopType;

    public float AmountToAdd = 1.0f;

    private void Start()
    {
        if (PlayerController.instance)
        {
            Pcon = PlayerController.instance;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Detected Player Overlap");
            
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Detected Player Leaving Zone");
        }

       
    }

    private void OnTriggerStay(Collider other)
    {
        Pcon.ApplyPickuptoPlayerCon(pitstopType, AmountToAdd * Time.deltaTime);
    }
}
