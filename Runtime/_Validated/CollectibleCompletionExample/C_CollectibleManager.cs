using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_CollectibleManager : MonoBehaviour
{

    //Declare the variables relevant to this scoring system
    public int requiredItemsForCompletion = 3;
    [SerializeField] int currentItemsCollected = 0;

    //Generic Completion Event that can be subscribed to or specified in the Inspector.
    public UnityEvent CompletionEvent;

    //OPTIONAL ANIMATION STUFF
    [SerializeField] bool useAnimationOnCOmpletion;
    public Animator AnimController;
    //Ensure you put the CORRECT trigger name from you animation controller into the Inspector Field for AnimationTriggername
    public string AnimationTriggername = "StartCinematic";
    public bool switchPlayerCameraOnAnimation = false;
    
    public void registerItemPickedUp() 
    {
        print("Item Registered as Collected");
        currentItemsCollected += 1;

        if (currentItemsCollected >= requiredItemsForCompletion) 
        {
            print("ALL ITEMS COLLECTED CONGRATUALTIONS!!!");            
            PerformCompletionEvents();
        }

    }

    void PerformCompletionEvents() 
    {
        if (useAnimationOnCOmpletion) 
        {
            AnimController.SetTrigger(AnimationTriggername);

            //HACK ZONE PLEASE DONT JUDGE ME
            if (switchPlayerCameraOnAnimation)
            {
                Camera tempPlayerCam;
                tempPlayerCam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
                tempPlayerCam.enabled = false;

                //Reach out and find the camera in my animation and turn it on
                /*            Camera animCam;
                            animCam = AnimController.gameObject.GetComponentInChildren<Camera>();
                            animCam.enabled = true;*/
                //Apply the Gameobject Active flag WITHIN my animation - much easier
            }
            else
            {
                //use this for any alternative functionality you may want
            }
        }
        //Generic Completion Event that can be subscribed to or specified in the Inspector.
        CompletionEvent.Invoke();
    }
}
