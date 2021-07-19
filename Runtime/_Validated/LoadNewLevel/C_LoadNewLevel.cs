using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_LoadNewLevel : MonoBehaviour
{
    [Header("This will trigger a Level Load, sending the player to a new level as named below.")]
    [Header("Ensure you have added the specified map to the Build Settings list")]

    //Carve out a space for us to put the target object in.
    public string Levelname = null;
    public bool DestroyPlayerController = false;
    //
    private void OnTriggerEnter(Collider other)
    {
        //If the overlapping actor is the Player
        if (other.gameObject.tag == "Player")
        {
            if (DestroyPlayerController) 
            {
                Destroy(PlayerController.instance.gameObject);
            }
            SceneManager.LoadScene(Levelname, LoadSceneMode.Single);
        }
    }
    public void LoadNewLevelDirect(string LevelName)
    {
        if (DestroyPlayerController)
        {
            Destroy(PlayerController.instance.gameObject);
        }

        SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
    }
    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "magic-gate.png", true);
    }
    private void OnDrawGizmosSelected()
    {
        //This will draw a line to our Teleport Destination when the Gameobject this Component is attached to is selected in the Editor.
        if (Levelname != null)
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawLine(transform.position, TeleportDestination.transform.position);

        }
    }
}
