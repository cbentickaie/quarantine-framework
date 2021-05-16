using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Teleporter : MonoBehaviour
{
    //Carve out a space for us to put the target object in.
    public GameObject TeleportDestination;
    public bool isLocked = false;

    [SerializeField]AudioSource tpAudioSrc;
    bool useAudio = false;
    public AudioClip TeleportSound;
    public AudioClip TeleportUnlockSound;
    public AudioClip TeleportLockedWarning;
    //[ExecuteInEditMode]
    private void Awake()
    {
        //LockUnlockTeleporter(!isLocked);
    }
    private void Start()
    {
        tpAudioSrc = GetComponent<AudioSource>();
        if (tpAudioSrc)
        {
            useAudio = true;
        }
        else { useAudio = false; }
    }
    private void OnTriggerEnter(Collider other)
    {
        //If the overlapping actor is the Player
        if (other.gameObject.tag == "Player" && !isLocked)
        {
            if (TeleportDestination) 
            {
                RemoveSpeedonTeleport(other.gameObject);
                other.gameObject.transform.position = TeleportDestination.transform.position;
                other.gameObject.transform.rotation = TeleportDestination.transform.rotation;
                PlayTeleportSound();
            }
            else 
            {
                Debug.Log("No teleport Destination selected, drop a Gameobject you want the player to Teleport to");
            }
            

        }
        if (other.gameObject.tag == "Player" && isLocked) 
        {
            PlayUnlockSound(false);
        }
    }
    void RemoveSpeedonTeleport(GameObject o)
    {
        Rigidbody rb = o.GetComponent<Rigidbody>();
        if (rb != null)
        {
            o.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        /*        CharacterController CharCon = playerCharacter.GetComponent<CharacterController>();
                if (CharCon != null) 
                {
                    CharCon.velocity = Vector3.zero;
                }*/
    }
    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "teleport.png", true);
    }
    public void LockUnlockTeleporter(bool unlock) 
    {
        if (unlock)
        {
            isLocked = false;
            gameObject.GetComponent<Collider>().enabled = true;
            if (useAudio) 
            {
                PlayUnlockSound(true);
            }
        }
        else if (!unlock) 
        {
            isLocked = true;
            gameObject.GetComponent<Collider>().enabled = false;
            PlayUnlockSound(false);
        }
    }
    void PlayTeleportSound() 
    {
        tpAudioSrc.clip = TeleportSound;
        tpAudioSrc.Play();
    }
    void PlayUnlockSound(bool isUnlocked) 
    {
        if (!isUnlocked)
        {
            tpAudioSrc.clip = TeleportLockedWarning;
        }
        else 
        {
            tpAudioSrc.clip = TeleportUnlockSound;
        }
        tpAudioSrc.Play();
    }
    private void OnDrawGizmosSelected()
    {
        //This will draw a line to our Teleport Destination when the Gameobject this Component is attached to is selected in the Editor.
        if (TeleportDestination)
        {
            if (isLocked)
            {
                Gizmos.color = Color.red;
            }
            else if (!isLocked) 
            {
                Gizmos.color = Color.green;
            }
            
            Gizmos.DrawLine(transform.position, TeleportDestination.transform.position);

        }
    }
}
