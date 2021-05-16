using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPDoor_Key : MonoBehaviour {

    public MultiDoor doorToUnlock;
    public bool showPromptOnPickup = true;
    public string promptMessage = "You found a Door Key!";
    private void OnTriggerEnter(Collider collision)
    {    
        //when the player collides with this object
        if (collision.gameObject.tag == "Player")
        {

            //Check if this Key has no door associated with it.
            if (doorToUnlock)
            {
                //unlock my associated door
                doorToUnlock.UnlockDoor();

                if (PlayerHudManager.instance && showPromptOnPickup)
                {
                    PlayerHudManager.instance.DisplayPlayerMessage(promptMessage);
                }
                //destroy myself
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("This key is orphaned, it has no Door specified in the Inspector");
                if (PlayerHudManager.instance)
                {
                    PlayerHudManager.instance.DisplayPlayerMessage("This Key needs a Door specified!");
                }
            }            
        }
    }

    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "wooden-door.png", true);

    }
    private void OnDrawGizmosSelected()
    {
        if (doorToUnlock)
        {
            Gizmos.DrawLine(this.transform.position, doorToUnlock.transform.position);
        }
        
    }
}
