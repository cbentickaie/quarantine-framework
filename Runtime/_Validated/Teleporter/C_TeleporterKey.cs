using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_TeleporterKey : MonoBehaviour
{
    public C_Teleporter TeleporterToUnlock;
    public bool useTrigger = true;
    public bool showPromptOnPickup = false;
    public string promptMessage = "This key has unlocked a Teleporter.";
    public float promptDuration = 3.14f;
    private void OnCollisionEnter(Collision collision)
    {
        if (!useTrigger)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (TeleporterToUnlock)
                {
                    TeleporterToUnlock.LockUnlockTeleporter(true);
                    Destroy(gameObject);
                }
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (useTrigger) 
        {
            if (other.gameObject.tag == "Player")
            {
                if (TeleporterToUnlock)
                {
                    if (showPromptOnPickup) 
                    {
                        PlayerHudManager.instance.DisplayPlayerMessage(promptMessage, promptDuration);
                    }
                    TeleporterToUnlock.LockUnlockTeleporter(true);
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "teleport.png", true);
    }
    private void OnDrawGizmosSelected()
    {
        //This will draw a line to our Teleport Destination when the Gameobject this Component is attached to is selected in the Editor.
        if (TeleporterToUnlock)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, TeleporterToUnlock.transform.position);

        }
    }
}
