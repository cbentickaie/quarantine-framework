using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    [HideInInspector]public InteractionTypes interactionType;
    

    [Tooltip("Arbitary text message")]

    public bool ShowMessageOnInteract = false;
    public string message = "Interaction Started";
    bool audioEnabled = false;
    AudioSource SoundSource;
    //Event Support
    public UnityEvent interactionStartedEvent;

    private void Awake()
    {
        if (!SoundSource)
        {
            if (SoundSource = gameObject.GetComponent<AudioSource>())
                audioEnabled = true;
        }
        else if (SoundSource) 
        {
            audioEnabled = true;
        }
    }
    public virtual void StartInteraction(bool isPlayer = false)
    {
        interactionStartedEvent.Invoke();
        if (ShowMessageOnInteract) 
        {
            showMessageonInteraction(message);
        }
        if (audioEnabled) 
        {
            SoundSource.Play();
        }
    }

    public virtual void StopInteraction(bool isPlayer = false)
    {
      //  print("Interaction Stopped");
    }

    public virtual void TickInteraction()
    {
        // print("Interaction Updated" + Time.deltaTime);
    }

    public virtual void StartHover()
    {
        print("Hover Started");
    }

    public virtual void StopHover()
    {
        print("Hover Stopped");
    }
    /*            //   print("Interaction Started");
            switch (interactionType)
            {
                case InteractionTypes.GenericInteract:

                    break;

                case InteractionTypes.EnableDisableObject:

                    break;

                case InteractionTypes.SwapObject:

                    break;
            }*/

    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "lever.png", true);
        
    }
    private void OnDrawGizmosSelected()
    {

    }

    void showMessageonInteraction(string inmessage) 
    {
        PlayerHudManager.instance.DisplayPlayerPrompt(inmessage, false);
    }
}
