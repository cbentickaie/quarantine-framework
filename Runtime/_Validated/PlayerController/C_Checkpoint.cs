using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            PlayerController.instance.transform.position = transform.position;
            GetComponent<Collider>().enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "wooden-sign.png", true);

    }
}
